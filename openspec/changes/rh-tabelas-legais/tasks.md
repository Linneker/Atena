# Tasks — rh-tabelas-legais

## Fase 1 — Modelo
- [ ] 1.1 Migration `AddTabelaInss` (com seed 2026)
- [ ] 1.2 Migration `AddTabelaIrrf` (com seed 2026)
- [ ] 1.3 Migration `AddTabelaFgts` (8% + 40% multa)
- [ ] 1.4 Migration `AddTabelaSalarioMinimoNacional`
- [ ] 1.5 Migration `AddTabelaSalarioMinimoRegional`
- [ ] 1.6 Migration `AddTabelaSalarioFamilia`
- [ ] 1.7 Migration `AddTabelaValeTransporte` (regra fixa)
- [ ] 1.8 Migration `AddTabelaFeriadosNacionais` (com seed 2026)
- [ ] 1.9 Migration `AddTabelaFeriadosEstaduais` (vazia, opt-in)
- [ ] 1.10 Migration `AddTabelaFeriadosMunicipais` (vazia, opt-in)
- [ ] 1.11 Migration `AddTabelaNaturezasRubricaEsocial` (seed completo S-1010)
- [ ] 1.12 Migration `AddTabelaRubricasCatalogoNacional` (seed ~30 rubricas modelo)
- [ ] 1.13 Migration `AddTabelaRubricasTenant` (vazia, tenant-scoped)
- [ ] 1.14 Domain: classes por tabela + enum `TipoRubrica`, `OrigemRubrica`

## Fase 2 — Repositórios e Queries

- [ ] 2.1 `ITabelaInssRepository` + impl + cache wrapper
- [ ] 2.2 `ITabelaIrrfRepository` + impl + cache
- [ ] 2.3 `ITabelaFgtsRepository` + impl + cache
- [ ] 2.4 `ISalarioMinimoRepository` + impl + cache
- [ ] 2.5 `ITabelaSalarioFamiliaRepository`
- [ ] 2.6 `IFeriadoRepository` (com fallback nac→est→mun)
- [ ] 2.7 `INaturezaRubricaEsocialRepository`
- [ ] 2.8 `IRubricaCatalogoRepository`
- [ ] 2.9 `IRubricaTenantRepository`
- [ ] 2.10 Query verticals: `ObterTabelaInssVigente`, `ObterTabelaIrrfVigente`, `ObterSalarioMinimoVigente`, `ListarFeriados`, `ListarNaturezasEsocial`, `ListarRubricasTenant`, `ObterRubricaTenant`
- [ ] 2.11 Endpoints públicos (autenticado): `/api/v1/rh/tabelas/{tipo}`, `/api/v1/rh/rubricas`

## Fase 3 — Upload admin

- [ ] 3.1 Permissão `admin:upload-tabelas-legais`, role `RhAdmin`
- [ ] 3.2 `IParserUploadTabela<T>` interface
- [ ] 3.3 Impls: `ParserInss`, `ParserIrrf`, `ParserFgts`, `ParserSM`, `ParserSalarioFamilia`, `ParserFeriadosNac`, `ParserFeriadosEst`, `ParserFeriadosMun`, `ParserNaturezasEsocial`
- [ ] 3.4 Command `UploadTabelaLegal` (genérico — dispatch por tipo)
- [ ] 3.5 Endpoint `POST /api/v1/admin/rh/tabelas/{tipo}/upload` (multipart)
- [ ] 3.6 Invalidação de cache via evento `TabelasLegaisAtualizadas`
- [ ] 3.7 Audit log obrigatório (quem, quando, override, tamanho do arquivo)

## Fase 4 — DSL de rubrica

- [ ] 4.1 Decidir biblioteca de parser: Sprache vs NCalc vs ANTLR — escrever spike de 1 dia
- [ ] 4.2 Gramática formal `RubricaDsl.g4` (se ANTLR) ou parser Sprache
- [ ] 4.3 AST tipada
- [ ] 4.4 Validador (whitelist de funções, ausência de loops, profundidade limitada)
- [ ] 4.5 `RubricaContexto` (dicionário de variáveis disponíveis)
- [ ] 4.6 `RubricaExpressionEvaluator.Avaliar(dsl, ctx)` com timeout
- [ ] 4.7 Funções built-in: `min, max, abs, round, floor, ceil`
- [ ] 4.8 Funções tabela: `aplicaTabelaInss(rem, comp)`, `aplicaTabelaIrrf(base, deps, comp)`
- [ ] 4.9 Funções calendário: `diasUteis(ano,mes)`, `diasMes(ano,mes)`, `eFeriado(data,uf?,mun?)`
- [ ] 4.10 Detector de ciclo em dependências
- [ ] 4.11 Topological sort para ordem de cálculo

## Fase 5 — CRUD de rubricas tenant + clonar

- [ ] 5.1 Command `CriarRubricaTenant` (valida DSL antes de salvar)
- [ ] 5.2 Command `AlterarRubricaTenant`
- [ ] 5.3 Command `RemoverRubricaTenant` (soft delete se já usada em folha)
- [ ] 5.4 Command `ClonarRubricaDoCatalogo`
- [ ] 5.5 Command `TestarRubricaTenant` (recebe contexto simulado, retorna resultado)
- [ ] 5.6 Endpoints
- [ ] 5.7 Validação no salvamento: testa rubrica com contexto típico antes de aceitar

## Fase 6 — Frontend

- [ ] 6.1 Tela "Tabelas Legais" (admin) — listar vigências por tipo
- [ ] 6.2 Tela "Upload de tabela" (admin) — upload JSON/CSV com preview
- [ ] 6.3 Tela "Rubricas" (RH/Tenant) — CRUD com editor de DSL
- [ ] 6.4 Editor DSL com autocomplete de variáveis e funções (monaco-editor)
- [ ] 6.5 Botão "Testar rubrica" com formulário de contexto simulado
- [ ] 6.6 Visualização de dependências entre rubricas (grafo)
- [ ] 6.7 Tela "Calendário de Feriados" (consulta + upload)

## Fase 7 — Seed inicial

- [ ] 7.1 Migrations 1.1-1.4 já trazem dados 2026 inline
- [ ] 7.2 Estender `SeedTenantCommandHandler` para criar 10 rubricas padrão básicas para o tenant (salário-base, HE 50%, INSS desc, IRRF desc, FGTS info, VT desc, VT recebido, DSR sobre HE, banco horas, salário-família)
- [ ] 7.3 `documentacao/rh/rubricas-padrao.md` documenta as 10 default

## Fase 8 — Testes

- [ ] 8.1 Unit: parser DSL — 30 válidas + 20 inválidas
- [ ] 8.2 Unit: evaluator — 50 expressões com expected
- [ ] 8.3 Unit: `aplicaTabelaInss` em 5 valores diferentes (faixa baixa, média, alta, teto)
- [ ] 8.4 Unit: validador de ciclo em dependências
- [ ] 8.5 Integration: upload INSS → consulta retorna novos valores → cache invalidou
- [ ] 8.6 Integration: tenant novo via seed-tenant já tem 10 rubricas padrão
- [ ] 8.7 `openspec validate rh-tabelas-legais --strict` válido
- [ ] 8.8 Documentação:
  - `documentacao/rh/tabelas-legais.md`
  - `documentacao/rh/uploads-tabelas-formato.md`
  - `documentacao/rh/rubricas-dsl.md`
