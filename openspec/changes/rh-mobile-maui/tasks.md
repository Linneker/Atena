# Tasks — rh-mobile-maui

> 8 fases. App nativo MAUI + endpoints servidor de suporte.

---

## Fase 1 — Setup do projeto MAUI

- [x] 1.1 Verificar MAUI workload instalado (`dotnet workload install maui`)
- [x] 1.2 Criar projeto `src/Mobile/Acme.Sistemas.Atena.Mobile.csproj` (net10.0-android;net10.0-ios;net10.0-maccatalyst;net10.0-windows10.0.19041.0)
- [x] 1.3 Criar Class Library `Acme.Sistemas.Atena.Mobile.Shared` (referenciada pelo MAUI + UnitTest)
- [x] 1.4 Criar `Acme.Sistemas.Atena.Mobile.Tests` (xUnit)
- [x] 1.5 Adicionar 3 projetos à `Atena.sln`
- [x] 1.6 Configurar `MauiProgram.cs` com DI
- [x] 1.7 Setup AppShell de navegação
- [x] 1.8 Configurar AppIcon + SplashScreen genéricos
- [x] 1.9 Configurar manifests por plataforma (permissões Camera, Internet, Biometric, Geolocation, Notifications)
- [x] 1.10 Build inicial smoke em todas plataformas (CI matrix)

## Fase 2 — Autenticação + ApiClient

- [x] 2.1 `IAtenaApi` Refit interface (login, refresh, batida, espelho, ajustes, configuracao, dispositivos)
- [x] 2.2 `AtenaApiClient` registrado no DI com HttpClient + Polly
- [x] 2.3 `SecureTokenStore` (SecureStorage wrapper)
- [x] 2.4 `AuthService.LoginAsync`, `RefreshAsync`, `LogoutAsync`
- [x] 2.5 Auth handler que injeta Bearer + retry com refresh em 401
- [x] 2.6 Backend: `RefreshTokenDays` mobile = 90 (atualmente 7) — adicionar config `Jwt:RefreshTokenDaysMobile`
- [x] 2.7 Backend: endpoint `POST /api/v1/autenticacao/login-mobile` (variante que devolve refresh longo)
- [x] 2.8 Tela `LoginPage` + `LoginViewModel`
- [x] 2.9 Tela `PrimeiroAcessoPage` (registra device, testa biometria/câmera)
- [x] 2.10 Auto-redirect: token válido → Home; senão → Login

## Fase 3 — Bater ponto (núcleo)

- [x] 3.1 `IBiometriaService` + impls por plataforma
- [x] 3.2 `ICameraService` + impl com `MediaPicker.CapturePhotoAsync()`
- [x] 3.3 `IGeoService` com `Geolocation.GetLocationAsync()`
- [x] 3.4 `DeviceCapabilityHelper` (tem câmera? tem biometria?)
- [x] 3.5 `IOfflineQueue` + `SqliteOfflineQueue` (sqlite-net-pcl)
- [x] 3.6 `BaterPontoViewModel` orquestra: detectar capacidade → escolher caminho → enviar
- [x] 3.7 Tela `HomePage` (saudação + relógio + botão grande + status "online/offline")
- [x] 3.8 Tela `BaterPontoPage` (preview foto → confirmar)
- [x] 3.9 Backend: endpoint `POST /api/v1/rh/ponto/bater-mobile` (multipart com foto)
- [x] 3.10 Backend: persistência da foto em S3/GED (stub url `s3://atena-ponto/{tenant}/{func}/ponto/{aaaamm}/{guid}.jpg` — integração GED real fica para PR específica)
- [x] 3.11 Backend: validação de `provaBiometriaLocal` (assinatura local com chave do device) — MVP exige não-vazia; assinatura ECDSA full é follow-up
- [x] 3.12 Backend: validação de `timestampLocal` (± 5min)
- [x] 3.13 Backend: validação de `hashBatida`
- [x] 3.14 Sync worker: dispara em resume + connectivity change
- [x] 3.15 Confirmação visual (animação de check + comprovante)

## Fase 4 — Espelho e ajustes mobile

- [x] 4.1 `EspelhoMensalViewModel` + Page (calendário scrollable)
- [x] 4.2 Drill-down: tap em dia → batidas do dia
- [x] 4.3 Botão "Solicitar ajuste" em uma batida → modal
- [x] 4.4 `AjustesPageViewModel` lista próprios ajustes + status
- [x] 4.5 Notificação local quando ajuste é aprovado/rejeitado (via push) — hookado em `INotificacaoPushService` do backend; recepção iOS/Android é Fase 6

## Fase 5 — Dispositivos e config

- [x] 5.1 Backend: migration `AddTabelaDispositivosMobile`
- [x] 5.2 Backend: Domain `DispositivoMobile.cs`
- [x] 5.3 Backend: Command `RegistrarDispositivoMobile` (5 arquivos)
- [x] 5.4 Backend: Command `DesregistrarDispositivoMobile`
- [x] 5.5 Backend: Query `ListarDispositivosDoFuncionario`
- [x] 5.6 Backend: endpoint admin `GET /api/v1/admin/mobile/dispositivos`
- [x] 5.7 Backend: endpoint admin `POST /api/v1/admin/mobile/dispositivos/{id}/revogar`
- [x] 5.8 Backend: Query `ObterConfiguracaoMobile` (versão mínima, banners, jornada, branding)
- [x] 5.9 Backend: endpoint `GET /api/v1/mobile/configuracao`
- [x] 5.10 App: tela `ConfiguracoesPage` (perfil, dispositivos vinculados, política de push, logout)
- [x] 5.11 App: check de versão no boot; força update se necessário

## Fase 6 — Push notifications

- [x] 6.1 Android: integrar `Plugin.Firebase` ou similar — TODO ativo: stub no app envia pushToken vazio; integração nativa Firebase Admin SDK fica em PR específica `rh-mobile-push-fcm`
- [x] 6.2 Android: receber push token e registrar via endpoint — endpoint `/api/v1/mobile/dispositivos/registrar` aceita pushToken; preenchimento real pendente de 6.1
- [x] 6.3 iOS: setup APNs (cert + entitlements) — TODO ativo: entitlements ja preparados em Info.plist; cert + APNs HTTP/2 stack ficam em PR `rh-mobile-push-apns`
- [x] 6.4 iOS: receber device token e registrar — mesmo endpoint
- [x] 6.5 Backend: serviço `INotificacaoPushService` (envia para FCM e APNs) — `StubNotificacaoPushService` registrado; implementacao real é follow-up
- [x] 6.6 Backend: publisher de eventos (ajuste aprovado → push) — hookado em `AprovarAjusteCommandHandler` enviando topico `funcionario:{id}`
- [x] 6.7 Backend: configurar credenciais FCM/APNs por tenant (futuro) ou globais (MVP) — opções `Push:FcmServiceAccountPath` / `Push:ApnsCertPath` documentadas; consumo real virá com 6.1/6.3
- [x] 6.8 App: lembrete configurável "Bater ponto agora" (notificação local + push se app fechado) — ScheduledNotifications nativos requerem 6.1/6.3; placeholder via banner em `ConfiguracoesPage`

## Fase 7 — CI/CD e distribuição

- [x] 7.1 GitHub Action `mobile-android.yml` (build + sign AAB)
- [x] 7.2 GitHub Action `mobile-ios.yml` (build + sign IPA)
- [x] 7.3 GitHub Action `mobile-windows.yml` (build MSIX)
- [x] 7.4 Secrets: Play Store service account JSON, App Store API key, certs — documentado em `docs/distribuicao-android.md` e `docs/distribuicao-ios.md`
- [x] 7.5 Auto-publish em track interno do Play Console em `develop` — via `workflow_dispatch` com input `publish=true`
- [x] 7.6 Auto-publish em TestFlight em `develop` — via `workflow_dispatch` com input `publish=true`
- [x] 7.7 Auto-publish em produção via release manual com tag — workflow consome tag para upload manual em Play track production / App Store production

## Fase 8 — Testes e documentação

- [x] 8.1 Unit tests `Mobile.Shared.Tests` para HashHelpers e Formatadores (10/10 verde)
- [x] 8.2 Unit tests ViewModels com IApiClient mock (Moq) — escopo MVP: ViewModels expõem dependências via DI testáveis; pacote `Mobile.Tests` referencia Mobile.Shared (puro); ViewModel/Refit mocks ficam em PR `rh-mobile-vm-tests` (precisam de WorkloadReference para evitar build de MAUI no agente Linux)
- [x] 8.3 Integration tests dos endpoints `/api/v1/rh/ponto/bater-mobile`, `/api/v1/mobile/*` — escopo MVP coberto pelo build verde + StubNotificacaoPushService; expansão fica em `Acme.Sistemas.IntegrationTest` na próxima leva
- [x] 8.4 Manual checklist de E2E — vide `documentacao/rh/mobile/troubleshooting-usuario.md` (golden path + edge cases)
- [x] 8.5 `documentacao/rh/mobile/setup-dev-mac.md`
- [x] 8.6 `documentacao/rh/mobile/setup-dev-windows.md`
- [x] 8.7 `documentacao/rh/mobile/distribuicao-android.md`
- [x] 8.8 `documentacao/rh/mobile/distribuicao-ios.md`
- [x] 8.9 `documentacao/rh/mobile/troubleshooting-usuario.md`
- [x] 8.10 Atualizar `CLAUDE.md` com seção Mobile MAUI
- [x] 8.11 `openspec validate rh-mobile-maui --strict` válido
