# test-conventions Specification

## Purpose
Convenções de attributes obrigatórios para métodos de teste xUnit nos projetos `Acme.Sistemas.Services.UnitTest` e `Acme.Sistemas.IntegrationTest`. Estabelece `Trait("Solucao", <Camada>)`, `Trait("Acao", <Unidade>)` e `Fact(DisplayName = "...")` como obrigatórios para garantir test runner legível, filtro por dimensão e enforcement automatizado via analyzer.

## Requirements

### Requirement: Todo método de teste xUnit declara DisplayName, Trait(Solucao) e Trait(Acao)

O sistema SHALL exigir que todo método público anotado com `[Fact]` ou `[Theory]` (incluindo `[Fact(Skip=...)]`) nos projetos `Acme.Sistemas.Services.UnitTest` e `Acme.Sistemas.IntegrationTest` declare:

1. `DisplayName` não-vazio na própria anotação `[Fact(DisplayName = "...")]`/`[Theory(DisplayName = "...")]`, em forma de frase Given-When-Then em PT-BR.
2. `[Trait("Solucao", X)]` onde X é uma das camadas arquiteturais conhecidas: `Api`, `Services`, `Core`, `Domain`, `Repository`, `Infrastructure`, `ExternalIntegration`, `Test`.
3. `[Trait("Acao", Y)]` onde Y é o nome curto da unidade-em-teste (Command/Query, Behavior, Service, Helper, Worker, ou `Convencoes` para meta-tests).

#### Scenario: Test runner exibe DisplayName em PT
- **WHEN** `dotnet test --logger "console;verbosity=detailed"` executa
- **THEN** cada teste é listado pelo `DisplayName`, não pelo nome técnico do método
- **THEN** a frase é Given-When-Then em PT-BR

#### Scenario: Filtro por camada lista apenas testes daquela camada
- **WHEN** `dotnet test --filter "Trait=Solucao=Services"` executa
- **THEN** apenas métodos com `[Trait("Solucao", "Services")]` rodam
- **THEN** nenhum teste de `Api`, `Core`, `Repository`, `Infrastructure`, `Test` é incluído

#### Scenario: Filtro por ação lista todos os testes daquela unidade
- **WHEN** `dotnet test --filter "Trait=Acao=CriarDespesa"` executa
- **THEN** todos os métodos com `[Trait("Acao", "CriarDespesa")]` rodam — independente da camada (handler unit + endpoint integration)

### Requirement: Conformidade verificada por analyzer em CI

O sistema SHALL falhar a suíte de testes quando algum método `[Fact]`/`[Theory]` descumpre o padrão (faltando DisplayName, Trait Solucao em allow-list, ou Trait Acao não-vazio).

#### Scenario: Analyzer reprova método sem DisplayName
- **WHEN** um teste é adicionado sem `DisplayName`
- **THEN** o test `TodoTeste_TemDisplayNameESolucaoEAcao` em `ConvencoesBlueprintTests` falha
- **THEN** a mensagem identifica o método ofensor (`<TipoCompleto>.<NomeMetodo>: faltando DisplayName`)

#### Scenario: Analyzer reprova Trait("Solucao") fora da allow-list
- **WHEN** um teste declara `[Trait("Solucao", "Frontend")]` (valor inválido)
- **THEN** o analyzer falha exigindo um valor da allow-list documentada (Api/Services/Core/Domain/Repository/Infrastructure/ExternalIntegration/Test)

#### Scenario: Analyzer cobre ambos os assemblies de teste
- **WHEN** o analyzer executa
- **THEN** itera tipos de `Acme.Sistemas.Services.UnitTest` E `Acme.Sistemas.IntegrationTest`
- **THEN** nenhum método de teste escapa por estar em outro projeto
