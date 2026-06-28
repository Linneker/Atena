# Design — esocial-nao-periodicos

## Estrutura

```
Esocial/Eventos/V1_2/NaoPeriodicos/
├── S2200_Admissao/
├── S2205_AltCadastrais/
├── S2206_AltContratuais/
├── S2230_Afastamento/
├── S2250_AvisoPrevio/
├── S2298_Reintegracao/
├── S2299_Desligamento/
├── S2300_TSVE_Inicio/
├── S2306_TSVE_Alteracao/
└── S2399_TSVE_Termino/
```

## S-2200 — campos mínimos

```
ideEvento (indRetif, tpAmb, procEmi, verProc)
ideEmpregador (CNPJ)
trabalhador
  ├── cpfTrab, nmTrab, sexo, racaCor, estCiv, grauInstr, nascimento
  ├── nacionalidade
  ├── documentos (CTPS, RG, CNH, etc.)
  ├── endereco
  ├── dependentes
vinculo
  ├── matricula, tpRegTrab, tpRegPrev, codCBO, dtAdm, tpAdmissao
  ├── infoCeletista (tpContr, dtTerm, clauAssec, tpJornada, ...)
  └── duracao, localTrabalho, valorSalario
```

## Hooks

```csharp
// FuncionarioRepository ou via domain event:
public override async Task<int> SaveAsync(Funcionario f)
{
    var antes = await _repo.ObterAsync(f.Id);
    var r = await base.SaveAsync(f);

    if (f.Status == StatusAtivo.Ativo && antes == null)
    {
        await _mediator.Send(new GerarEventoS2200Command(f.Id));
    }
    else if (antes != null)
    {
        if (MudouCamposCadastrais(antes, f))
            await _mediator.Send(new GerarEventoS2205Command(f.Id));
        if (MudouCamposContratuais(antes, f))
            await _mediator.Send(new GerarEventoS2206Command(f.Id));
    }
    return r;
}
```

## Orquestração

```csharp
public sealed class OrquestradorNaoPeriodicos
{
    public async Task PrepararAsync(GerarEventoSxxxxCommand cmd)
    {
        // S-2205, S-2206, S-2230, S-2299: exige S-2200 Aceito
        if (cmd.Tipo != "S-2200")
        {
            var s2200 = await _eventoRepo.UltimoAceitoAsync(empregadorId, funcId, "S-2200");
            if (s2200 == null)
                throw new InvalidOperationException("S-2200 precisa estar Aceito antes");
        }
        // ...
    }
}
```

## Test strategy

- Unit: builder por evento + fixture
- Integration: ciclo S-2200 → S-2205 → S-2299 em Restrita
- Integration: hook automático funciona
- Integration: ordem é respeitada (S-2205 antes de S-2200 falha)
