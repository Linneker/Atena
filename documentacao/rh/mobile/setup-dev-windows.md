# Setup dev — Atena Mobile (Windows)

## Pré-requisitos

- Windows 11 + Visual Studio 2022 17.10+ ou VS Code com extensão **.NET MAUI Dev Kit**
- .NET SDK **10.0.x**
- Workload: `dotnet workload install maui`
- Android: SDK 34 + emulador (via Android Studio ou `sdkmanager`)
- Windows app: Windows 10 SDK 19041 ou superior

## Estrutura

```
src/Mobile/
├── Acme.Sistemas.Atena.Mobile/         ← MAUI multi-target
├── Acme.Sistemas.Atena.Mobile.Shared/  ← DTOs + Helpers (puros, testáveis)
test/Mobile/
└── Acme.Sistemas.Atena.Mobile.Tests/   ← xUnit
```

## Rodar localmente

```powershell
# Android emulado
dotnet build src/Mobile/Acme.Sistemas.Atena.Mobile -t:Run -f net10.0-android

# Windows (Hot Reload)
dotnet build src/Mobile/Acme.Sistemas.Atena.Mobile -t:Run -f net10.0-windows10.0.19041.0
```

API consumida: `ApiBaseUrl` em `AppSettings.cs`. No Android emulator,
`http://10.0.2.2:5000` aponta para `localhost` do host.

## Testes unitários

```powershell
dotnet test test/Mobile/Acme.Sistemas.Atena.Mobile.Tests
```

Os DTOs e helpers ficam em `Mobile.Shared` (target `net10.0` puro) justamente
para serem testáveis sem precisar do MAUI runtime.
