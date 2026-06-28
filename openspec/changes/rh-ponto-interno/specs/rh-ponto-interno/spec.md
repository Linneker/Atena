## ADDED Requirements

### Requirement: Registro de marcação de ponto pelo funcionário

O sistema SHALL permitir que todo funcionário autenticado registre sua própria marcação de ponto via `POST /api/v1/rh/ponto/bater`, gerando uma entidade `MarcacaoPonto` com hash de integridade encadeado à marcação anterior do mesmo funcionário.

#### Scenario: Funcionário bate ponto pela manhã

- **GIVEN** funcionário logado sem batidas hoje
- **WHEN** chama `POST /api/v1/rh/ponto/bater { tipo: null }`
- **THEN** sistema infere `tipo = Entrada` (primeira batida do dia)
- **AND** grava `MarcacaoPonto { dataHora: now, tipo: Entrada, origem: Web, hashIntegridade: SHA-256(...) }`
- **AND** retorna 201 com a marcação criada

#### Scenario: Hash chain detecta adulteração

- **GIVEN** funcionário tem 10 marcações encadeadas com hash íntegro
- **WHEN** alguém altera diretamente o campo `dataHora` da marcação #5 no banco
- **AND** job `VerificarIntegridadePonto` roda
- **THEN** sistema detecta divergência e dispara alerta para o tenant admin
- **AND** grava `AuditLog` com `MarcacaoPontoIntegridadeViolada`

#### Scenario: Funcionário não pode bater ponto de outro

- **WHEN** funcionário A chama `POST /api/v1/rh/ponto/bater` impersonando funcionário B
- **THEN** sistema usa SEMPRE o `funcionarioId` derivado do JWT
- **AND** ignora qualquer `funcionarioId` no body

### Requirement: Ajuste de ponto com workflow de aprovação

O sistema SHALL permitir que funcionário solicite ajuste em sua marcação (inclusão, alteração, exclusão, ou apenas justificativa), criando entidade `AjustePonto` com status `Pendente`. Aprovador (com permissão `rh-ponto:aprovar`) decide; ao aprovar, nova `MarcacaoPonto.Ajustada` é gerada mantendo a cadeia de hash, e a original mantida com `status=Ajustada`.

#### Scenario: Funcionário solicita inclusão de batida esquecida

- **GIVEN** funcionário esqueceu de bater a saída de ontem (16:00 a 18:00 esperada)
- **WHEN** chama `POST /api/v1/rh/ponto/ajustes { tipoAjuste: "Inclusao", dataHoraProposta: "2026-06-26T18:00", motivo: "Esqueci de bater", anexoUrl: null }`
- **THEN** sistema cria `AjustePonto { status: Pendente, aprovadorId: <gestor> }`
- **AND** notifica gestor por e-mail e bell

#### Scenario: Gestor aprova ajuste

- **WHEN** gestor chama `POST /api/v1/rh/ponto/ajustes/{id}/aprovar { justificativa: "OK" }`
- **THEN** sistema cria nova `MarcacaoPonto` com a hora proposta e `origem=Manual`
- **AND** atualiza `AjustePonto.status = Aprovado`
- **AND** preserva auditoria completa (quem aprovou, quando, por quê)
- **AND** notifica funcionário

### Requirement: Espelho mensal de ponto

O sistema SHALL gerar espelho mensal estruturado para cada funcionário, contendo: lista de dias do mês, batidas de cada dia, jornada esperada, horas trabalhadas, atrasos, faltas, saldo do dia, saldo acumulado do mês, e totais. SHALL estar disponível em JSON e em PDF assinado por servidor com hash do espelho.

#### Scenario: Espelho do mês corrente

- **WHEN** RH chama `GET /api/v1/rh/ponto/espelho?funcionarioId=...&competencia=2026-06`
- **THEN** retorna JSON com 30 entradas (uma por dia), totais agregados, e `hashEspelho`

#### Scenario: PDF do espelho

- **WHEN** funcionário chama `GET /api/v1/rh/ponto/espelho.pdf?competencia=2026-06`
- **THEN** retorna PDF (Content-Type: application/pdf) renderizado via QuestPDF
- **AND** PDF contém marca d'água "GERENCIAL — NÃO SUBSTITUI PONTO OFICIAL PORTARIA 671" (enquanto W4 não estiver disponível)
- **AND** PDF contém QR code com `hashEspelho`

### Requirement: Banco de horas configurável

O sistema SHALL modelar políticas de banco de horas por tenant (`BancoHorasPolitica`) com limites de acúmulo, prazo de compensação, e fator de pagamento. Cada movimento (acúmulo, compensação, pagamento, ajuste, expiração) SHALL ser registrado em `MovimentoBancoHoras` com referência à marcação que originou (quando aplicável).

#### Scenario: Funcionário acumula horas extras na política "Padrão"

- **GIVEN** política `Padrão: limiteAcumular=40, prazoCompensacao=180, permitePagar=true`
- **AND** funcionário trabalhou 1h além da jornada hoje
- **WHEN** engine de cálculo processa as marcações do dia
- **THEN** cria `MovimentoBancoHoras { minutos: +60, origem: Acumulo }`
- **AND** atualiza `BancoHorasSaldo` da competência

#### Scenario: RH paga saldo positivo do banco

- **WHEN** RH chama `POST /api/v1/rh/banco-horas/{funcionarioId}/pagar { competencia: "2026-06", minutos: 600 }`
- **THEN** sistema cria `MovimentoBancoHoras { minutos: -600, origem: Pagamento }`
- **AND** atualiza saldo
- **AND** registra pendência para folha (W6) lançar como rubrica de HE paga

### Requirement: Fechamento de competência

O sistema SHALL prover endpoint `POST /api/v1/rh/ponto/competencia/{ymd}/fechar` que trava edição de marcações daquela competência, gera espelhos PDF em massa (async via RabbitMQ), e disponibiliza dados para consumo da folha (W6).

#### Scenario: Fechar competência com 100 funcionários

- **WHEN** RH fecha competência 2026-06
- **THEN** sistema marca `FechamentoPonto.status = Fechado` para todos 100
- **AND** publica 100 mensagens `GerarEspelhoPdfMessage` em RabbitMQ
- **AND** worker gera cada PDF e armazena em S3 com chave `tenant/funcId/espelho/202606.pdf`
- **AND** cada funcionário recebe e-mail quando seu PDF está pronto
- **AND** edição posterior de marcação dessa competência retorna 409 Conflict

#### Scenario: Reabrir competência fechada (admin)

- **GIVEN** competência 2026-06 está fechada
- **WHEN** admin tenant chama `POST /api/v1/rh/ponto/competencia/2026-06/reabrir { motivo: "..." }`
- **THEN** sistema reabre, audita com motivo, e libera edições
- **AND** folha já calculada em W6 fica marcada como "potencialmente desatualizada"

### Requirement: Endpoints RH-Ponto seguem blueprint Acme

Todas as rotas `/api/v1/rh/ponto/*` e `/api/v1/rh/banco-horas/*` SHALL seguir o padrão Endpoint+Request+Response+Map (uma rota por pasta com 4 arquivos), validado em runtime por `EndpointConventionTests`.

#### Scenario: Endpoint plural é reprovado em CI

- **WHEN** PR introduz arquivo `RhPontoEndpoints.cs` (plural) em `Endpoints/V1/Rh/Ponto/`
- **THEN** `EndpointConventionTests` falha localmente e em CI
- **AND** PR é bloqueado até o autor refatorar para o padrão `BaterPonto/BaterPontoEndpoint.cs + Request.cs + Response.cs + Map.cs`
