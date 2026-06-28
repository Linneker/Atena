## ADDED Requirements

### Requirement: App MAUI cross-plataforma para colaboradores

O sistema SHALL prover app nativo .NET MAUI suportando Android 8+, iOS 14+, Windows 10 22H2+ e macOS 13+, com login email/senha, batida de ponto online/offline com foto ou biometria local, visualização de espelho mensal, e solicitação de ajustes.

#### Scenario: Login e batida online

- **GIVEN** colaborador instalou app, abriu primeira vez
- **WHEN** insere email e senha válidos
- **THEN** recebe access token + refresh token (90 dias)
- **AND** vê tela Home com botão "Bater ponto"
- **WHEN** toca "Bater ponto" com câmera disponível
- **THEN** abre preview de câmera
- **AND** ao confirmar foto, envia `POST /api/v1/rh/ponto/bater-mobile` (multipart)
- **AND** recebe confirmação visual em < 3s (rede normal)

#### Scenario: Batida offline com sync posterior

- **GIVEN** colaborador está sem rede (avião, subsolo)
- **WHEN** toca "Bater ponto"
- **THEN** app captura foto e grava em SQLite local com `status=Pending`
- **AND** mostra confirmação local "Batida registrada — será sincronizada quando houver rede"
- **WHEN** rede volta (Connectivity.ConnectivityChanged)
- **THEN** sync worker envia POST imediatamente
- **AND** atualiza status para `Synced`
- **AND** notifica usuário se houve conflito

#### Scenario: Device sem câmera exige biometria local

- **GIVEN** colaborador instalou app em Windows desktop sem câmera
- **WHEN** toca "Bater ponto"
- **THEN** app detecta ausência de câmera
- **AND** exige biometria local (Windows Hello)
- **AND** ao validar, envia POST com `provaBiometriaLocal` (JWT assinado local) sem foto
- **AND** servidor aceita pois política exige "foto OU prova bio"

### Requirement: Dispositivos registrados com revogação

O sistema SHALL registrar cada device que faz login em `dispositivos_mobile`, com push token, plataforma, modelo, OS version, chave pública local (para validar `provaBiometriaLocal`). Admin tenant SHALL poder revogar dispositivos.

#### Scenario: Registro automático no primeiro login

- **WHEN** colaborador faz login no app pela primeira vez em um device
- **THEN** app gera UUID local (deviceId), gera par de chaves local
- **AND** chama `POST /api/v1/mobile/dispositivos/registrar { deviceId, plataforma, modelo, osVersion, appVersion, pushToken, chavePublicaLocal }`
- **AND** servidor persiste com `ativo=true`

#### Scenario: Admin revoga device

- **WHEN** admin chama `POST /api/v1/admin/mobile/dispositivos/{id}/revogar`
- **THEN** sistema marca `ativo=false`
- **AND** próxima tentativa de bater desse device retorna 403 com mensagem "Device revogado"
- **AND** app limpa tokens locais e redireciona para login

### Requirement: Push notifications via FCM e APNs

O sistema SHALL enviar push notifications via FCM (Android) e APNs (iOS) para os eventos: lembrete de ponto (opt-in), ajuste aprovado/rejeitado, espelho disponível, holerite disponível, comunicado RH.

#### Scenario: Lembrete de bater ponto

- **GIVEN** colaborador configurou lembrete "5 minutos após início da jornada"
- **AND** jornada vigente prevê entrada às 08:00
- **WHEN** são 08:05 e funcionário ainda não bateu
- **THEN** servidor envia push "Lembrete: bata seu ponto"
- **AND** app abre direto na tela de bater quando usuário toca a notificação

### Requirement: Atualização mínima forçada

O sistema SHALL prover endpoint `/api/v1/mobile/configuracao` que retorna versão mínima suportada e atual. App SHALL bloquear uso e exigir atualização quando sua versão for menor que a mínima.

#### Scenario: App desatualizado bloqueia uso

- **GIVEN** app está em versão 1.0.5 e backend declara `minimoSuportado=1.2.0`
- **WHEN** app inicia
- **THEN** app mostra tela bloqueante "Atualização obrigatória — abra a loja"
- **AND** botão abre Play Store / App Store
