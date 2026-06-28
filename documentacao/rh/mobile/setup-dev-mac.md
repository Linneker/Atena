# Setup dev — Atena Mobile (macOS)

## Pré-requisitos

- macOS 14 (Sonoma) ou superior
- Xcode 16+ (necessário para `net10.0-ios` e `net10.0-maccatalyst`)
- .NET SDK **10.0.x** (`brew install dotnet-sdk` ou installer oficial)
- Workload: `dotnet workload install maui maui-ios maui-maccatalyst`

## Rodar localmente

```bash
# iOS simulator (escolhe o primeiro disponível)
dotnet build src/Mobile/Acme.Sistemas.Atena.Mobile -t:Run -f net10.0-ios

# Mac Catalyst
dotnet build src/Mobile/Acme.Sistemas.Atena.Mobile -t:Run -f net10.0-maccatalyst
```

Para device físico:

1. Plug o iPhone via USB e confie no computador.
2. Provisionar via Xcode (abrir um projeto vazio para gerar profile).
3. `dotnet build ... -t:Run -f net10.0-ios -p:RuntimeIdentifier=ios-arm64`.

## Testes unitários

```bash
dotnet test test/Mobile/Acme.Sistemas.Atena.Mobile.Tests
```

Os helpers e DTOs vivem em `Mobile.Shared` (target `net10.0` puro), portanto
o teste roda sem precisar dos workloads ios/android.
