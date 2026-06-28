# Design — esocial-tabelas

## Estrutura

```
Acme.Sistemas.ExternalIntegration/Esocial/Eventos/V1_2/Tabelas/
├── S1000_Empregador/
│   ├── S1000.cs                    (POCO)
│   ├── S1000Builder.cs             (Atena → POCO)
│   └── S1000Validator.cs
├── S1005_Estabelecimentos/
├── S1010_Rubricas/
├── S1020_Lotacoes/
├── S1070_Processos/
└── S1280_InformacoesComplementares/
```

## Builder S-1000

```csharp
public sealed class S1000Builder
{
    public S1000 Construir(EmpregadorEsocial emp, IndicadorRetificacao tipo)
    {
        return new S1000
        {
            IdeEvento = new IdeEvento
            {
                IndRetif = tipo,
                TpAmb = (int)emp.Ambiente,
                ProcEmi = 1,
                VerProc = "AtenaERP/1.0"
            },
            IdeEmpregador = new IdeEmpregador
            {
                TpInsc = 1,
                NrInsc = emp.CnpjEmpregador
            },
            InfoEmpregador = new InfoEmpregador
            {
                IdePeriodo = new IdePeriodo { IniValid = "2026-06" },
                InfoCadastro = new InfoCadastro
                {
                    NmRazao = emp.RazaoSocial,
                    ClassTrib = emp.ClassificacaoTributaria,
                    NatJurid = emp.NaturezaJuridica,
                    IndCoop = emp.IndicativoCooperativa,
                    IndSitPj = SituacaoEmpregador.Normal,
                    InfoComplem = new InfoComplem { /* ... */ }
                }
            }
        };
    }
}
```

## Builder S-1005 (Estabelecimentos)

Mapeia `Lotacao` (W1) → estabelecimento eSocial. Cada lotação com CNPJ próprio = 1 estabelecimento separado.

## Builder S-1010 (Rubricas)

Mapeia `RubricaTenant` (W5) → tabela de rubricas eSocial. Campo `natureza_esocial_codigo` da RubricaTenant é o `codRubr` aqui.

## Hook automático

```csharp
// Em RubricaTenantRepository.SaveAsync ou via event sourcing:
public override async Task<int> SaveAsync(RubricaTenant rubrica)
{
    var r = await base.SaveAsync(rubrica);
    if (rubrica.AlterouCamposEsocial())
    {
        var indRetif = rubrica.PrimeiraTransmissao ? IndicadorRetificacao.Original : IndicadorRetificacao.Retificacao;
        await _mediator.Send(new GerarEventoS1010Command(rubrica.Id, indRetif));
    }
    return r;
}
```

## Ordem de transmissão (orquestrador)

```csharp
public sealed class OrquestradorTabelasEsocial
{
    public async Task GarantirOrdemAsync(Guid empregadorId)
    {
        // Verifica que S-1000 está Aceito antes de transmitir qualquer S-1005/S-1010
        var s1000 = await _eventoRepo.UltimoAsync(empregadorId, "S-1000");
        if (s1000?.Status != StatusEventoEsocial.Aceito)
            throw new InvalidOperationException("S-1000 precisa estar Aceito antes de outras tabelas");

        // ... orquestra outros
    }
}
```

## Test strategy

- Unit: builder por evento com fixture
- Unit: validator XSD por evento
- Integration: enviar S-1000 → Restrita → Aceito → enviar S-1005 → Aceito
- Integration: edição de Lotacao gera S-1005 com indRetif=2
