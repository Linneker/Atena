## Why

A suíte de testes do Atena (~58 fatos em 22 arquivos entre Unit e Integration) usa `[Fact]`/`[Theory]` puro — sem `DisplayName` e sem `[Trait(...)]`. Isso causa três dores concretas:

1. **Test runner ilegível** — IDE e `dotnet test` listam o nome técnico do método (`Login_CredenciaisInvalidas_Retorna401`), em vez de uma frase Given-When-Then que descreve o cenário em PT.
2. **Sem filtro por dimensão** — não dá pra rodar "só os testes da camada Services" ou "só os testes da ação CriarDespesa" via `--filter Trait=...`. CI hoje só filtra por `FullyQualifiedName`, o que acopla naming convention ao filtro.
3. **Sem garantia de continuidade** — qualquer novo `[Fact]` reverte ao padrão zero. Não há analyzer reprovando.

O blueprint Acme já norma layout de Endpoint, Command, Behavior etc. — falta normar layout de teste. Esta change preenche esse buraco.

## What Changes

- **Padrão obrigatório** para todo método `[Fact]`/`[Theory]` nos projetos de teste:
  - `[Trait("Solucao", "<Camada>")]` — camada arquitetural (Api, Services, Core, Domain, Repository, Infrastructure, ExternalIntegration, Test).
  - `[Trait("Acao", "<Unidade>")]` — nome curto da unidade-em-teste (Command/Query name, Behavior, Service, Helper, ou `Convencoes` para meta-tests).
  - `[Fact(DisplayName = "...")]` — frase Given-When-Then em PT.
- **Analyzer em `ConvencoesBlueprintTests`** que falha o build se algum método de teste descumprir o padrão.
- **Retrofit completo** dos 22 arquivos × ~58 fatos.
- **Documentação** no `blueprint.yml` + `ESTRUTURA_PADRAO_PROJETOS_ACME.md` + `CLAUDE.md`.

## Capabilities

### Added Capabilities

- `test-conventions` — convenções de attributes para métodos de teste xUnit.

## Out of Scope

- Mudar test runner (continua xUnit + FluentAssertions + Moq + Bogus).
- Adicionar nova dimensão de Trait além de `Solucao` + `Acao` (ex.: `Categoria` Unit/Integration/Smoke). Pode ser proposto em change futura — hoje a separação por projeto (Unit/Integration) já cobre.
- Reorganização física de arquivos de teste.
- DisplayName dinâmico em `[Theory]` (aceitamos o estático + suffix automático dos parâmetros).

## Risks

- **DisplayName redundante** — para métodos com nome técnico expressivo (`Login_CredenciaisInvalidas_Retorna401`), o DisplayName Given-When-Then duplica informação. Aceito como preço da legibilidade no test runner.
- **Bikeshed na Acao** — para tests transversais (PipelineBehavior, Convencoes), a granularidade do Trait pode virar discussão. Mitigação: lista finita de exceções documentada no spec.
- **Regressão na CI** — se algum método ficar fora do padrão e o analyzer reprovar, build quebra. Mitigação: implementar analyzer **depois** do retrofit, fase 2 só documenta.

## Success Criteria

- 100% dos métodos `[Fact]`/`[Theory]` (incluindo os com `Skip`) têm `DisplayName` + `Trait("Solucao")` + `Trait("Acao")`.
- Analyzer em `ConvencoesBlueprintTests` reprova qualquer regressão.
- `dotnet test --filter "Trait=Solucao=Services"` lista somente testes da camada Services.
- `blueprint.yml` + `CLAUDE.md` documentam o padrão.
- `dotnet build` + `dotnet test` (unit) verdes após retrofit.
