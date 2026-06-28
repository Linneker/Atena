# Tasks — esocial-tabelas

## Fase 1 — Estrutura
- [ ] 1.1 Criar pastas `Eventos/V1_2/Tabelas/SXXXX/` para 7 eventos
- [ ] 1.2 Estudar XSD oficial v1.2 e baixar para `Eventos/V1_2/xsd/`

## Fase 2 — S-1000 Empregador
- [ ] 2.1 POCO S1000 + sub-records
- [ ] 2.2 S1000Builder (mapeia EmpregadorEsocial)
- [ ] 2.3 S1000Validator (XSD)
- [ ] 2.4 Command `GerarEventoS1000`
- [ ] 2.5 Hook em `EmpregadorEsocialRepository.Save`
- [ ] 2.6 Endpoint força regeração
- [ ] 2.7 Unit test builder + validator
- [ ] 2.8 Integration: transmite Restrita → Aceito

## Fase 3 — S-1005 Estabelecimentos
- [ ] 3.1 POCO + Builder + Validator
- [ ] 3.2 Hook em LotacaoRepository
- [ ] 3.3 Orquestrador: verifica S-1000 Aceito antes
- [ ] 3.4 Tests

## Fase 4 — S-1010 Rubricas
- [ ] 4.1 POCO + Builder + Validator
- [ ] 4.2 Mapper de incidências (incideINSS, etc → codigosIncidencia eSocial)
- [ ] 4.3 Hook em RubricaTenantRepository
- [ ] 4.4 Tests

## Fase 5 — S-1020 Lotações Tributárias
- [ ] 5.1 POCO + Builder + Validator
- [ ] 5.2 Hook
- [ ] 5.3 Tests

## Fase 6 — S-1070 Processos
- [ ] 6.1 POCO + Builder + Validator
- [ ] 6.2 CRUD admin para processos administrativos/judiciais
- [ ] 6.3 Tests

## Fase 7 — S-1080 Operadores Portuários (opcional)
- [ ] 7.1 Marcar como condicional ao setor portuário

## Fase 8 — S-1280 Informações Complementares (desoneração)
- [ ] 8.1 POCO + Builder + Validator
- [ ] 8.2 Hook em Empresa.desoneracaoFolha
- [ ] 8.3 Tests

## Fase 9 — Orquestração
- [ ] 9.1 `OrquestradorTabelasEsocial.GarantirOrdemAsync`
- [ ] 9.2 Endpoint admin `POST /esocial/tabelas/sincronizar-tudo` (envia tudo na ordem)

## Fase 10 — Frontend
- [ ] 10.1 Tela "Status Tabelas eSocial" — visão por evento
- [ ] 10.2 Ação "Re-transmitir" individual
- [ ] 10.3 Visualizador de XML enviado/retorno

## Fase 11 — Testes e validação
- [ ] 11.1 Smoke: ciclo S-1000 + S-1005 + S-1010 em Restrita → todos Aceito
- [ ] 11.2 Retificação: alterar rubrica gera S-1010 com indRetif=2
- [ ] 11.3 Exclusão: deletar rubrica gera S-3000 (em W14)
- [ ] 11.4 `openspec validate esocial-tabelas --strict` válido
- [ ] 11.5 Docs `documentacao/rh/esocial-tabelas.md`
