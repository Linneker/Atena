# Tasks — esocial-nao-periodicos

## Fase 1 — Estrutura
- [ ] 1.1 Pastas para 10 tipos de evento
- [ ] 1.2 Estudar XSD v1.2 para cada tipo

## Fase 2 — S-2200 Admissão (mais complexo, prioritário)
- [ ] 2.1 POCOs + sub-records
- [ ] 2.2 S2200Builder (mapeia Funcionario completo)
- [ ] 2.3 S2200Validator (XSD + regras de negócio)
- [ ] 2.4 Command GerarEventoS2200
- [ ] 2.5 Hook em FuncionarioRepository (novo ativo)
- [ ] 2.6 Tests builder + integration

## Fase 3 — S-2205 Alterações Cadastrais
- [ ] 3.1 POCO + Builder + Validator
- [ ] 3.2 Detector `MudouCamposCadastrais(antes, depois)`
- [ ] 3.3 Hook + tests

## Fase 4 — S-2206 Alterações Contratuais
- [ ] 4.1 POCO + Builder + Validator
- [ ] 4.2 Detector `MudouCamposContratuais` (cargo, salario via HistoricoSalario, jornada)
- [ ] 4.3 Hook + tests

## Fase 5 — S-2230 Afastamento
- [ ] 5.1 POCO + Builder + Validator
- [ ] 5.2 Hook em AfastamentoRepository
- [ ] 5.3 Encerramento de afastamento gera S-2230 alteração
- [ ] 5.4 Tests

## Fase 6 — S-2250 Aviso Prévio
- [ ] 6.1 POCO + Builder
- [ ] 6.2 Hook em Rescisao.Programada com aviso indenizado
- [ ] 6.3 Tests

## Fase 7 — S-2298 Reintegração (raro)
- [ ] 7.1 POCO + Builder + endpoint manual

## Fase 8 — S-2299 Desligamento
- [ ] 8.1 POCO + Builder (mapeia Rescisao + funcionario)
- [ ] 8.2 Inclui motivo eSocial (mapeamento de TipoRescisao → cod motivo)
- [ ] 8.3 Hook em Rescisao.Concluida
- [ ] 8.4 Tests

## Fase 9 — S-2300/2306/2399 TSVE (Trabalhador sem vínculo)
- [ ] 9.1 POCOs + Builders
- [ ] 9.2 Hooks (TipoContrato in EstagioRemunerado, AutonomoRpa)
- [ ] 9.3 Tests

## Fase 10 — Orquestração
- [ ] 10.1 `OrquestradorNaoPeriodicos` valida ordem
- [ ] 10.2 Trabalhador com S-2200 Aceito gate

## Fase 11 — Frontend
- [ ] 11.1 Tela "Eventos eSocial do funcionário" (timeline)
- [ ] 11.2 Ação "Reenviar" individual
- [ ] 11.3 Visualizador XML

## Fase 12 — Testes e validação
- [ ] 12.1 Smoke: admissão → alteração contratual → desligamento em Restrita
- [ ] 12.2 Hook automático: criar funcionário gera S-2200 sem ação manual
- [ ] 12.3 `openspec validate esocial-nao-periodicos --strict` válido
- [ ] 12.4 Docs `documentacao/rh/esocial-nao-periodicos.md`
