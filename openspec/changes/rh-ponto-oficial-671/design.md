# Design — rh-ponto-oficial-671

## Componentes reusados do NFe

```
nfe-cliente-sefaz-proprio                       rh-ponto-oficial-671
─────────────────────────────                   ─────────────────────
XmlSignerC14N (XMLDSig SHA-256)        ─reuso─► AssinadorComprovante671
CertificadoTenantResolver              ─reuso─► (mesmo cert do tenant)
NumeradorAtomicoNFe (lote)             ─reuso─► NumeradorNsr
ContingenciaPolicy                     ─reuso─► não aplica (não há transmissão)
```

## NSR atômico

```sql
CREATE TABLE numerador_nsr (
  tenant_id CHAR(36),
  empresa_id CHAR(36),
  ultimo_numero BIGINT NOT NULL DEFAULT 0,
  atualizado_em DATETIME(6),
  PRIMARY KEY (tenant_id, empresa_id)
);
```

```csharp
public sealed class NumeradorNsrService : INumeradorNsr
{
    private readonly IDataConfiguration _db;

    public async Task<long> ProximoAsync(Guid tenantId, Guid empresaId, CancellationToken ct)
    {
        // Padrão idêntico ao NumeradorNFe:
        //   transação atômica
        //   SELECT ... FOR UPDATE
        //   UPDATE numerador_nsr SET ultimo_numero = ultimo_numero + 1
        //   COMMIT
        //   retorna o novo valor
    }
}
```

## Fluxo de batida com REP oficial

```
POST /api/v1/rh/ponto/bater (ou bater-mobile)
            │
            ▼
   BaterPontoCommandHandler
            │
            ├── cria MarcacaoPonto (W2)
            │
            ▼
   Empresa.usa_rep_oficial?
            │
   ┌────────┴────────┐
   ▼ não             ▼ sim
   retorna           ▼
                Gerar671CommandHandler
                ├── NumeradorNsrService.ProximoAsync → nsr=N
                ├── Monta payload texto (Portaria 671 anexo II)
                ├── Assina via AssinadorComprovante671 (ICP-Brasil A1)
                ├── Persiste ComprovantePonto
                ├── Retorna comprovante (PDF gerado on-demand)
                └── Atualiza MarcacaoPonto.nsr = N
```

## Layout AFD (versão 003 da Portaria 671)

```
Registro Cabeçalho (tipo 1):
  9 dígitos NSR | 1 tipo=1 | 14 CNPJ | 12 CEI | 30 razão | 100 endereço | 8 data | 4 hora | 8 data_geração | 4 hora_geração | preencheZeros

Registro Identificador (tipo 2):
  ...

Registro Marcação (tipo 3):
  9 dígitos NSR | 1 tipo=3 | 8 data | 4 hora | 12 PIS | DV

...

Registro Trailer (tipo 9 ou 7 dep. versão):
  9 dígitos NSR | 1 tipo=9 | totalizadores | 256 hash SHA-256 do arquivo todo
```

Implementação: `LayoutAfd003Writer` em `Acme.Sistemas.ExternalIntegration/Rh/671/AfdV003/`. Versão futura `LayoutAfd004` quando MTE atualizar.

## Layout AEJ

JSON conforme anexo IV, com seções:
```json
{
  "cabecalho": { ... empregador, empresa, períodos ... },
  "jornadas": [ ... lista de jornadas vigentes no período ... ],
  "bancosHoras": [ ... políticas + movimentos por funcionário ... ],
  "marcacoes": [ ... lista de NSR + data + hora + funcionário ... ],
  "ajustes": [ ... ajustes aprovados com justificativa ... ],
  "espelhos": [ ... por funcionário ... ]
}
```

Assinatura: detached signature em JSON via JWS (RFC 7515) com cert ICP-Brasil.

## Comprovante PDF

```
┌──────────────────────────────────────────────┐
│ RAZÃO SOCIAL DA EMPRESA                      │
│ CNPJ: XX.XXX.XXX/XXXX-XX                     │
│ Endereço: ...                                │
│                                              │
│ COMPROVANTE DE REGISTRO DE PONTO             │
│                                              │
│ Empregado: NOME COMPLETO                     │
│ CPF: XXX.XXX.XXX-XX | PIS: XXX.XXXXX.XX-X    │
│                                              │
│ Data:  DD/MM/AAAA                            │
│ Hora:  HH:MM:SS                              │
│ Tipo:  ENTRADA                               │
│                                              │
│ NSR:   00012345                              │
│                                              │
│ Assinatura digital ICP-Brasil:               │
│ [128 chars de resumo da assinatura]          │
│                                              │
│ Hash: SHA-256: [64 chars]                    │
│                                              │
│ [QR code com URL/hash para verificação]      │
└──────────────────────────────────────────────┘
```

## Decisão: REP-P vs REP-C

| Aspecto | REP-P | REP-C |
|---------|:-----:|:-----:|
| Instalação | Desktop / web local | Cloud SaaS |
| Auditoria | Auditor in loco | Auditor remoto via VPN/dump |
| Backup | Responsabilidade da empresa | Provider |
| Multi-tenant | 1 empresa = 1 REP | Múltiplos |
| Atena se encaixa | sim (versão self-hosted) | sim (versão cloud) |

**Decisão**: `ConfiguracaoRep.tipo` permite ambos; comportamento idêntico, diferença é documental e contratual.

## Verificação contra app oficial do MTE

MTE publica "Validador" CLI gratuito que testa AFD/AEJ contra layout oficial. Vamos:
1. Integrar como teste de aceitação: gerar AFD, rodar validador, esperar exit-code 0.
2. Documentar diferenças (se houver) e iterar.

## Tests

- Unit: `LayoutAfd003Writer` para cada tipo de registro (1, 2, 3, 4, 5, 6, 7).
- Unit: `NumeradorNsrService` — 1000 chamadas concorrentes → resultado único e sequencial.
- Integration: gerar AFD de 30 dias → rodar validador MTE → esperar sucesso.
- Integration: assinar comprovante → verificar com OpenSSL externamente.
- Integration: bater ponto em empresa com `usa_rep_oficial=true` → resposta contém comprovante + NSR.
