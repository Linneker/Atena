## Why

W4. Empresas com mais de 20 funcionários no Brasil são obrigadas a manter **Sistema de Registro Eletrônico de Ponto** conforme **Portaria MTP 671/2021** (que revoga a 1.510/2009 e moderniza com REP-A/REP-P/REP-C). Sem isso, o ponto interno do W2 serve só pra controle interno — não tem **fé pública**.

Esta onda transforma o ponto interno em **ponto oficial fiscalizável**, gerando:
- **Comprovante de marcação** com NSR (Número Sequencial de Registro), assinatura ICP-Brasil do empregador, formato definido pela Portaria.
- **AFD (Arquivo Fonte de Dados)** layout fixo, exportável para fiscalização, com hash de integridade.
- **AEJ (Arquivo Eletrônico de Jornada)** com jornadas, acordos, banco de horas, espelho — exigido em fiscalização do MTE.
- **REP-P** (programa em PC/web — o que já estamos construindo).
- **REP-C** (cloud — multi-tenant, escala, redundância).

Note: **REP-A** (relógio físico hardware autônomo) **não** é escopo. Foco em REP-P/REP-C — software-based.

## What Changes

### Backend — entidades novas

- `Nsr` (numerador atômico por tenant + empregador)
  - tenant_id, empresa_id, ultimo_numero (BIGINT), atualizado_em
  - reuso conceitual do numerador atômico de NFe lote
- `ComprovantePonto`
  - id, marcacao_id (FK 1:1), nsr (BIGINT), payload_xml (texto), assinatura_xml (texto), hash_sha256
  - emitido_em, certificado_cnpj_thumbprint
- `ConfiguracaoRep`
  - tenant_id, empresa_id
  - tipo (`Rep-P`, `Rep-C`)
  - razao_social, cnpj_cei, cno, endereco
  - certificado_id (FK ao certificado já gerido pelo CertificadoTenantResolver)
  - inscricao_estadual, atividade_principal_cnae
  - responsavel_legal_cpf, responsavel_legal_nome
- `ExportacaoAfd`
  - tenant_id, empresa_id, periodo_inicio, periodo_fim
  - arquivo_url (S3), hash_sha256, gerado_em, gerado_por
- `ExportacaoAej`
  - similar, mas para AEJ

### NSR (Número Sequencial de Registro)

NSR é **monotonicamente crescente, gap-free, único por REP**. Comparável ao número de NFe lote. Reaproveitamos `NumeradorAtomico` do `nfe-cliente-sefaz-proprio`:

```csharp
// Em vez de NumeradorNFe, NumeradorNsr
public sealed class NumeradorNsrService : INumeradorNsr
{
    // Mesma mecânica: lock pessimista no DB, incremento atômico
    public async Task<long> ProximoAsync(Guid tenantId, Guid empresaId, CancellationToken ct);
}
```

### Comprovante — formato e assinatura

Portaria 671/2021 anexo II define formato textual ASCII fixo (não XML mais — diferente do que era na 1.510). Layout:

```
NSR|TIPO_REGISTRO|CPF|PIS|DATA_HORA|FUNCIONARIO_NOME|CNPJ_EMPREGADOR|HASH
```

Comprovante físico (entregue ao funcionário sob demanda — papel ou PDF/PNG):
- Linha 1: razão social do empregador + CNPJ
- Linha 2: local da prestação de serviço
- Linha 3: nome do empregado + CPF + PIS
- Linha 4: data + hora da marcação
- Linha 5: NSR
- Linha 6 em diante: assinatura digital (resumo) + hash

Assinatura: ICP-Brasil A1/A3 do empregador, algoritmo SHA-256 + RSA-PKCS#1 v1.5. Reuso de `XmlSignerC14N` adaptado (mesmas primitivas; payload é texto não XML).

### AFD (Arquivo Fonte de Dados)

Layout texto fixo conforme Portaria 671/2021 anexo I:
- Registro tipo 1: cabeçalho (CNPJ, razão, endereço, CEI, CNO, mês/ano referência)
- Registro tipo 2: identificador (nome empregador, INPI, versão do REP)
- Registro tipo 3: marcações (NSR, data, hora, PIS)
- Registro tipo 4: ajustes do RTC (relógio do REP)
- Registro tipo 5: empregado (PIS, CPF, nome)
- Registro tipo 6: eventos do REP (inicialização, manutenção)
- Registro tipo 7: trailer (totalizadores + hash do arquivo)

Endpoint:
```
POST /api/v1/rh/ponto/671/afd/exportar
Body: { empresaId, dataInicio, dataFim }
→ retorna { exportacaoId, arquivoUrl, hash }
Validação no app verificador do MTE.
```

### AEJ (Arquivo Eletrônico de Jornada)

Layout JSON + assinado, conforme anexo IV Portaria 671. Contém:
- Jornadas cadastradas
- Banco de horas e movimentos
- Justificativas de ajuste
- Acordos de compensação
- Espelho do período

```
POST /api/v1/rh/ponto/671/aej/exportar
Body: { empresaId, dataInicio, dataFim }
→ arquivo .json assinado + AFD reference
```

### Modificações em W2

- `MarcacaoPonto` ganha campo `nsr` (BIGINT) **somente** quando emitida via REP oficial (W4); marcações puramente internas (W2) podem não ter NSR.
- Flag por empresa: `usa_rep_oficial` (BOOLEAN). Se true, **toda** batida exige NSR (mesmo via web/mobile) e gera comprovante.
- `BaterPonto` quando `usa_rep_oficial=true`:
  1. Cria MarcacaoPonto.
  2. Reserva NSR atômico.
  3. Gera comprovante.
  4. Assina.
  5. Persiste `ComprovantePonto`.
  6. Retorna comprovante PDF + hash na resposta.

### Permissions novas

- `Recursos.RhPontoOficial`
- `Acoes.ExportarAfd`, `Acoes.ExportarAej`, `Acoes.ConfigurarRep`, `Acoes.EmitirComprovante2via`

### Endpoints novos

- `POST /api/v1/rh/ponto/671/configuracao` (CRUD da `ConfiguracaoRep`)
- `GET /api/v1/rh/ponto/671/comprovantes/{marcacaoId}.pdf` (2ª via para funcionário)
- `POST /api/v1/rh/ponto/671/afd/exportar`
- `GET /api/v1/rh/ponto/671/afd/{exportacaoId}/download`
- `POST /api/v1/rh/ponto/671/aej/exportar`
- `GET /api/v1/rh/ponto/671/aej/{exportacaoId}/download`
- `GET /api/v1/rh/ponto/671/validar` (auto-verificação do REP — diagnostica config, cert, NSR)

## Capabilities

### New Capabilities
- `rh-ponto-oficial-671` — Conformidade Portaria MTP 671/2021: NSR, comprovante assinado ICP-Brasil, AFD, AEJ.

### Modified Capabilities
- `rh-ponto-interno` — `MarcacaoPonto` ganha NSR + comprovante quando empresa usa REP oficial.

## Out of Scope
- REP-A (relógio físico autônomo hardware).
- Convênio com tribunal trabalhista para envio automático.
- Integração com sistemas de jornada de terceiros (cartão ponto físico, biometria server-side).
- Migração de bases de outros REPs (TopData, Henry, ZKTeco).

## Risks

- **R1**: Mudança na Portaria (novas versões — historicamente acontece). Mitigação: versionar layouts AFD/AEJ via classes versionadas, igual ao XSD do NFe.
- **R2**: Certificado A1 do empregador expira. Mitigação: reuso da gestão já feita no NFe + alerta 30 dias antes.
- **R3**: NSR perdido / gap. Mitigação: storage atômico no MySQL + auditoria de gap (job noturno).
- **R4**: Fiscalização real do MTE pode encontrar bug no layout. Mitigação: bateria de testes contra app verificador oficial do MTE (existe ferramenta CLI gratuita) + período de homologação com 1 tenant piloto.
- **R5**: Pegada do REP-P na desktop vs web — alguns auditores ainda exigem instalação local. Mitigação: declarar REP-C explicitamente; AFD/AEJ exportáveis cobrem fiscalização.

## Success Criteria

- AFD gerado para 30 dias de 100 funcionários valida no app verificador oficial do MTE (que existe como CLI gratuita).
- AEJ idem.
- Comprovante assinado verifica em ferramentas externas (Adobe, ITI).
- NSR monotonicamente crescente garantido em teste de carga (1000 batidas concorrentes → todas com NSR único, sem gap).
- Empresas com `usa_rep_oficial=false` continuam usando ponto interno do W2 sem regressão.
- `openspec validate rh-ponto-oficial-671 --strict` válido.
