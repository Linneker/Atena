## ADDED Requirements

### Requirement: Catálogo de 20 relatórios RH

O sistema SHALL prover ao menos 20 relatórios cobrindo as categorias Operacional, Legal e Gerencial, com formatos PDF, CSV e XLSX conforme aplicável, e geração assíncrona via RabbitMQ para os relatórios pesados.

#### Scenario: Folha analítica em XLSX

- **WHEN** RH chama `POST /rh/relatorios/folha-analitica { competencia: "2026-06", formato: "xlsx" }`
- **THEN** sistema enfileira geração
- **AND** worker produz arquivo .xlsx com 1 linha por funcionário × 1 coluna por rubrica
- **AND** notifica usuário com link para download

### Requirement: Comprovante anual de rendimentos por funcionário

O sistema SHALL gerar PDF anual de comprovante de rendimentos para cada funcionário (ativo ou desligado no ano fiscal), conforme layout oficial CGRT 1.215, com totais mensais de rendimentos tributáveis, IRRF retido, INSS, pensão alimentícia, e outras retenções. SHALL ser gerado automaticamente na 1ª semana de fevereiro do ano seguinte.

#### Scenario: Geração automática anual

- **GIVEN** ano fiscal 2026 encerrado
- **WHEN** chega a 1ª semana de fevereiro de 2027
- **THEN** job dispara geração para todos os funcionários
- **AND** cada um recebe e-mail com link PDF
- **AND** funcionário pode acessar via app mobile (W3) e web

### Requirement: Relatório de conferência folha × eSocial

O sistema SHALL prover relatório que reconcilia a folha mensal com os eventos eSocial S-1200/S-1210/S-1299 da competência, destacando: holerites sem S-1200, S-1200 Rejeitados (com motivo), S-1210 pendentes, e status do S-1299.

#### Scenario: Divergência detectada

- **GIVEN** folha 2026-06 com 100 holerites; S-1200 com 98 Aceitos + 2 Rejeitados
- **WHEN** RH gera o relatório
- **THEN** retorna lista de 2 funcionários divergentes com motivo eSocial + sugestão de ação

### Requirement: Agendamento recorrente

O sistema SHALL permitir usuário agendar relatório para envio recorrente (mensal/diário) por e-mail, persistido em `agendamentos_relatorios`. Job hosted service hourly dispara as execuções devidas.

#### Scenario: Agendar folha sintética mensal

- **WHEN** gestor agenda `{ tipoRelatorio: "folha-sintetica", periodicidade: "Mensal", parametros: {}, ativo: true }`
- **THEN** todo dia 6 do mês, job gera relatório da competência anterior e envia por e-mail ao gestor
- **AND** registra execução em `execucoes_relatorios`

### Requirement: Performance — folha de 1000 funcionários

Folha analítica de 1000 funcionários × 12 meses SHALL ser gerada em ≤ 30s com cache hit e ≤ 120s com cache miss.

#### Scenario: Cache hit

- **GIVEN** Query `FolhaAnaliticaQuery` para 2026-06 já cacheada no Redis
- **WHEN** RH solicita o relatório novamente
- **THEN** retorna em < 30s
