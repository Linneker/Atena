# Distribuição iOS — Atena Mobile

## Bundle ID
`br.com.acme.atena.mobile`

## Secrets exigidos no GitHub Actions

| Secret | Conteúdo |
|--------|----------|
| `IOS_CERT_P12_BASE64` | Certificado de distribuição (Apple Distribution) `.p12` em base64 |
| `IOS_CERT_PASSWORD` | Senha do .p12 |
| `IOS_PROVISIONING_PROFILE_BASE64` | Profile `.mobileprovision` em base64 |
| `APP_STORE_API_KEY_ID` | Key ID da App Store Connect API |
| `APP_STORE_ISSUER_ID` | Issuer ID |
| `APP_STORE_API_PRIVATE_KEY` | Conteúdo do .p8 |

## Gerar IPA localmente (macOS)

```bash
dotnet publish src/Mobile/Acme.Sistemas.Atena.Mobile \
  -f net10.0-ios -c Release \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:CodesignKey="Apple Distribution: ACME (TEAM12345)" \
  -p:CodesignProvision="Atena Mobile Distribution"
```

O IPA sai em `bin/Release/net10.0-ios/ios-arm64/publish/Atena.ipa`.

## Subir para TestFlight

```bash
xcrun altool --upload-app --type ios -f Atena.ipa \
  --apiKey "$APP_STORE_API_KEY_ID" --apiIssuer "$APP_STORE_ISSUER_ID"
```

Promoção TestFlight → produção é feita pelo App Store Connect.
