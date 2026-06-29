# RH Mobile (W3)

## Propósito

App nativo **.NET MAUI** multi-plataforma (Android/iOS/Windows/macOS) para
colaboradores baterem ponto, verem espelho, solicitarem ajustes e receberem
push notifications. Inclui endpoints servidor de suporte (login mobile,
bater-mobile, dispositivos, configuração).

## Projetos .NET

| Projeto | TargetFramework | Função |
|---------|-----------------|--------|
| `Acme.Sistemas.Atena.Mobile` | `net10.0-android;net10.0-ios;net10.0-maccatalyst;net10.0-windows10.0.19041.0` | App MAUI |
| `Acme.Sistemas.Atena.Mobile.Shared` | `net10.0` | DTOs + Helpers puros, testáveis |
| `Acme.Sistemas.Atena.Mobile.Tests` | `net10.0` | xUnit + FluentAssertions + Moq |

App ID: `br.com.acme.atena.mobile`.

## Stack do app

- **MVVM** com `CommunityToolkit.Mvvm` (`[ObservableProperty]`, `[RelayCommand]`,
  `ObservableObject`)
- **HTTP** via `Refit` (`IAtenaApi`) + `AuthDelegatingHandler` (Bearer + refresh
  em 401) + Polly retry (3 tentativas exponencial)
- **Token store**: `SecureTokenStore` wrap `Microsoft.Maui.Storage.SecureStorage`
  (Android Keystore / iOS Keychain / Windows PasswordVault)
- **Offline queue**: `SqliteOfflineQueue` (sqlite-net-pcl) — enfileira batidas
  sem rede; sync dispara em `App.OnResume` + `Connectivity.ConnectivityChanged`
- **Hash mobile**: `HashHelpers.CalcularHashBatida(funcId, timestamp, tipo, deviceId)`
  reproduz o mesmo SHA-256 do W2 + servidor valida

## 7 telas (ViewModels + Pages)

| Tela | Função |
|------|--------|
| `LoginPage` | E-mail/senha → `login-mobile` |
| `PrimeiroAcessoPage` | Inspeciona capacidades (câmera, bio) + registra dispositivo |
| `HomePage` | Saudação + relógio + botão grande + status online/offline |
| `BaterPontoPage` | Captura foto/GPS/bio → hash → submit ou enqueue offline |
| `EspelhoMensalPage` | Calendário com saldos; drill-down em dia; "solicitar ajuste" |
| `AjustesPage` | Lista próprios ajustes + status |
| `ConfiguracoesPage` | Perfil, dispositivo, AppVersion, OS, logout |

## Backend — endpoints do W3

| Método | Rota | Permissão |
|--------|------|-----------|
| POST | `/api/v1/autenticacao/login-mobile` | público (refresh 90d) |
| POST | `/api/v1/rh/ponto/bater-mobile` | `rh-ponto:bater-ponto` (multipart com foto) |
| POST | `/api/v1/mobile/dispositivos/registrar` | autenticado (idempotente por device_id) |
| POST | `/api/v1/mobile/dispositivos/{deviceId}/desregistrar` | autenticado |
| GET | `/api/v1/mobile/configuracao` | autenticado |
| GET | `/api/v1/admin/mobile/dispositivos` | `admin:seed-tenant` |
| POST | `/api/v1/admin/mobile/dispositivos/{id}/revogar` | `admin:seed-tenant` |

## Backend — entidades + handlers W3

- `DispositivoMobile` (entidade) — UNIQUE `(tenant_id, usuario_id, device_id)`
- `PlataformaMobile` enum — Android, Ios, Windows, MacOS
- `BaterPontoMobileCommandHandler` — valida deviceId registrado + ±5min timestamp
  local + hashBatida + foto/biometria; estende W2 com origem=`MobileApp`
- `RegistrarDispositivoCommandHandler` (idempotente)
- `DesregistrarDispositivoCommandHandler`, `RevogarDispositivoCommandHandler`
- `ListarDispositivosQueryHandler` (admin)
- `ObterConfiguracaoQueryHandler` — versão mínima, banners, branding

## Push Notifications

- Interface `INotificacaoPushService` em `Services/V1/Rh/Mobile/Push/`
- Impl atual: `StubNotificacaoPushService` (loga payload)
- Hookado em `AprovarAjusteCommandHandler` — publica para tópico
  `funcionario:{id}` quando ajuste é aprovado
- FCM (Android) + APNs (iOS) real ficam em PRs follow-up `rh-mobile-push-fcm` /
  `rh-mobile-push-apns` — credenciais via `Push:FcmServiceAccountPath` /
  `Push:ApnsCertPath`

## JWT mobile (refresh longo)

`JwtOptions.RefreshTokenDaysMobile = 90` (vs 7 do web). `JwtTokenService.IssueMobile()`
chamado por `LoginMobileCommandHandler` que enriquece UserAgent com
`mobile/{Plataforma}/{DeviceId}`.

## CI/CD

3 workflows GitHub Actions em `.github/workflows/`:
- `mobile-android.yml` — build AAB assinado + publish opcional Play Console internal
- `mobile-ios.yml` — build IPA + upload opcional TestFlight via `xcrun altool`
- `mobile-windows.yml` — build MSIX

`workflow_dispatch` com input `publish=true` no branch `develop` ativa upload.

## Migrations

- `V20260630001_AddTabelaDispositivosMobile`
- `V20260630002_AddColunasMobileMarcacoesPonto` — `prova_biometria_local TEXT`
  + `timestamp_local DATETIME` em `marcacoes_ponto`

## Docs operacionais

- `documentacao/rh/mobile/setup-dev-windows.md`
- `documentacao/rh/mobile/setup-dev-mac.md`
- `documentacao/rh/mobile/distribuicao-android.md`
- `documentacao/rh/mobile/distribuicao-ios.md`
- `documentacao/rh/mobile/troubleshooting-usuario.md`

## Tests

`test/Mobile/Acme.Sistemas.Atena.Mobile.Tests/`:
- `HashHelpersTests` — determinismo + sensibilidade a deviceId + SHA-256 vazio
- `FormatadoresTests` — `MinutosParaHoras`, `MinutosParaHorasComSinal`,
  `FormatarCpf`

10/10 testes verde no último build.

## Arquivos para consultar

- `src/Mobile/Acme.Sistemas.Atena.Mobile/`
- `src/Mobile/Acme.Sistemas.Atena.Mobile.Shared/`
- `test/Mobile/Acme.Sistemas.Atena.Mobile.Tests/`
- `src/Service/Acme.Sistemas.Services/V1/Rh/Mobile/`
- `src/Service/Acme.Sistemas.Services/V1/Autenticacao/Command/LoginMobile/`
- `src/Service/Acme.Sistemas.Services/V1/Rh/Ponto/Marcacao/Command/BaterPontoMobile/`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Mobile/`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Admin/{ListarDispositivos,RevogarDispositivo}Mobile/`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Rh/Ponto/BaterMobile/`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Auth/LoginMobile/`
- `.github/workflows/mobile-*.yml`

## Follow-ups conhecidos

- `rh-mobile-push-fcm` — integração Firebase Admin SDK real
- `rh-mobile-push-apns` — APNs HTTP/2 real
- `rh-mobile-vm-tests` — unit tests dos ViewModels com Refit mock
- `rh-mobile-comprovante-671` — exibir/baixar PDF 671 no app após bater
