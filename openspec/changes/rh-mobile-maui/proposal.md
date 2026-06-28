## Why

W3 do programa `programa-rh-folha-esocial`. Decisão Q4 = **.NET MAUI nativo**. Esta onda introduz o **primeiro projeto mobile** no Atena: app cross-plataforma Android/iOS/Windows/macOS para colaboradores baterem ponto, verem espelho, solicitarem ajustes e receberem notificações.

Esta é uma onda **transversal**: dela em diante, todo módulo RH com vertente "colaborador" expõe sua superfície tanto na web quanto no app.

## What Changes

### Solução .NET — novos projetos

```
src/Mobile/
├── Acme.Sistemas.Atena.Mobile/                  (csproj MAUI .NET 8/9)
│   TargetFrameworks: net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-windows10.0.19041.0
├── Acme.Sistemas.Atena.Mobile.Shared/           (Class Library — DTOs e serviços compartilhados)
└── Acme.Sistemas.Atena.Mobile.Tests/            (testes do código compartilhado)
```

### MAUI app — estrutura

```
Acme.Sistemas.Atena.Mobile/
├── App.xaml, App.xaml.cs                        (root + DI)
├── AppShell.xaml                                (shell navigation)
├── MauiProgram.cs                               (DI registration)
├── Platforms/
│   ├── Android/  (MainActivity, AndroidManifest, FCM service, biometric impl)
│   ├── iOS/      (AppDelegate, Info.plist, APNs, FaceID/TouchID impl)
│   ├── Windows/  (App.xaml.cs, package.appxmanifest)
│   └── MacCatalyst/
├── Views/
│   ├── LoginPage.xaml
│   ├── HomePage.xaml                            (botão grande "Bater ponto" + relógio)
│   ├── BaterPontoPage.xaml                      (preview da câmera + confirmar)
│   ├── EspelhoMensalPage.xaml                   (calendário)
│   ├── AjustesPage.xaml                         (listar próprios + solicitar)
│   ├── HoleritePage.xaml                        (W6 vai habilitar)
│   ├── ConfiguracoesPage.xaml
│   └── PrimeiroAcessoPage.xaml                  (testa biometria/câmera, configura)
├── ViewModels/                                  (MVVM com CommunityToolkit.Mvvm)
│   ├── LoginViewModel.cs
│   ├── HomeViewModel.cs
│   ├── BaterPontoViewModel.cs
│   ├── EspelhoMensalViewModel.cs
│   └── ...
├── Services/
│   ├── IAtenaApiClient.cs / AtenaApiClient.cs   (HTTP client tipado, Refit)
│   ├── ITokenStore.cs / SecureTokenStore.cs     (SecureStorage MAUI)
│   ├── IAuthService.cs / AuthService.cs         (login + refresh)
│   ├── IBiometriaService.cs / BiometriaService.cs (BiometricAuthentication.Maui)
│   ├── ICameraService.cs / CameraService.cs     (MediaPicker + Camera.MAUI)
│   ├── IGeoService.cs / GeoService.cs           (Geolocation MAUI)
│   ├── IOfflineQueue.cs / SqliteOfflineQueue.cs (SQLite local + sync)
│   ├── IPushService.cs / FcmPushService.cs / ApnsPushService.cs
│   └── INotificationService.cs                  (alerts in-app)
├── Resources/
│   ├── AppIcon, SplashScreen
│   ├── Fonts (Roboto, Inter)
│   ├── Images
│   └── Strings (pt-BR primeiro, en-US later)
└── Helpers/
    ├── ConnectivityHelper.cs
    └── DeviceCapabilityHelper.cs                (detecta câmera, biometria)
```

### Backend — endpoints novos para suportar mobile

- `POST /api/v1/mobile/dispositivos/registrar` — registra device (deviceId, modelo, plataforma, push token FCM/APNs)
- `POST /api/v1/mobile/dispositivos/{deviceId}/desregistrar`
- `POST /api/v1/mobile/configuracao` — endpoint que entrega config dinâmica (políticas vigentes, jornada do funcionário, jornada de outras pessoas da equipe, mensagens-banner, etc.) — chamado no boot do app
- `POST /api/v1/rh/ponto/bater-mobile` — variante do `bater` que aceita multipart com `foto` (binário) + `gps` + `deviceId` + `provaBiometriaLocal` (JWT assinado pelo app provando que biometria local foi validada)
- `GET /api/v1/mobile/atualizacoes-pendentes?desde=` — sync delta para offline queue
- WebSocket / SSE opcional para push em tempo real quando app estiver aberto

### Autenticação mobile

- Login com email/senha → recebe `accessToken (15min) + refreshToken (90d para mobile, mais longo que web)`.
- Refresh transparente em background.
- `SecureStorage` (Keychain iOS / Keystore Android) para guardar tokens.
- Biometria local **opcional para destravar app** (não substitui senha de login).
- Logout limpa tokens e dispositivo registrado.

### Fluxo de batida mobile (Q5 — biometria + foto)

```
1. App boot → detecta capacidade (câmera? biometria?)
2. Usuário toca "Bater ponto"
3. App decide caminho:
   a) Tem câmera → abre preview, captura foto → POST
   b) Tem câmera mas usuário negou permissão → mostra erro + opção biometria local
   c) Não tem câmera (kiosk Windows? device sem câmera?) → exige biometria local
4. Antes do POST:
   ├── valida conectividade
   ├── coleta GPS (não bloqueia se negado, registra null)
   ├── se offline → grava em SQLite local + marca para sync
   └── se online → envia direto
5. POST /api/v1/rh/ponto/bater-mobile (multipart):
   ├── foto (JPEG, ~200KB target)
   ├── tipo (opcional, servidor infere)
   ├── gps (lat, lng, accuracy)
   ├── deviceId (do dispositivo registrado)
   ├── provaBiometriaLocal (opcional — JWT autoassinado pelo app
   │                       quando user passou biometria)
   ├── timestampLocal (para detectar relógio adulterado)
   └── hashBatida (SHA-256 dos campos)
6. Servidor:
   ├── valida JWT, deviceId registrado, tenant
   ├── persiste foto em S3 (chave: tenant/funcId/ponto/AAAAMM/timestamp.jpg)
   ├── cria MarcacaoPonto (hash chain igual W2)
   ├── grava metadata (gps, prova bio, deviceId) em colunas adicionais
   └── retorna comprovante (em W4 vira o comprovante NSR assinado)
```

### Offline-first

- SQLite local guarda batidas/ajustes feitos offline.
- Sync worker em background (a cada 1min com WiFi, a cada 10min com 4G).
- Conflito (servidor rejeitou): notifica usuário com motivo.

### Push notifications

- FCM (Android) + APNs (iOS) configurados.
- Casos de push:
  - Lembrete de bater ponto (5min após início/fim esperado da jornada — opt-in).
  - Ajuste aprovado/rejeitado.
  - Espelho mensal disponível.
  - Holerite disponível (W6).
  - Comunicado RH.

### Distribuição

- **Interno (sprint review / homologação)**: AppCenter ou GitHub Releases.
- **Produção Android**: Google Play Console (track interno → fechado → produção).
- **Produção iOS**: App Store Connect (TestFlight → produção).
- **Windows**: MSIX via Microsoft Store ou distribuído direto pelo cliente.
- **macOS**: notarized via Apple Developer.

### CI/CD

- GitHub Actions:
  - Build Android (ubuntu-latest + Android SDK)
  - Build iOS (macos-latest + Xcode)
  - Build Windows (windows-latest + MAUI workload)
  - Sign + publish em homologação a cada merge em `develop`
  - Tag → publish em produção

## Capabilities

### New Capabilities
- `rh-mobile` — App nativo MAUI para colaboradores (Android/iOS/Windows/macOS).

### Modified Capabilities
- `rh-ponto-interno` — endpoint `bater-mobile` (multipart com foto + biometria-local-prova).

## Out of Scope

- App para **gestor** (gestor usa web). App mobile é só colaborador.
- Reconhecimento facial **server-side** com matching de templates (apenas foto-prova armazenada).
- Apple Watch / Wear OS companions.
- Pagamentos in-app, compras, etc.
- Modo "kiosk" físico (caixa/totem) — caso especial em W4 (REP-C).
- White-label per-tenant agora (todos veem "Atena Mobile" no MVP; branding em onda futura).

## Risks

- **R1**: MAUI menos maduro especialmente em iOS — bugs de UI/build podem aparecer. Mitigação: snapshot tests, beta interno antes de público, plano B = congelar features no Windows+Android se iOS travar.
- **R2**: Apple Developer ($99/ano) e Google Play ($25) — custo do cliente. Documentar no onboarding e no checklist de pre-W3.
- **R3**: Build iOS exige Mac. Mitigação: usar GitHub macOS runners, ou MacInCloud.
- **R4**: SecureStorage Android < API 23 não tem Keystore (raro hoje, mas device antigo). Mitigação: minSdk 23 (Android 6.0+).
- **R5**: SQLite local cresce com offline prolongado. Mitigação: limpeza após sync confirmado + alerta acima de 50MB local.
- **R6**: Battery drain do GPS contínuo. Mitigação: GPS só na batida, não em background.
- **R7**: Mudança de fuso horário em viagem altera relógio do device. Mitigação: enviar `timestampLocal` ao servidor + servidor compara com `now`; divergência > 5min vira flag de revisão.

## Success Criteria

- App roda em Android 8+, iOS 14+, Windows 10 22H2+, macOS 13+ (declarados como mínimos).
- Login → bater ponto → ver espelho em fluxo de < 30s para usuário novo.
- Offline: bater ponto sem rede → entra fila local → sync ao restabelecer.
- Push notifications recebidas em < 30s em rede normal.
- Build Android < 5min em CI; iOS < 15min.
- Cobertura de testes do código compartilhado (`Mobile.Shared`) ≥ 85%.
- Publicado em Google Play interno + TestFlight interno.
- `openspec validate rh-mobile-maui --strict` válido.
