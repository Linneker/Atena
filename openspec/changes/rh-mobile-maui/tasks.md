# Tasks — rh-mobile-maui

> 8 fases. App nativo MAUI + endpoints servidor de suporte.

---

## Fase 1 — Setup do projeto MAUI

- [ ] 1.1 Verificar MAUI workload instalado (`dotnet workload install maui`)
- [ ] 1.2 Criar projeto `src/Mobile/Acme.Sistemas.Atena.Mobile.csproj` (net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-windows10.0.19041.0)
- [ ] 1.3 Criar Class Library `Acme.Sistemas.Atena.Mobile.Shared` (referenciada pelo MAUI + UnitTest)
- [ ] 1.4 Criar `Acme.Sistemas.Atena.Mobile.Tests` (xUnit)
- [ ] 1.5 Adicionar 3 projetos à `Atena.sln`
- [ ] 1.6 Configurar `MauiProgram.cs` com DI
- [ ] 1.7 Setup AppShell de navegação
- [ ] 1.8 Configurar AppIcon + SplashScreen genéricos
- [ ] 1.9 Configurar manifests por plataforma (permissões Camera, Internet, Biometric, Geolocation, Notifications)
- [ ] 1.10 Build inicial smoke em todas plataformas (CI matrix)

## Fase 2 — Autenticação + ApiClient

- [ ] 2.1 `IAtenaApi` Refit interface (login, refresh, batida, espelho, ajustes, configuracao, dispositivos)
- [ ] 2.2 `AtenaApiClient` registrado no DI com HttpClient + Polly
- [ ] 2.3 `SecureTokenStore` (SecureStorage wrapper)
- [ ] 2.4 `AuthService.LoginAsync`, `RefreshAsync`, `LogoutAsync`
- [ ] 2.5 Auth handler que injeta Bearer + retry com refresh em 401
- [ ] 2.6 Backend: `RefreshTokenDays` mobile = 90 (atualmente 7) — adicionar config `Jwt:RefreshTokenDaysMobile`
- [ ] 2.7 Backend: endpoint `POST /api/v1/autenticacao/login-mobile` (variante que devolve refresh longo)
- [ ] 2.8 Tela `LoginPage` + `LoginViewModel`
- [ ] 2.9 Tela `PrimeiroAcessoPage` (registra device, testa biometria/câmera)
- [ ] 2.10 Auto-redirect: token válido → Home; senão → Login

## Fase 3 — Bater ponto (núcleo)

- [ ] 3.1 `IBiometriaService` + impls por plataforma
- [ ] 3.2 `ICameraService` + impl com `MediaPicker.CapturePhotoAsync()`
- [ ] 3.3 `IGeoService` com `Geolocation.GetLocationAsync()`
- [ ] 3.4 `DeviceCapabilityHelper` (tem câmera? tem biometria?)
- [ ] 3.5 `IOfflineQueue` + `SqliteOfflineQueue` (sqlite-net-pcl)
- [ ] 3.6 `BaterPontoViewModel` orquestra: detectar capacidade → escolher caminho → enviar
- [ ] 3.7 Tela `HomePage` (saudação + relógio + botão grande + status "online/offline")
- [ ] 3.8 Tela `BaterPontoPage` (preview foto → confirmar)
- [ ] 3.9 Backend: endpoint `POST /api/v1/rh/ponto/bater-mobile` (multipart com foto)
- [ ] 3.10 Backend: persistência da foto em S3/GED
- [ ] 3.11 Backend: validação de `provaBiometriaLocal` (assinatura local com chave do device)
- [ ] 3.12 Backend: validação de `timestampLocal` (± 5min)
- [ ] 3.13 Backend: validação de `hashBatida`
- [ ] 3.14 Sync worker: dispara em resume + connectivity change
- [ ] 3.15 Confirmação visual (animação de check + comprovante)

## Fase 4 — Espelho e ajustes mobile

- [ ] 4.1 `EspelhoMensalViewModel` + Page (calendário scrollable)
- [ ] 4.2 Drill-down: tap em dia → batidas do dia
- [ ] 4.3 Botão "Solicitar ajuste" em uma batida → modal
- [ ] 4.4 `AjustesPageViewModel` lista próprios ajustes + status
- [ ] 4.5 Notificação local quando ajuste é aprovado/rejeitado (via push)

## Fase 5 — Dispositivos e config

- [ ] 5.1 Backend: migration `AddTabelaDispositivosMobile`
- [ ] 5.2 Backend: Domain `DispositivoMobile.cs`
- [ ] 5.3 Backend: Command `RegistrarDispositivoMobile` (5 arquivos)
- [ ] 5.4 Backend: Command `DesregistrarDispositivoMobile`
- [ ] 5.5 Backend: Query `ListarDispositivosDoFuncionario`
- [ ] 5.6 Backend: endpoint admin `GET /api/v1/admin/mobile/dispositivos`
- [ ] 5.7 Backend: endpoint admin `POST /api/v1/admin/mobile/dispositivos/{id}/revogar`
- [ ] 5.8 Backend: Query `ObterConfiguracaoMobile` (versão mínima, banners, jornada, branding)
- [ ] 5.9 Backend: endpoint `GET /api/v1/mobile/configuracao`
- [ ] 5.10 App: tela `ConfiguracoesPage` (perfil, dispositivos vinculados, política de push, logout)
- [ ] 5.11 App: check de versão no boot; força update se necessário

## Fase 6 — Push notifications

- [ ] 6.1 Android: integrar `Plugin.Firebase` ou similar
- [ ] 6.2 Android: receber push token e registrar via endpoint
- [ ] 6.3 iOS: setup APNs (cert + entitlements)
- [ ] 6.4 iOS: receber device token e registrar
- [ ] 6.5 Backend: serviço `INotificacaoPushService` (envia para FCM e APNs)
- [ ] 6.6 Backend: publisher de eventos (ajuste aprovado → push)
- [ ] 6.7 Backend: configurar credenciais FCM/APNs por tenant (futuro) ou globais (MVP)
- [ ] 6.8 App: lembrete configurável "Bater ponto agora" (notificação local + push se app fechado)

## Fase 7 — CI/CD e distribuição

- [ ] 7.1 GitHub Action `mobile-android.yml` (build + sign AAB)
- [ ] 7.2 GitHub Action `mobile-ios.yml` (build + sign IPA)
- [ ] 7.3 GitHub Action `mobile-windows.yml` (build MSIX)
- [ ] 7.4 Secrets: Play Store service account JSON, App Store API key, certs
- [ ] 7.5 Auto-publish em track interno do Play Console em `develop`
- [ ] 7.6 Auto-publish em TestFlight em `develop`
- [ ] 7.7 Auto-publish em produção via release manual com tag

## Fase 8 — Testes e documentação

- [ ] 8.1 Unit tests `Mobile.Shared.Tests` para OfflineQueue, HashHelpers, Formatadores
- [ ] 8.2 Unit tests ViewModels com IApiClient mock (Moq)
- [ ] 8.3 Integration tests dos endpoints `/api/v1/rh/ponto/bater-mobile`, `/api/v1/mobile/*`
- [ ] 8.4 Manual checklist de E2E (login, bater online, bater offline+sync, ver espelho, solicitar ajuste, receber push)
- [ ] 8.5 `documentacao/rh/mobile/setup-dev-mac.md`
- [ ] 8.6 `documentacao/rh/mobile/setup-dev-windows.md`
- [ ] 8.7 `documentacao/rh/mobile/distribuicao-android.md`
- [ ] 8.8 `documentacao/rh/mobile/distribuicao-ios.md`
- [ ] 8.9 `documentacao/rh/mobile/troubleshooting-usuario.md`
- [ ] 8.10 Atualizar `CLAUDE.md` com seção Mobile MAUI
- [ ] 8.11 `openspec validate rh-mobile-maui --strict` válido
