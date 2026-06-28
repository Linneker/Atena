# Distribuição Android — Atena Mobile

## Application ID
`br.com.acme.atena.mobile`

## Secrets exigidos no GitHub Actions

| Secret | Conteúdo |
|--------|----------|
| `ANDROID_KEYSTORE_BASE64` | Keystore JKS/PKCS12 do app, base64-encoded |
| `ANDROID_KEYSTORE_PASSWORD` | Senha do keystore |
| `ANDROID_KEY_ALIAS` | Alias da chave (ex.: `atena-release`) |
| `ANDROID_KEY_PASSWORD` | Senha da chave |
| `PLAY_SERVICE_ACCOUNT_JSON` | JSON da service account do Google Play Console com `Manage releases` |

## Gerar AAB localmente

```powershell
dotnet publish src/Mobile/Acme.Sistemas.Atena.Mobile `
  -f net10.0-android -c Release `
  -p:AndroidPackageFormat=aab `
  -p:AndroidKeyStore=true `
  -p:AndroidSigningKeyStore=atena.keystore `
  -p:AndroidSigningStorePass=$env:STORE_PASS `
  -p:AndroidSigningKeyAlias=atena-release `
  -p:AndroidSigningKeyPass=$env:KEY_PASS
```

O AAB sai em `bin/Release/net10.0-android/publish/*.aab`.

## Publicar no Play Console

Os workflows `.github/workflows/mobile-android.yml` aceitam input
`publish=true` (apenas branch `develop`) que envia o AAB para a track
**internal**. Promoção para `alpha` → `beta` → `production` é manual via
Play Console.
