# Tasks — rh-rescisao

## Fase 1 — Modelo
- [ ] 1.1 Migration `AddTabelaRescisoes`
- [ ] 1.2 Migration `AddTabelaMotivosRescisaoEsocial` (seed)
- [ ] 1.3 Domain `Rescisao` + enums `TipoRescisao`, `TipoAvisoPrevio`, `StatusRescisao`

## Fase 2 — Regras e engine
- [ ] 2.1 `DireitosRescisao` record + `RegrasRescisao.Direitos` (8 tipos)
- [ ] 2.2 `EngineFolhaRescisao.CalcularAsync`
- [ ] 2.3 Helpers: SaldoSalario, AvisoPrevioIndenizado, FeriasVencidas, FeriasProporcionais, Decimo3oProporcional, MultaFgts
- [ ] 2.4 Cálculo de aviso prévio (30d + 3d/ano até 90d)
- [ ] 2.5 Cálculo de FGTS rescisório informativo

## Fase 3 — TRCT PDF
- [ ] 3.1 `GeradorTrctV2Pdf` (QuestPDF) com layout oficial NR-127
- [ ] 3.2 Marca d'água RASCUNHO se não Homologada
- [ ] 3.3 Hash do PDF gravado

## Fase 4 — Workflow
- [ ] 4.1 Command `ProgramarRescisao`
- [ ] 4.2 Command `CalcularRescisao`
- [ ] 4.3 Command `HomologarRescisao`
- [ ] 4.4 Command `ConcluirRescisao` (dispara desligamento + pendências)
- [ ] 4.5 Command `CancelarRescisao` (antes de homologar)
- [ ] 4.6 Query `ObterRescisao`, `ListarRescisoesPorPeriodo`
- [ ] 4.7 Endpoints

## Fase 5 — Pós-rescisão
- [ ] 5.1 Desativar `Funcionario.status = Desligado` + `dataDemissao`
- [ ] 5.2 Desativar `Usuario.status = Desativado`
- [ ] 5.3 Criar pendência para W10 (ContaPagar líquido + GRRF)
- [ ] 5.4 Criar pendência para W13 (S-2299)

## Fase 6 — Frontend
- [ ] 6.1 Tela "Rescisões" lista por status
- [ ] 6.2 Wizard "Nova rescisão" (tipo, datas, motivo, saldo FGTS conhecido)
- [ ] 6.3 Tela "Detalhe da rescisão" com preview de cálculo + TRCT
- [ ] 6.4 Form "Homologar" (data, local, homologador, upload assinatura)
- [ ] 6.5 Confirmação dupla para "Concluir" (ação irreversível)

## Fase 7 — Testes
- [ ] 7.1 8 fixtures (1 por tipo de rescisão), valores conferidos
- [ ] 7.2 Unit: tabela Direitos (8 tipos × 6 flags)
- [ ] 7.3 Unit: aviso prévio por anos de serviço
- [ ] 7.4 Integration: ciclo completo termina com funcionário Desligado + pendências criadas
- [ ] 7.5 `openspec validate rh-rescisao --strict` válido
- [ ] 7.6 Docs `documentacao/rh/rescisao.md`
