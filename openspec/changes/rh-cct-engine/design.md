# Design — rh-cct-engine

## Modelo polimórfico de regra

```
┌──────────────────────────┐
│      Convencao           │
│  (cabeçalho)             │
└──────────┬───────────────┘
           │ 1:N
           ▼
┌──────────────────────────┐    parametros_json schema varia por tipo:
│      RegraConvencao      │
│  tipo: ENUM              │    ┌──────────────────────────────┐
│  parametros_json: JSON   │    │ AdicionalHeDiurnoPct:        │
│  ordem: INT              │    │   { pct: 60, comDsr: true }  │
│  condicao_dsl: TEXT?     │    └──────────────────────────────┘
└──────────────────────────┘    ┌──────────────────────────────┐
                                │ PisoSalarialPorCbo:          │
                                │   { codigosCbo:["212405"],   │
                                │     salarioMinimo: 4500.00 } │
                                └──────────────────────────────┘
                                ┌──────────────────────────────┐
                                │ AnueniePct:                  │
                                │   { pctPorAno: 1,            │
                                │     tetoAnos: 25 }           │
                                └──────────────────────────────┘
                                etc.
```

Cada tipo tem **handler dedicado** no engine:

```csharp
public interface IRegraConvencaoHandler<TParam>
{
    string Tipo { get; }
    Task<RegraAplicacao> AplicarAsync(TParam parametros, ContextoFuncionarioFolha ctx);
}

// Registro automático por reflexão; engine itera handlers em ordem.
```

## Resolução da convenção aplicável

```csharp
public sealed class ResolvedorConvencao
{
    public async Task<ConvencaoResolvida?> ResolverAsync(Guid funcionarioId, string competencia)
    {
        // 1. Override individual?
        var ov = await _repo.ObterOverrideVigenteAsync(funcionarioId, competencia);
        if (ov != null) return new ConvencaoResolvida(ov.ConvencaoId, ov.MotivoOverride);

        // 2. Adesão da empresa do funcionário?
        var empId = funcionario.EmpresaId;
        var ades = await _repo.ListarAdesoesVigentesAsync(empId, competencia);

        // 3. Se múltiplas, junta em ordem (última prevalece para regra duplicada)
        if (ades.Count == 0) return null;
        return new ConvencaoResolvida(ades.Select(a => a.ConvencaoId).ToList());
    }
}
```

## Integração com W6 (engine de folha)

```csharp
// Em PreparadorContextoFolha (W6)
var cct = await _resolvedorConvencao.ResolverAsync(funcId, competencia);
ctx.Convencao = cct;

// Em EngineFolhaMensal.Calcular (W6):
if (ctx.Convencao != null)
{
    foreach (var regra in ctx.Convencao.Regras.OrderBy(r => r.Ordem))
    {
        var handler = _handlerRegistry.Get(regra.Tipo);
        await handler.AplicarAsync(regra.Parametros, ctx);
    }
}
```

Handler exemplo:
```csharp
public class HandlerAdicionalHeDiurnoPct : IRegraConvencaoHandler<AdicionalHeDiurnoPctParam>
{
    public string Tipo => "AdicionalHeDiurnoPct";
    public Task<RegraAplicacao> AplicarAsync(AdicionalHeDiurnoPctParam p, ContextoFuncionarioFolha ctx)
    {
        // Sobrescreve o pct default de 50% para o pct da CCT
        ctx.PctHeDiurno = p.Pct;
        ctx.HeComDsr = p.ComDsr;
        return Task.FromResult(new RegraAplicacao("AdicionalHeDiurnoPct", $"HE diurna alterada para {p.Pct}%"));
    }
}

public class HandlerAnuenie : IRegraConvencaoHandler<AnuenieParam>
{
    public Task<RegraAplicacao> AplicarAsync(AnuenieParam p, ContextoFuncionarioFolha ctx)
    {
        var anos = (int)((ctx.Competencia.ToDate() - ctx.Funcionario.DataAdmissao).TotalDays / 365.25);
        var anosLimite = Math.Min(anos, p.TetoAnos);
        var valor = ctx.SalarioBaseVigente * (p.PctPorAno / 100m) * anosLimite;
        ctx.Add(rubrica: "ANU-CCT", valor: valor, tipo: Provento);
        return ...;
    }
}
```

## Simulação de impacto

```
POST /api/v1/rh/convencoes/{id}/simular-impacto { funcionarioIds: [...], competencia: "2026-06" }

Servidor:
  Para cada funcionario:
    1. Roda engine SEM aplicar CCT → holerite-base
    2. Roda engine COM CCT → holerite-com-cct
    3. Calcula diff por rubrica
  Retorna { funcionarios: [{ funcId, baseLiquido, ccLiquido, delta, rubricasNovas, rubricasAlteradas }] }
```

Otimização: cache base por competência se já calculada.

## Detecção de mudança em CCT que afeta folha já calculada

```
Trigger: ao salvar Convencao/RegraConvencao/AdesaoConvencao:
  Se vigência sobrepõe folhas Calculadas/Conferidas:
    Marca folhas afetadas com flag `precisa_recalcular=true`
    Notifica RH
```

## Tradeoffs

### Por que polimorfismo via JSON e não tabela por tipo?

Tipos de regra variam muito; criar tabela por tipo = ~15 tabelas. JSON+enum + handler é mais flexível e barato.

### Por que `RegraCustomDsl` como escape hatch?

Não conseguimos antecipar TODA criatividade sindical. Escape hatch reusa W5 evaluator.

### Múltiplas convenções vigentes — última prevalece?

Padrão "última prevalece por chave de regra" (ex: `AdicionalHeDiurnoPct` da última CCT manda). Alternativa: lançar erro de conflito. **Decisão**: prevalece + warning, com opção de marcar regra como "exclusiva" (impede outras do mesmo tipo).

### Aplicação retroativa?

Reabertura de folha (W6) é obrigatória. Engine não recalcula folhas fechadas automaticamente.

## Test strategy

- Unit: cada handler de tipo de regra com 3-5 fixtures.
- Unit: resolvedor com 5 cenários (sem nada, com override, com adesão, com múltiplas).
- Integration: criar CCT METAL-SP-2026 com 3 regras → aplicar em 5 funcionários → folha bate com expected.
- Simulação: diff produzido bate com cálculo manual.

## Migração

Sem migração de dados (CCT é novo). Tenants existentes podem rodar sem nenhuma CCT (engine cai no fallback CLT base).
