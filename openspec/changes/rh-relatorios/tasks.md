# Tasks — rh-relatorios

## Fase 1 — Infraestrutura
- [ ] 1.1 NuGet: ClosedXML para XLSX
- [ ] 1.2 NuGet: CsvHelper já tem? validar
- [ ] 1.3 Migration `AddTabelaAgendamentosRelatorios`
- [ ] 1.4 Migration `AddTabelaExecucoesRelatorios` (histórico)
- [ ] 1.5 Permissão `rh-relatorios:operacional/legal/gerencial`
- [ ] 1.6 Worker `GeradorRelatorioWorker`
- [ ] 1.7 Hosted service `AgendamentoRelatoriosHostedService` (hourly)

## Fase 2 — Relatórios operacionais (1-9)
- [ ] 2.1 Espelho de ponto mensal — Query + 1 renderer (PDF, já existe no W2 — agregar)
- [ ] 2.2 Holerite individual (W6 — agregar aqui)
- [ ] 2.3 Folha analítica (Query + 3 renderers)
- [ ] 2.4 Folha sintética (Query + 3 renderers)
- [ ] 2.5 Banco de horas (Query + 2 renderers)
- [ ] 2.6 Admissões/demissões (Query + 2 renderers)
- [ ] 2.7 Recibo de férias (W8 — agregar)
- [ ] 2.8 Recibo de 13º (W8 — agregar)
- [ ] 2.9 TRCT (W9 — agregar)

## Fase 3 — Relatórios legais (10-15)
- [ ] 3.1 Comprovante anual rendimentos (Query + PDF + worker async)
- [ ] 3.2 Resumo anual horas
- [ ] 3.3 GPS detalhada
- [ ] 3.4 DARF IRRF detalhada
- [ ] 3.5 GRF FGTS detalhada
- [ ] 3.6 Conferência folha × eSocial

## Fase 4 — Relatórios gerenciais (16-20)
- [ ] 4.1 Headcount por dept/CC/lotação
- [ ] 4.2 Turnover do período
- [ ] 4.3 Custo total RH
- [ ] 4.4 Aniversariantes do mês
- [ ] 4.5 Calendário férias/afastamentos (PDF + ICS)

## Fase 5 — Agendamento e job anual
- [ ] 5.1 CRUD de agendamentos
- [ ] 5.2 Worker dispara recorrentes
- [ ] 5.3 Job especial: comprovante anual de rendimentos (1ª semana fevereiro)

## Fase 6 — Frontend
- [ ] 6.1 Tela "Relatórios RH" — catálogo com cards por categoria
- [ ] 6.2 Form de parâmetros dinâmico por relatório
- [ ] 6.3 Preview HTML
- [ ] 6.4 Download PDF/CSV/XLSX
- [ ] 6.5 Tela "Agendamentos"
- [ ] 6.6 Tela "Histórico de relatórios gerados"

## Fase 7 — Cache e performance
- [ ] 7.1 Behavior cache nas queries pesadas
- [ ] 7.2 Invalidação automática em fechamento de folha
- [ ] 7.3 Teste de carga (folha analítica 1000 funcs)

## Fase 8 — Documentação e validação
- [ ] 8.1 `documentacao/rh/relatorios.md` (catálogo + parâmetros)
- [ ] 8.2 `documentacao/rh/comprovante-anual.md`
- [ ] 8.3 `openspec validate rh-relatorios --strict` válido
- [ ] 8.4 Atualizar `CLAUDE.md`
