# Design — rh-mobile-maui

## Arquitetura do app

```
┌──────────────────────────────────────────────────────────────┐
│                         MAUI App                             │
├──────────────────────────────────────────────────────────────┤
│  Views (XAML)  ──MVVM──►  ViewModels  ──►  Services         │
│                                                  │           │
│                                                  ▼           │
│                                          ┌──────────────┐    │
│                                          │  HttpClient  │    │
│                                          │ + Polly retry│    │
│                                          │ + JWT bearer │    │
│                                          └──────┬───────┘    │
│                                                 │            │
│                            HTTPS+TLS1.2+        ▼            │
└─────────────────────────────────────────────────┼────────────┘
                                                  │
                              ┌───────────────────┼─────────────────┐
                              ▼                                     ▼
                  ┌────────────────────────┐         ┌──────────────────────┐
                  │   Acme.Sistemas.Atena. │         │  FCM (Android) /     │
                  │        Api             │         │  APNs (iOS)          │
                  │   /api/v1/...          │         │   ─ token push       │
                  └────────────────────────┘         └──────────────────────┘
```

## Decisões técnicas

### Por que MAUI e não Flutter/React Native/Capacitor?

| Critério | MAUI | RN | Flutter | Capacitor (PWA+) |
|----------|:----:|:--:|:------:|:----------------:|
| Mesma linguagem da stack (C#) | ✓ | ✗ | ✗ | parcial |
| Reuso de DTOs/Validation | ✓ | ✗ | ✗ | parcial |
| Single codebase 4 plataformas | ✓ | ✓ (3) | ✓ (5) | ✓ |
| Performance nativa | ✓ | parcial | ✓ | menor |
| Skill da equipe atual | ✓ | – | – | – |
| Maturidade ecossistema | médio | alto | alto | alto |
| Suporte Microsoft | ✓ longo prazo | – | – | – |
| Custo de aprendizado | baixo | alto | médio | baixo |

MAUI vence por skill + reuso, aceitando o custo de maturidade.

### Reuso máximo via `Mobile.Shared`

```csharp
// Acme.Sistemas.Atena.Mobile.Shared (Class Library .net 8)
//  ◄── Acme.Sistemas.Domain (DTOs públicos, enums, validações)
//  Sem dependência de Infrastructure/Repository/Api

// Apenas API contracts: Request, Response, Result records
// Validators (FluentValidation) reaproveitáveis
// Helpers de formatação (CPF, CNPJ, datas)
```

### HTTP client

- **Refit** para client tipado: `IAtenaApi { Task<LoginResult> LoginAsync(LoginRequest); ... }`.
- **Polly** para retry exponencial em falhas de rede.
- Interceptor injeta `Authorization: Bearer <jwt>` automaticamente.
- Refresh transparente: ao receber 401, tenta refresh; se também falhar, redireciona pra login.

### SecureStorage cross-platform

`Microsoft.Maui.Storage.SecureStorage`:
- Android: Keystore + EncryptedSharedPreferences.
- iOS: Keychain.
- Windows: PasswordVault.
- macOS: Keychain.

```csharp
await SecureStorage.SetAsync("accessToken", token);
var token = await SecureStorage.GetAsync("accessToken");
```

### Biometria local

`Plugin.Maui.Biometric` (open source) ou implementação manual por plataforma:
- Android: `BiometricPrompt`.
- iOS: `LocalAuthentication.LAContext`.
- Windows: `Windows.Security.Credentials.UI.UserConsentVerifier`.
- macOS: `LAContext` (mesmo iOS).

**Modelo**: biometria é só "unlock" do app, NÃO substitui senha. Sucesso na biometria emite JWT local assinado com chave por-device (gerada na primeira instalação, guardada em SecureStorage) — esse JWT vai no `provaBiometriaLocal` da batida. Servidor decide se aceita.

### Câmera

`Microsoft.Maui.Media.MediaPicker.CapturePhotoAsync()` — funciona out-of-box em todas plataformas.

Para preview live (importante para qualidade do enquadramento), usar `Camera.MAUI` (Nuget terceiro, MIT) ou implementar handler nativo. Decisão: começar com `MediaPicker` (built-in), upgrade para Camera.MAUI em iteração 2.

### Offline-first com SQLite

```csharp
// Tabela local: pending_marcacoes
//   id (UUID local), payload (JSON), foto_path (file local), criado_em, status (Pending/Synced/Failed), tentativas

OfflineQueueService:
  EnqueueMarcacao(dto, fotoBytes) → grava local + agenda sync
  SyncAsync():
    foreach pending:
      try { POST /bater-mobile; on success → status=Synced; on conflict → status=Failed (notifica) }
  Limpeza periódica: status=Synced há > 30 dias → remove
```

Sync triggers:
- App resume → sync imediato.
- Conectividade restabelecida (Connectivity.ConnectivityChanged) → sync.
- Timer background (15min com WiFi, 30min com cell).

### Push notifications

**Android (FCM)**:
- `Plugin.Firebase` ou `Xamarin.Firebase.Messaging`.
- google-services.json configurado.
- Receiver registra token em `/api/v1/mobile/dispositivos/registrar`.

**iOS (APNs)**:
- Certificado push do Apple Developer.
- Provisioning profile.
- Service registra token devolvido.

**Windows (WNS)**: planejado para v2.

**macOS**: usa APNs igual iOS.

### Endpoint `bater-mobile` (servidor)

```csharp
// Acme.Sistemas.Atena.Api/Endpoints/V1/Rh/Ponto/BaterMobile/

[POST] /api/v1/rh/ponto/bater-mobile  (multipart/form-data)

Aceita:
  ├── form fields: tipo (opcional), gps, deviceId, timestampLocal, hashBatida, provaBiometriaLocal
  └── file: foto (binário JPEG)

Validações:
  ├── JWT válido e tem permission rh-ponto:bater
  ├── deviceId existe e está vinculado ao funcionário do JWT (sem revogação)
  ├── timestampLocal dentro de ± 5min do `now` servidor
  ├── foto OU provaBiometriaLocal obrigatório (pelo menos um)
  └── hashBatida confere

Side effects:
  ├── upload foto S3 (tenant/funcId/ponto/AAAAMM/uuid.jpg)
  ├── cria MarcacaoPonto (igual ao bater web, mas com origem=MobileApp + foto_url)
  ├── audita
  └── retorna comprovante { marcacaoId, dataHora, comprovante (opcional, W4) }
```

### Dispositivos cadastrados

```sql
CREATE TABLE dispositivos_mobile (
  id, tenant_id, funcionario_id, usuario_id,
  device_id VARCHAR(120),                     -- ID estável do device
  plataforma ENUM('Android','iOS','Windows','MacOS'),
  modelo VARCHAR(120),
  os_version VARCHAR(40),
  app_version VARCHAR(20),
  push_token VARCHAR(500),                    -- FCM/APNs
  chave_publica_local TEXT,                   -- para validar provaBiometriaLocal
  ativo BOOLEAN,
  registrado_em DATETIME,
  ultimo_acesso DATETIME
);
```

Funcionário pode ter múltiplos dispositivos (celular pessoal + tablet corporativo). Admin pode revogar via web.

### Versionamento e atualização forçada

Endpoint `/api/v1/mobile/configuracao` retorna:
```json
{
  "minimoSuportado": "1.0.0",
  "atual": "1.2.3",
  "obrigatorioAtualizar": false,
  "linkAndroid": "https://play.google.com/...",
  "linkIos": "https://apps.apple.com/..."
}
```

Se `app.versao < minimoSuportado` → app bloqueia uso e exige update.

### Cores e identidade

- Reusa CSS custom properties do tenant (já existem no `TenantBrandingService` web) via endpoint `/api/v1/mobile/branding`.
- Splash screen e ícone do app por enquanto neutros "Atena" — branding por-tenant é futuro.

### Internacionalização

- `Resources/Strings/AppResources.pt-BR.resx` (default).
- Estrutura preparada para `en-US`, `es-ES` em ondas futuras.

## CI/CD pipeline

```yaml
# .github/workflows/mobile-android.yml (esboço)
on: [push]
jobs:
  build-android:
    runs-on: ubuntu-latest
    steps:
      - checkout
      - setup .NET 8
      - install MAUI workload (android)
      - dotnet workload restore
      - dotnet publish -f net8.0-android -c Release
      - upload AAB to Play Console (apenas em branch main)
```

iOS exige `macos-latest` + Xcode + provisioning profile no secrets.

## Test strategy

- **Mobile.Shared.Tests**: unit tests dos serviços puros (offline queue logic, hash helpers, formatadores).
- **MAUI ViewModels Tests**: unit com mocks de IApiClient (CommunityToolkit.Mvvm é testável).
- **UI tests** (opcional, deferir para iteração 2): MAUI tem Appium support; começar manual.
- **End-to-end manual**: checklist por release.

## Documentação

- `documentacao/rh/mobile/setup-dev-mac.md`
- `documentacao/rh/mobile/setup-dev-windows.md`
- `documentacao/rh/mobile/distribuicao-android.md`
- `documentacao/rh/mobile/distribuicao-ios.md`
- `documentacao/rh/mobile/troubleshooting-usuario.md`
