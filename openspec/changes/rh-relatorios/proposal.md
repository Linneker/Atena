## Why

W15. Última onda do programa. Com folha + CCT + eventos + eSocial funcionando, falta consolidar a **camada analítica e legal**: relatórios para gestor, contador, RH, auditor, e funcionário. Sem relatórios, o sistema é uma caixa-preta operacional — RH precisa explicar números, contador precisa fechar mês, gestor precisa enxergar custo.

## What Changes

### Categorias de relatório

**Operacional (RH usa diariamente)**
- Espelho mensal de ponto (W2 já entrega — agregação aqui)
- Holerite individual (W6 já entrega)
- Folha analítica (todas rubricas × todos funcionários × competência)
- Folha sintética (total de proventos/descontos/líquido × CC × competência)
- Banco de horas (saldo + movimentos × competência)
- Lista de admissões/demissões do período
- Recibo de férias / 13º / rescisão (W8/W9 já entregam — agregação aqui)

**Legal / Fiscal (anuais)**
- Comprovante anual de rendimentos (DIRF) — entrega aos funcionários
- Resumo anual de horas trabalhadas
- Demonstrativo INSS GPS por competência
- Demonstrativo IRRF DARF por competência
- Demonstrativo FGTS GRF por competência
- Memorando de conferência (folha × eSocial S-1200/S-1210/S-1299)

**Gerencial (gestor + diretoria)**
- Headcount por departamento/CC/lotação (mensal)
- Turnover (admissões/demissões / headcount médio)
- Custo total RH por competência (folha + encargos + benefícios)
- Custo por funcionário (líquido + encargos)
- Aniversariantes do mês
- Funcionários em férias / afastados (calendário)
- Saldo de férias agregado (passivo da empresa)
- Horas extras agregadas por departamento
- Distribuição salarial por cargo

### Tecnologia

- **Geração**: QuestPDF (já adotado), CsvHelper para CSV, ClosedXML para XLSX.
- **Cache**: Redis para queries agregadas pesadas (TTL 1h, invalidado por eventos de fechamento).
- **Async**: relatórios grandes via RabbitMQ + worker.
- **Templates**: cada relatório em pasta dedicada com pdf/csv/xlsx em paralelo.

### Permissions

- `Recursos.RhRelatorios` × `Acoes.Operacional, Legal, Gerencial`.
- Hierarquia: RH (todos), Gestor (operacional + gerencial), Funcionário (próprio holerite + comprovante anual).

### Frontend

Nova área `features/rh/relatorios/`:
- Catálogo de relatórios (grid de cards)
- Form de parâmetros por relatório
- Preview HTML
- Download PDF/CSV/XLSX
- Agendamento (envio recorrente por e-mail)

### Comprovante anual de rendimentos (DIRF substituiu, mas comprovante segue)

Layout oficial Receita Federal (CGRT 1.215):
- Identificação do beneficiário e fonte pagadora
- Rendimentos tributáveis × meses
- IRRF retido × meses
- INSS recolhido × meses
- Outras retenções
- Pensão alimentícia (se houver)

Geração: 1 PDF por funcionário × ano → S3 → notificação por e-mail no início de fevereiro do ano seguinte.

## Capabilities

### New Capabilities
- `rh-relatorios` — Catálogo amplo de relatórios operacionais, legais e gerenciais.

### Modified Capabilities
- `seed-tenant-administrativo` — semeia permissão `rh-relatorios:operacional` para role `RH`.

## Out of Scope
- BI customizável (Tableau-like).
- Relatórios LGPD (separados).
- DIRF/eSocial REINF — eSocial substituiu DIRF a partir de 2024, mas comprovante anual permanece.
- Dashboard executivo com gráficos interativos (separado).

## Risks

- **R1**: Relatórios pesados (1000 funcionários × 12 meses) podem estourar timeout HTTP. Mitigação: async via RabbitMQ.
- **R2**: Conferência folha × eSocial pode divergir (não-aceito vs aceito). Mitigação: relatório explicita estado de cada evento.
- **R3**: Comprovante anual tem layout oficial que pode mudar anualmente. Mitigação: versionar.

## Success Criteria

- 20 relatórios entregues (lista em design).
- Folha sintética de 1000 funcionários em < 30s.
- Comprovante anual de 1000 funcionários em < 5min (worker).
- Preview HTML responsivo no front.
- Agendamento recorrente funciona.
- `openspec validate rh-relatorios --strict` válido.
