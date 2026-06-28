# Design — rh-rescisao

## Tabela de direitos por tipo

Implementada como dicionário estático + interface `IRegrasRescisao`:

```csharp
public sealed class RegrasRescisao
{
    public static readonly Dictionary<TipoRescisao, DireitosRescisao> Direitos = new()
    {
        [TipoRescisao.SemJustaCausaEmpresa] = new(
            avisoPrevio: true, multaFgts40: true, multaSocial10: false,
            seguroDesemprego: true, indenizacaoAdicional: false),
        [TipoRescisao.PedidoDemissao] = new(
            avisoPrevio: true /*trabalhado*/, multaFgts40: false, seguroDesemprego: false, ...),
        [TipoRescisao.JustaCausaEmpresa] = new(
            avisoPrevio: false, multaFgts40: false, seguroDesemprego: false,
            ferias: SoVencidas, decimo3o: false, ...),
        [TipoRescisao.RescisaoIndireta] = new(... equivalente sem justa causa),
        [TipoRescisao.AcordoConsensual] = new(
            avisoPrevio: AvisoIndenizadoMetade,
            multaFgts40: true /*mas 20% — metade*/,
            seguroDesemprego: false, podeSacarFgts: 80%),
        ...
    };
}
```

## Engine

```csharp
public sealed class EngineFolhaRescisao
{
    public async Task<HoleriteFuncionario> CalcularAsync(Guid rescisaoId)
    {
        var rescisao = await _repo.ObterAsync(rescisaoId);
        var ctx = await _prep.PrepararContextoRescisaoAsync(rescisao);
        var d = RegrasRescisao.Direitos[rescisao.Tipo];

        // Saldo salário
        ctx.Add("R01-SAL-SALDO", ctx.SaldoSalarioDoMes());

        // Aviso prévio (se direito + indenizado)
        if (d.AvisoPrevio && rescisao.TipoAvisoPrevio == TipoAvisoPrevio.Indenizado)
            ctx.Add("R02-AVISO-IND", ctx.AvisoPrevioIndenizado());

        // Férias vencidas + 1/3
        ctx.Add("R03-FER-VENC",    ctx.FeriasVencidas() * 1.333m);

        // Férias proporcionais (exceto justa causa para empresa)
        if (rescisao.Tipo != TipoRescisao.JustaCausaEmpresa)
            ctx.Add("R04-FER-PROP", ctx.FeriasProporcionais() * 1.333m);

        // 13º proporcional (exceto justa causa para empresa)
        if (rescisao.Tipo != TipoRescisao.JustaCausaEmpresa)
            ctx.Add("R05-13-PROP",  ctx.Decimo3oProporcional());

        // Multa 40% FGTS (ou 20% acordo consensual)
        if (d.MultaFgts40)
        {
            var pct = rescisao.Tipo == TipoRescisao.AcordoConsensual ? 0.20m : 0.40m;
            ctx.Add("R06-MULTA-FGTS", rescisao.SaldoFgtsConhecido * pct);
        }

        // Banco horas (quitação)
        var bh = await _bancoHoras.SaldoAtualAsync(funcId);
        if (bh.SaldoMinutos > 0)
            ctx.Add("R07-BANCO-HORAS-PAGA", bh.SaldoMinutos / 60m * ctx.SalarioHora);
        else if (bh.SaldoMinutos < 0)
            ctx.Add("R08-BANCO-HORAS-DESC", bh.SaldoMinutos / 60m * ctx.SalarioHora);  // negativo

        // Adiantamentos pendentes
        ctx.Add("R09-ADTO-DESC", -await _adiantamentos.SaldoPendenteAsync(funcId));

        // Descontos legais
        ctx.Add("R100-INSS-RESC", -ctx.InssSobreRescisao());
        ctx.Add("R110-IRRF-RESC", -ctx.IrrfSobreRescisao());
        // FGTS sobre rubricas incidentes (informativo)
        ctx.Add("R900-FGTS-RESC", ctx.FgtsRescisaoInfo());

        return ConsolidaEPersiste(ctx);
    }
}
```

## TRCT PDF

Layout oficial NR-127:
- 6 seções fixas + tabelas.
- Hash do PDF gravado para auditoria.
- Marca d'água "RASCUNHO" enquanto status != Homologada.

## Workflow / Estados

```
Programada
   │ /calcular
   ▼
Calculada (rascunho)
   │ /homologar { data, local, homologador, anexos? }
   ▼
Homologada
   │ /concluir (após pagamento)
   ▼
Concluida → dispara:
   ├── Funcionario.status = Desligado
   ├── Usuario.status = Desativado
   ├── Pendência ContaPagar do líquido (W10)
   └── Pendência S-2299 (W13)
```

## Tradeoffs

### Saldo FGTS — vem de onde?

Não vem do Atena. Campo manual no momento da rescisão. RH consulta extrato da Caixa e digita. Em W13/eSocial, FGTS_TRCT é enviado separado.

### Acordo consensual — 50% das verbas

Lei 13.467/2017 instituiu acordo consensual com 50% do aviso e 20% multa FGTS (em vez de 40%). Sistema implementa diferenciado por tipo.

### Reabertura?

Rescisão é altamente sensível juridicamente. Em status Homologada, qualquer mudança vira NOVA rescisão (cancela + recria). Audit pesado.

## Test strategy

- Unit: cada `RegrasRescisao.Direitos[tipo]` (8 cenários × 6 flags).
- Unit: engine por tipo (8 fixtures completas).
- Unit: cálculo aviso prévio (30d + 3d/ano, teto 90d).
- Integration: criar rescisão → calcular → gerar TRCT → homologar → concluir → funcionário desligado + Usuario desativado + pendência criada.
