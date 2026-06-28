## ADDED Requirements

### Requirement: NSR monotonicamente crescente e gap-free por empresa

O sistema SHALL prover serviço `NumeradorNsrService` que retorna NSR único e sequencial por (tenant, empresa), garantindo: nunca repete, nunca pula, ordem por hora de criação. Em concorrência alta, SHALL serializar via lock pessimista no banco.

#### Scenario: 1000 batidas concorrentes

- **WHEN** 1000 threads chamam `ProximoAsync(tenantA, empresaX)` simultaneamente
- **THEN** retornam 1000 valores distintos
- **AND** valores formam sequência contígua sem gap (mínimo+999 = máximo)

#### Scenario: Gap detectado por job auditoria

- **GIVEN** banco tem NSRs 1..100 e alguém apaga manualmente o NSR 50
- **WHEN** `JobAuditarGapsNsr` roda
- **THEN** dispara alerta para tenant admin + auditoria

### Requirement: Comprovante de marcação assinado ICP-Brasil

Quando empresa tem `usa_rep_oficial=true`, cada batida SHALL gerar `ComprovantePonto` com payload no formato da Portaria 671 anexo II, assinado com certificado ICP-Brasil A1/A3 do empregador (SHA-256 + RSA). Comprovante SHALL ser disponibilizado ao funcionário em PDF (1ª via instantânea + 2ª via sob demanda).

#### Scenario: Batida gera comprovante assinado

- **GIVEN** empresa X com `usa_rep_oficial=true` e certificado A1 válido
- **WHEN** funcionário bate ponto
- **THEN** sistema persiste `ComprovantePonto { nsr, payload, assinatura, hash }`
- **AND** retorna PDF na resposta da batida
- **AND** assinatura verifica com chave pública do certificado

#### Scenario: 2ª via do comprovante

- **WHEN** funcionário chama `GET /api/v1/rh/ponto/671/comprovantes/{marcacaoId}.pdf`
- **THEN** sistema regenera PDF com mesmos dados e assinatura
- **AND** PDF é byte-identico (determinístico)

### Requirement: Exportação AFD conforme anexo I

O sistema SHALL exportar AFD em formato texto fixo conforme Portaria MTP 671/2021 anexo I, contendo todos os 7 tipos de registro (cabeçalho, identificador, marcação, ajuste RTC, empregado, eventos, trailer com hash SHA-256). Arquivo SHALL passar no validador CLI oficial do MTE.

#### Scenario: AFD de 30 dias

- **WHEN** RH chama `POST /api/v1/rh/ponto/671/afd/exportar { empresaId, dataInicio: "2026-06-01", dataFim: "2026-06-30" }`
- **THEN** sistema enfileira geração; worker produz arquivo em S3
- **AND** retorna `{ exportacaoId, arquivoUrl, hash }`
- **AND** `GET /afd/{id}/download` retorna o arquivo
- **AND** validador CLI MTE retorna exit-code 0 para esse arquivo

### Requirement: Exportação AEJ conforme anexo IV

O sistema SHALL exportar AEJ em formato JSON assinado (JWS detached) conforme Portaria 671 anexo IV, contendo jornadas, banco de horas, marcações, ajustes e espelhos do período.

#### Scenario: AEJ contém todos os componentes

- **WHEN** RH exporta AEJ do mês
- **THEN** JSON tem seções `cabecalho, jornadas, bancosHoras, marcacoes, ajustes, espelhos`
- **AND** assinatura JWS verifica contra cert do empregador

### Requirement: Configuração REP por empresa

O sistema SHALL manter `ConfiguracaoRep` por empresa do tenant, contendo: tipo (REP-P ou REP-C), CNPJ/CEI/CNO, endereço, certificado vinculado, dados do responsável legal. Sem configuração completa, empresa não pode ativar `usa_rep_oficial`.

#### Scenario: Tentativa de ativar sem cert válido

- **WHEN** admin ativa `usa_rep_oficial=true` em empresa sem certificado
- **THEN** API retorna 400 com mensagem `Configuração REP incompleta: certificado obrigatório`

### Requirement: Compatibilidade com ponto interno do W2

Empresas com `usa_rep_oficial=false` SHALL continuar operando ponto interno do W2 sem regressão. NSR é campo opcional em `MarcacaoPonto`.

#### Scenario: Empresa sem REP oficial não gera NSR

- **GIVEN** empresa Y com `usa_rep_oficial=false`
- **WHEN** funcionário bate ponto
- **THEN** sistema cria MarcacaoPonto com `nsr=NULL`
- **AND** não chama `NumeradorNsrService`
- **AND** não cria `ComprovantePonto`
