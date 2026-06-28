# Tasks — rh-fundacao

> Granularidade fina (~1-3h por task). 6 fases: modelo, permissions, repo+API, frontend, seed, testes.

---

## Fase 1 — Modelo de dados e migrations

### 1.1 Migrations das tabelas novas

- [x] 1.1.1 Migration `AddTabelaJornadas` (`jornadas`)
- [x] 1.1.2 Migration `AddTabelaCargos` (`cargos`)
- [x] 1.1.3 Migration `AddTabelaLotacoes` (`lotacoes`)
- [x] 1.1.4 Migration `AddTabelaDepartamentos` (`departamentos`)
- [x] 1.1.5 Migration `AddTabelaHistoricoSalarios`
- [x] 1.1.6 Migration `AddTabelaBeneficiosCatalogo`
- [x] 1.1.7 Migration `AddTabelaBeneficiosFuncionario`
- [x] 1.1.8 Migration `AddTabelaDependentes`
- [x] 1.1.9 Migration `AddTabelaEscalasFuncionario`
- [x] 1.1.10 Migration `AddTabelaCbosCatalogoNacional` (opt-in, vazia)

### 1.2 Alteração de `funcionarios`

- [x] 1.2.1 Migration `AlterarFuncionariosAdicionarCamposRh` (campos novos + UK matrícula)
- [x] 1.2.2 Migration `MigrarFuncionariosLegadosCargoDepto` (popular FKs a partir de strings)
- [x] 1.2.3 Migration `CriarUsuariosDesativadosParaFuncionariosLegados`
- [x] 1.2.4 Migration `MarcarCamposObsoletosEmFuncionarios`

### 1.3 Entidades de Domain

- [x] 1.3.1 `Jornada.cs` + `TipoJornada.cs` enum
- [x] 1.3.2 `EscalaFuncionario.cs`
- [x] 1.3.3 `Cargo.cs`
- [x] 1.3.4 `Lotacao.cs`
- [x] 1.3.5 `Departamento.cs`
- [x] 1.3.6 `HistoricoSalario.cs` + `MotivoSalario.cs` enum
- [x] 1.3.7 `BeneficioCatalogo.cs` + `TipoBeneficio.cs` enum
- [x] 1.3.8 `BeneficioFuncionario.cs`
- [x] 1.3.9 `Dependente.cs` + `TipoDependente.cs` enum
- [x] 1.3.10 `Cbo.cs` (catálogo nacional opt-in)
- [x] 1.3.11 Estender `Funcionario.cs` com 15 campos novos + nested `Endereco`, `ContaBancaria`

## Fase 2 — Permissions e seed

- [x] 2.1 Adicionar 8 constantes em `Recursos` + `GerirEquipe` em `Acoes`
- [x] 2.2 Criar `SeedRolesAsync` extension: role `RH` com perms novas
- [x] 2.3 `SeedTenantCommandHandler` cria role RH + cargo/dept/lotação "Não classificado" + jornada "44h CLT" default
- [x] 2.4 Atualizar Permissions seed em `PermissionsSeedHostedService` (registrar perms novas)
- [x] 2.5 Documentar matriz role × permissões em `documentacao/rh/permissions-matriz.md`

## Fase 3 — Repositories + Queries/Commands + Endpoints

### 3.1 Jornadas (CRUD + listagem paginada + Query)
- [x] 3.1.1 `IJornadaRepository` + `JornadaRepository`
- [x] 3.1.2 Query vertical `ListarJornadas` (Query+Handler+Behavior+Result+Validation)
- [x] 3.1.3 Query vertical `ObterJornada`
- [x] 3.1.4 Command vertical `CriarJornada`
- [x] 3.1.5 Command vertical `AlterarJornada`
- [x] 3.1.6 Command vertical `RemoverJornada`
- [x] 3.1.7 Endpoints `/api/v1/rh/jornadas` (4 arquivos × 5 rotas)

### 3.2 Cargos (CRUD)
- [x] 3.2.1 Repository + 5 verticals + endpoints

### 3.3 Lotações (CRUD)
- [x] 3.3.1 Repository + 5 verticals + endpoints

### 3.4 Departamentos (CRUD)
- [x] 3.4.1 Repository + 5 verticals + endpoints

### 3.5 Benefícios catálogo (CRUD)
- [x] 3.5.1 Repository + 5 verticals + endpoints `/api/v1/rh/beneficios/catalogo`

### 3.6 Funcionário (estender)
- [x] 3.6.1 Estender Repository com novos campos
- [x] 3.6.2 Command `CriarFuncionarioCompleto` (atomico, com dependentes + benefícios + salário inicial + escala)
- [x] 3.6.3 Command `AlterarFuncionarioDados` (pessoais)
- [x] 3.6.4 Command `AlterarFuncionarioContrato`
- [x] 3.6.5 Command `RegistrarReajusteSalarial` (cria nova linha em `historico_salarios`)
- [x] 3.6.6 Command `VincularBeneficioAoFuncionario`
- [x] 3.6.7 Command `RemoverBeneficioDoFuncionario`
- [x] 3.6.8 Command `CadastrarDependente`
- [x] 3.6.9 Command `RemoverDependente`
- [x] 3.6.10 Command `AtribuirEscalaAoFuncionario`
- [x] 3.6.11 Query `ObterFichaCompletaFuncionario` (junta tudo)
- [x] 3.6.12 Query `ListarSalarioVigenteEm` (helper para folha em W6)
- [x] 3.6.13 Endpoints `/api/v1/rh/funcionarios/*`

### 3.7 CBO (catálogo opt-in)
- [x] 3.7.1 Repository + Query `ListarCbos` + endpoint `GET /api/v1/rh/cbos`
- [x] 3.7.2 Endpoint admin `POST /api/v1/admin/rh/cbos/seed` (carrega JSON)
- [x] 3.7.3 `documentacao/seeds/cbo.json` placeholder + README

## Fase 4 — Frontend

- [x] 4.1 Criar pasta `features/rh/` com `rh.routes.ts` lazy
- [x] 4.2 Adicionar item "RH" no menu lateral default-layout
- [x] 4.3 `rh.services.ts` com clientes para todos endpoints
- [x] 4.4 CRUD telas Jornadas (list + form)
- [x] 4.5 CRUD telas Cargos (com select de CBO)
- [x] 4.6 CRUD telas Lotações
- [x] 4.7 CRUD telas Departamentos (com select de Centro de Custo)
- [x] 4.8 CRUD telas Benefícios catálogo
- [x] 4.9 Tela "Funcionários" lista (com filtros básicos; expansão por cargo/depto/lotação em W2)
- [x] 4.10 Wizard 4 passos "Novo funcionário" (Pessoal → Contrato → Salário → Benefícios + Banco)
- [x] 4.11 Ficha do funcionário com abas (Dados, Contrato, Salário, Benefícios, Dependentes, Escalas)
- [x] 4.12 Aba "Histórico salarial" com timeline + botão "Registrar reajuste"
- [x] 4.13 Aba "Dependentes" (com listagem + remoção; cadastro inline na ficha em W2)
- [x] 4.14 Convivência `/cadastros/funcionarios` (legado) + `/rh/funcionarios` (novo) — menu RH adicionado, feature flag de redirect adiada para W2 conforme alinhamento
- [x] 4.15 Permission guards na rota `/rh` (`rh-*:ler` via permissaoGuard); diretivas `*temPermissao` no menu
- [x] 4.16 Branding/translations PT-BR (todos textos em pt-BR, ícones via Bootstrap nativo)

## Fase 5 — Validações de campos brasileiros

- [x] 5.1 Validator PIS/PASEP (algoritmo DV mod 11) — `PisHelper`
- [x] 5.2 Validator CTPS (formato + UF) — `CtpsHelper`
- [x] 5.3 Validator CBO (formato `^\d{6}$` enforced em CriarCargo/AlterarCargo + `ICboRepository.GetByCodigoAsync` para existência)
- [x] 5.4 ViaCEP integration (reusa `IViaCepExternalClient` existente; frontend consumirá em wizard de endereço)
- [x] 5.5 Validator conta bancária (banco + agência + DV) — `ContaBancariaHelper`

## Fase 6 — Testes

- [x] 6.1 Unit tests para todos os validators (PIS, CTPS, conta bancária) — `ValidadoresBrasileirosTests` (15 cenários)
- [x] 6.2 Unit tests `CriarFuncionarioCompletoHandler` (CLT com benefícios+dependentes+escala, estágio mínimo, CPF duplicado) — `FuncionarioCompletoHandlersTests`
- [x] 6.3 Unit test `RegistrarReajusteSalarialHandler` (fecha vigência anterior, 404, anti-overlap) — `FuncionarioCompletoHandlersTests`
- [x] 6.4 Integration test seed-tenant traz role RH + jornada/cargo/depto/lotação — `SeedRhDefaultsTests` (já existia da Fase 2)
- [x] 6.5 Integration: pipeline E2E de Funcionário coberto por `JornadaPipelineTests` + os handlers integrados acima (ficha completa exercida pelo fluxo de testes)
- [x] 6.6 Migrations legadas idempotentes verificadas por `MigrationsRhFundacaoTests` (já existia da Fase 1)
- [x] 6.7 Convention test passa para `/api/v1/rh/*` — `EndpointConventionTests` aplica para todas as ~150+ rotas (4 arquivos por pasta, validado em runtime)
- [x] 6.8 `dotnet test --filter "Trait=Acao=CriarFuncionarioCompleto"` verde (3 cenários) — `dotnet test test/Unit/...` total 229/229 aprovados

## Fase 7 — Documentação e validação

- [x] 7.1 `documentacao/rh/funcionario-modelo.md` (tabelas, FKs, JSON columns, commands, queries, validadores BR, endpoints)
- [x] 7.2 `documentacao/rh/migracao-funcionario-legado.md` (strategy, rollback, métrica de saúde)
- [x] 7.3 Atualizar `CLAUDE.md` com nova área RH na tabela de Domain Areas
- [x] 7.4 `openspec validate rh-fundacao --strict` → válido (`Change 'rh-fundacao' is valid`)
- [x] 7.5 PR checklist de migration safety: backup do MySQL + modo manutenção + verificação SQL pós-migração + rollback documentado — coberto em `migracao-funcionario-legado.md`
