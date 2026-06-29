# Mobile MAUI (cliente)

## Propósito

App **.NET MAUI** nativo para colaboradores baterem ponto, verem espelho,
solicitarem ajustes. Para o lado servidor (endpoints, handlers, push), ver
`rh-mobile-w3.md`. Este arquivo cobre só a **estrutura do app** e como o
desenvolvedor mobile mexe nele.

## Projetos

```
src/Mobile/
├── Acme.Sistemas.Atena.Mobile/           ← MAUI app multi-target
│   TargetFrameworks: net10.0-android, net10.0-ios,
│                     net10.0-maccatalyst, net10.0-windows10.0.19041.0
├── Acme.Sistemas.Atena.Mobile.Shared/    ← Class Library net10.0 puro
└── ...
test/Mobile/
└── Acme.Sistemas.Atena.Mobile.Tests/     ← xUnit
```

App ID: `br.com.acme.atena.mobile`.

## Setup do projeto

`MauiProgram.cs` registra no DI:
- `AppSettings` (singleton)
- `ISecureTokenStore` → `SecureTokenStore` (wrap `Microsoft.Maui.Storage.SecureStorage`)
- `IConnectivityService` (event `StatusMudou` ouvindo `Connectivity.ConnectivityChanged`)
- `IDeviceCapabilityHelper` (detecta câmera, biometria)
- `IBiometriaService` (stub que retorna prova `"local-bio:hash"`)
- `ICameraService` (`MediaPicker.CapturePhotoAsync`)
- `IGeoService` (`Geolocation.GetLocationAsync` com tratamento de exceções)
- `IOfflineQueue` → `SqliteOfflineQueue` (sqlite-net-pcl)
- `IAuthService`, `INotificationService`
- `IAtenaApi` via **Refit** com `AuthDelegatingHandler` + Polly retry
- Todos ViewModels + Views como **transient**

`App.xaml.cs` overrides:
- `OnResume()` — dispara sync da `IOfflineQueue` + check de versão mínima
- `CreateWindow()` — instancia `AppShell`, dispara check de versão

`AppShell.xaml.cs` — guard `OnNavigated` redireciona para Login se sem token.

## Telas (7 ViewModels + 7 Pages)

| Tela | ViewModel | Notas |
|------|-----------|-------|
| Login | `LoginViewModel` | E-mail + senha + botão. Chama `IAuthService.LoginAsync` |
| PrimeiroAcesso | `PrimeiroAcessoViewModel` | Inspeciona capacidades, registra dispositivo |
| Home | `HomeViewModel` | Timer 1s para `HoraAtual`; Online via `ConnectivityService` event |
| BaterPonto | `BaterPontoViewModel` | Orquestra: capacidade → câmera/bio → GPS → hash → online (multipart) ou offline (enqueue) |
| EspelhoMensal | `EspelhoMensalViewModel` | Carrega espelho; drill-down `DiaSelecionado`; comando `SolicitarAjusteParaBatidaAsync` (DisplayPromptAsync para hora + motivo) |
| Ajustes | `AjustesPageViewModel` | Lista próprios ajustes + `SolicitarNovoAjusteAsync` |
| Configurações | `ConfiguracoesViewModel` | AppVersion, OS, Plataforma, Modelo + Logout |

XAML usa `x:DataType` (AOT-friendly bindings).

## Mobile.Shared — DTOs + Helpers

`Acme.Sistemas.Atena.Mobile.Shared/`:

- `Dtos/AuthDtos.cs` — `LoginMobileRequest/Response`, `RefreshTokenRequest/Response`
- `Dtos/PontoDtos.cs` — `BaterPontoMobileForm`, `MarcacaoDto`, `EspelhoMensalDto`,
  `SolicitarAjusteRequest`, etc.
- `Dtos/DispositivoDtos.cs` — `RegistrarDispositivoRequest/Response`, etc.
- `Dtos/ConfiguracaoMobileDtos.cs` — versão mínima, banners, branding
- `Helpers/HashHelpers.cs` — `CalcularHashBatida(funcId, ts, tipo, deviceId)`
  igual ao validador do servidor. `Sha256Hex(input)`.
- `Helpers/Formatadores.cs` — `MinutosParaHoras`, `MinutosParaHorasComSinal`
  (sinal `+` ou `-`), `FormatarCpf`, `FormatarData`, `FormatarDataHora`

**Por que target `net10.0` puro?** Para ser testável sem MAUI workload no
agente — ViewModels e helpers podem ser exercitados sem precisar do runtime
ios/android.

## Plataformas

| Pasta | Arquivos chave |
|-------|----------------|
| `Platforms/Android/` | `MainActivity`, `MainApplication`, `AndroidManifest.xml` (INTERNET, CAMERA, ACCESS_LOCATION, USE_BIOMETRIC, POST_NOTIFICATIONS, RECEIVE_BOOT_COMPLETED, FOREGROUND_SERVICE) |
| `Platforms/iOS/` | `AppDelegate`, `Program`, `Info.plist` (NSCameraUsageDescription, NSLocationWhenInUseUsageDescription, NSFaceIDUsageDescription, MinimumOSVersion 14.0) |
| `Platforms/MacCatalyst/` | `AppDelegate`, `Program`, `Info.plist` |
| `Platforms/Windows/` | `App.xaml(.cs)`, `Package.appxmanifest` (webcam + location capabilities) |

## Resources

- `Resources/AppIcon/appicon.svg`, `appiconfg.svg`
- `Resources/Splash/splash.svg`
- `Resources/Raw/AboutAssets.txt`

## AppSettings & ApiBaseUrl

`AppSettings.cs`:
- Debug Android → `http://10.0.2.2:5000` (mapeia para localhost do host emulador)
- Debug outras plataformas → `http://localhost:5000`
- Produção → `https://api.atena.com.br`

## Offline Queue

`SqliteOfflineQueue` cria tabela `PendingBatida`:
- `Id`, `PayloadJson`, `FotoPath`, `CriadoEm`, `Status` (Pending/Synced/Failed), `Tentativas`
- `EnfileirarBatidaAsync` no offline
- `SyncPendentesAsync` tenta enviar até 5x antes de marcar `Failed`
- Disparado em `App.OnResume` + `Connectivity.ConnectivityChanged`

## Comandos

```powershell
# Windows app
dotnet build src/Mobile/Acme.Sistemas.Atena.Mobile -t:Run -f net10.0-windows10.0.19041.0

# Android emulado
dotnet build src/Mobile/Acme.Sistemas.Atena.Mobile -t:Run -f net10.0-android

# Tests
dotnet test test/Mobile/Acme.Sistemas.Atena.Mobile.Tests
```

```bash
# macOS — iOS sim
dotnet build src/Mobile/Acme.Sistemas.Atena.Mobile -t:Run -f net10.0-ios

# macOS — Mac Catalyst
dotnet build src/Mobile/Acme.Sistemas.Atena.Mobile -t:Run -f net10.0-maccatalyst
```

## CI/CD

3 workflows em `.github/workflows/`:
- `mobile-android.yml` — build AAB + sign + publish opcional Play internal
- `mobile-ios.yml` — build IPA + sign + upload opcional TestFlight
- `mobile-windows.yml` — build MSIX

`workflow_dispatch` com `publish=true` (em `develop`) ativa upload. Secrets
em `documentacao/rh/mobile/distribuicao-{android,ios}.md`.

## Tests

`test/Mobile/Acme.Sistemas.Atena.Mobile.Tests/`:
- `HashHelpersTests` — determinismo + sensibilidade a deviceId + SHA-256 vazio
- `FormatadoresTests` — formatos com `Theory` + `InlineData`

ViewModels não têm tests ainda — bloqueado pela referência indireta ao MAUI
no projeto principal. Plano: `rh-mobile-vm-tests` extrai `Mobile.ViewModels`
para Class Library puro para mockar `IAtenaApi`.

## Arquivos para consultar

- `src/Mobile/Acme.Sistemas.Atena.Mobile/MauiProgram.cs`
- `src/Mobile/Acme.Sistemas.Atena.Mobile/App.xaml.cs`
- `src/Mobile/Acme.Sistemas.Atena.Mobile/AppShell.xaml.cs`
- `src/Mobile/Acme.Sistemas.Atena.Mobile/AppSettings.cs`
- `src/Mobile/Acme.Sistemas.Atena.Mobile/ViewModels/`
- `src/Mobile/Acme.Sistemas.Atena.Mobile/Views/`
- `src/Mobile/Acme.Sistemas.Atena.Mobile/Services/`
- `src/Mobile/Acme.Sistemas.Atena.Mobile/Platforms/`
- `src/Mobile/Acme.Sistemas.Atena.Mobile.Shared/Helpers/HashHelpers.cs`
- `documentacao/rh/mobile/` (5 docs operacionais)
- `.github/workflows/mobile-*.yml`

## Follow-ups conhecidos

- `rh-mobile-vm-tests` — extrair ViewModels para projeto puro testável
- `rh-mobile-push-fcm` / `rh-mobile-push-apns` — push real
- `rh-mobile-comprovante-671` — exibir/baixar PDF 671 (W4) no app
- Auto-update do app — hoje só alerta de versão mínima no `App.OnResume`
