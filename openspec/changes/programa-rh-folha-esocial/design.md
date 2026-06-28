# Design — programa-rh-folha-esocial

## Visão global

```
                                ┌──────────────────────────────┐
                                │  ATENA RH+FOLHA+eSOCIAL+671  │
                                │       ~700-1000 tasks        │
                                │       ~12-18 meses           │
                                └──────────────┬───────────────┘
                                               ▼
                                ┌──────────────────────────────┐
                                │      15 ondas (W1-W15)       │
                                │  cada uma = change OpenSpec  │
                                │     independente e arquivável│
                                └──────────────────────────────┘
```

## Grafo de dependências

```
                                    ┌──────────────┐
                                    │ W1 Fundação  │
                                    └──────┬───────┘
                  ┌────────────────────────┼────────────────────────┐
                  ▼                        ▼                        ▼
          ┌──────────────┐         ┌──────────────┐         ┌──────────────┐
          │W2 Ponto Int. │         │W5 Tab.Legais │         │W11 eSocial   │
          └──────┬───────┘         └──────┬───────┘         │   Fundação   │
                 │                        │                 └──────┬───────┘
        ┌────────┼────────┐               │                        │
        ▼        ▼        ▼               │                        ▼
   ┌─────┐  ┌─────┐  ┌─────────┐          │              ┌──────────────┐
   │W3   │  │W4   │  │W6 Folha │◄─────────┘              │W12 eS Tabelas│
   │MAUI │  │671  │  │ Engine  │                         └──────┬───────┘
   └─────┘  │     │  └────┬────┘                                │
            └─────┘       ▼                                     │
                     ┌─────────┐                                │
                     │W7 CCT   │                                │
                     │ Engine  │                                │
                     └────┬────┘                                │
                  ┌───────┼───────┐                             │
                  ▼       ▼       ▼                             │
              ┌─────┐ ┌─────┐ ┌──────┐                          │
              │W8   │ │W9   │ │W10   │                          │
              │Evtos│ │Resci│ │Bridge│                          │
              └──┬──┘ └──┬──┘ │ Fin  │                          │
                 │       │    └──────┘                          │
                 └───┬───┘                                      │
                     ▼                                          │
                ┌──────────────┐                                │
                │W13 eS Não-Per├◄───────────────────────────────┘
                └──────┬───────┘
                       ▼
                ┌──────────────┐
                │W14 eS Period.│
                └──────┬───────┘
                       ▼
                ┌──────────────┐
                │W15 Relatórios│
                └──────────────┘
```

**Caminho crítico (mais longo):** W1 → W5 → W6 → W7 → W8 (ou W9) → W11 → W12 → W13 → W14 → W15.

## Diretrizes técnicas transversais

Toda onda **deve** seguir:

### D1. Aderência ao Blueprint Acme
- Endpoints em `Api/Endpoints/V1/{Recurso}/{Verbo}{Recurso}/` (4 arquivos).
- Commands/Queries em `Services/V1/{Funcionalidade}/Command|Query/{Acao}/` (5 arquivos).
- Validation com FluentValidation; Behavior no pipeline.
- Repository com `BaseRepository` (filtro tenant automático).
- Domain entity em `Domain/Entities/Rh/` ou `Domain/Entities/Esocial/`.
- Migration `Vyyyymmddxxx_AddTabelaXxx.cs` raw SQL via `IMigration`.

### D2. Permissions
- Novos recursos em `Permissions.cs` (constantes em `Recursos`).
- Acoes específicas quando necessário (`Bater`, `Aprovar`, `Fechar`).
- Role `RH` semeada por padrão em `SeedTenantCommandHandler` (a partir de W1).

### D3. Multi-tenancy
- Toda tabela RH/eSocial carrega `tenant_id`.
- Catálogos legais nacionais (INSS, IRRF, SM, códigos eSocial) são **globais** (não tenant-scoped) — repo direto via `IDataConfiguration`.
- Rubricas (Q2 = por tenant) são tenant-scoped.

### D4. Auditoria
- Toda mutação de ponto/folha/rescisão passa por `AuditBehavior` (já default).
- Eventos críticos (fechamento de folha, transmissão eSocial) gravam `AuditLog` com payload completo.

### D5. Reuso obrigatório de componentes existentes

| Onda | Componente reusado |
|------|---------------------|
| W4 | `XmlSignerC14N`, `CertificadoTenantResolver` (NFe → 671) |
| W11 | `SefazSoapClient`, `ContingenciaPolicy`, `XmlSignerC14N` (NFe → eSocial) |
| W4, W11 | Numeração atômica de NFe (lote) → NSR |
| W2, W3 | `RabbitMQ` + worker (transmissão assíncrona ao 671/eSocial) |
| W3 | Autenticação JWT existente; refresh token; rotas pré-criadas |
| W10 | `ContaPagar` do módulo Financeiro existente |
| W15 | `CrudListComponent` / `CrudFormComponent` do front |

### D6. Testes
- Cobertura mínima 85% nos engines (W2 banco horas, W6 folha, W7 CCT, W11 transmissão).
- Fixtures de folha em `test/fixtures/folha/` (CLT padrão + diferenciais regionais).
- Convenção de Traits obrigatória: `Solucao`, `Acao`, `DisplayName` GWT em PT-BR.
- Integration tests com `SkippableFact + Skip.IfNot(Docker.IsAvailable)`.

## Decisões transversais e tradeoffs

### Mobile = .NET MAUI nativo (Q4)

**Por que:**
- Mesma linguagem (C#) que toda a stack — reuso de DTOs/clientes da API.
- Single codebase Android + iOS + Windows + macOS.
- Reuso de `Acme.Sistemas.Domain` (DTOs, enums, validações) via Class Library.
- Equipe não precisa aprender TypeScript/Swift/Kotlin.

**Custo:**
- Build iOS exige máquina Mac (CI/build server ou pair com dev Mac).
- Apple Developer Program $99/ano.
- Google Play Console $25 único.
- MAUI menos maduro que React Native em produção — risco mitigado por adoção crescente da Microsoft e versão 9 estável.

**Arquitetura proposta:**

```
src/Mobile/
├── Acme.Sistemas.Atena.Mobile/        (csproj MAUI net8.0-android;net8.0-ios;...)
│   ├── App.xaml
│   ├── Platforms/{Android,iOS,Windows,MacCatalyst}/
│   ├── Views/                          (XAML pages: Login, BaterPonto, Espelho, Ajuste)
│   ├── ViewModels/                     (MVVM com CommunityToolkit.Mvvm)
│   ├── Services/                       (ApiClient, AuthStore, BiometriaService, GeoService, CameraService, OfflineQueue)
│   └── Resources/
└── Acme.Sistemas.Atena.Mobile.Shared/  (Class Library; DTOs partilhados com Domain via reuso)
```

Detalhe completo em `W3/design.md`.

### Biometria (Q5)

**Modelo adotado:** *device-side biometric unlock + foto-prova*

```
┌─────────────────────────────────────────────────────────────────┐
│ Fluxo de batida no app mobile                                   │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  1. App detecta capacidade do device                            │
│     ├── tem câmera? → caminho preferencial: FOTO                │
│     └── não tem câmera? → exige biometria local                 │
│                                                                 │
│  2. Caminho FOTO:                                               │
│     ├── BiometricAuth (opcional para destravar app)             │
│     ├── Camera.CaptureAsync() → JPEG                            │
│     └── POST /api/v1/rh/ponto/bater (multipart: foto+gps+ts)    │
│                                                                 │
│  3. Caminho DIGITAL (sem câmera):                               │
│     ├── BiometricAuth obrigatória (TouchID/FaceID/FP Android)   │
│     ├── Token assinado localmente comprovando autenticação      │
│     └── POST /api/v1/rh/ponto/bater (sem foto, com prova bio)  │
│                                                                 │
│  4. Servidor:                                                   │
│     ├── valida JWT do usuário                                   │
│     ├── salva foto em S3/GED (chave: tenant/funcId/ano/mes/ts) │
│     ├── grava MarcacaoPonto + meta (origem, ip, gps, deviceId) │
│     └── retorna comprovante c/ NSR (em W4)                      │
└─────────────────────────────────────────────────────────────────┘
```

**Tradeoff:** Server-side fingerprint matching (templates ISO/IEC 19794-2 + SDKs Innovatrics/Neurotec) ficaria caro e complexo. Confiar na biometria local do device é o padrão de mercado (Ahgora Mobile, Pontomais Mobile, VR Gente).

### Rubricas por tenant (Q2)

```
Catálogo nacional (read-only, semeado por migration):
  rubricas_catalogo_nacional
    codigo | descricao | natureza_esocial | tipo (provento/desconto/informativa)

Customização por tenant:
  rubricas_tenant
    tenant_id | codigo_tenant | descricao | natureza_esocial | tipo
    | formula_expr (DSL ou template) | incidencias (INSS/IRRF/FGTS bits)

Tenant pode:
  ✓ usar rubrica do catálogo como está
  ✓ "clonar" rubrica do catálogo e customizar
  ✓ criar rubrica nova do zero
  ✓ mapear sua rubrica → natureza eSocial S-1010
```

Detalhes do DSL/formula_expr em W5.

### CCT estruturada (Q3) — Wave 7

```
Convencao
  ├── codigo, vigencia_inicio, vigencia_fim
  ├── categoria (sindicato/profissional)
  ├── piso_salarial (por cargo, opcional)
  ├── adicional_he_diurno_pct  (default 50%, CCT pode subir p/ 60-100%)
  ├── adicional_he_noturno_pct
  ├── reajuste_anual_pct
  ├── anuenio_pct, biennio, quinquenio
  ├── auxilios (creche, alimentacao, educacao)
  └── regras_custom : List<RegraCct>

RegraCct (DSL plug-in)
  ├── tipo (gatilho, modificador, calculo_custom)
  ├── condicao (expr)
  ├── acao (expr)

EmpresaCct
  tenant_id | convencao_id | vigencia (override anual)

FuncionarioCct
  tenant_id | funcionario_id | convencao_id | vigencia
```

Sem entrar em PDC/DSL completo aqui — escopo em W7.

### Tabelas legais via upload (Q6)

```
TabelaLegal (genérica, versionada)
  ├── tipo (INSS, IRRF, FGTS, SM, …)
  ├── competencia_inicio (YYYY-MM)
  ├── competencia_fim (YYYY-MM ou null = vigente)
  ├── payload_json (estrutura por tipo)
  └── seed_origem (migration | upload-admin | api)

Endpoint:
  POST /api/v1/admin/rh/tabelas/{tipo}/upload
    multipart: arquivo (JSON ou CSV) + competencia + override?
    permissão: admin:upload-tabelas-legais (exclusiva Root + nova role RhAdmin)

Resolução:
  motor de folha pede "TabelaINSS vigente em 2026-06" → retorna a vigente
```

Detalhe em W5.

## Capability map

| Capability | Origem | Owner |
|------------|--------|-------|
| `rh-cadastros` | W1 | Backend lead |
| `rh-ponto-interno` | W2 | Backend lead |
| `rh-mobile` | W3 | Mobile dev (novo perfil) |
| `rh-ponto-oficial-671` | W4 | Backend + Fiscal |
| `rh-tabelas-legais` | W5 | Backend + Contador consultor |
| `rh-folha` | W6 | Backend + Contador consultor |
| `rh-cct` | W7 | Backend + Jurídico/RH consultor |
| `rh-eventos-mes` | W8 | Backend + RH consultor |
| `rh-rescisao` | W9 | Backend + RH consultor |
| `rh-financeiro-bridge` | W10 | Backend |
| `esocial-transmissao` | W11 | Backend + Fiscal |
| `esocial-tabelas` | W12 | Backend + Fiscal |
| `esocial-nao-periodicos` | W13 | Backend + Fiscal |
| `esocial-periodicos` | W14 | Backend + Fiscal |
| `rh-relatorios` | W15 | Backend + Front |

## Estado da máquina — fechamento de competência

Visão de alto nível do ciclo mensal (detalhado em W6/W14):

```
   ABERTA  ─────────►  EM_CALCULO  ─────────►  CALCULADA  ─────────►  CONFERIDA
     │                       │                     │                       │
     ▼                       ▼                     ▼                       ▼
  bater ponto         folha gera         RH revisa            Aprovador autoriza
  ajustar ponto       proventos/desc     ajusta rubricas
  lançar evento                          re-calcula
                                                                   │
                                                                   ▼
                                                              FECHADA
                                                                   │
                              ┌────────────────────────────────────┼─────────────┐
                              ▼                                    ▼             ▼
                       eSocial S-1200/1210/1299       ContaPagar gerada    Holerite emitido
                                                                   │
                                                                   ▼
                                                              ARQUIVADA
                                                       (não permite mais edição)
```

## Plano de mitigação para o monstro de escopo (R1)

A cada 3 ondas concluídas:
1. Demo executável fim-a-fim ao stakeholder.
2. Re-validar prioridade das próximas 3 (escopo pode mudar pelo aprendizado).
3. Re-estimar esforço restante.
4. Decidir continuar / pausar / pivotar.

Pontos de saída segura (onde o programa pode parar e ainda entregar valor):
- **Após W3** — RH interno básico funcional + mobile. Empresa terceiriza folha.
- **Após W7** — Folha calculada internamente. Empresa não envia eSocial pelo Atena.
- **Após W10** — Folha + financeira integrados. eSocial via sistema externo.
- **Após W14** — Conformidade legal completa. Faltam apenas relatórios analíticos avançados.
