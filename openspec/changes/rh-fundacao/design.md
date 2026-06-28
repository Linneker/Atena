# Design — rh-fundacao

## Modelo de dados (visão)

```
                       ┌──────────────────────┐
                       │      Funcionario     │
                       │ (estendido)          │
                       └──┬─────┬─────────────┘
            ┌─────────────┘     └─────────────┐
            ▼                                 ▼
      ┌──────────┐                      ┌──────────┐
      │ Usuario  │                      │  Cargo   │─┐
      │(obrig.)  │                      │  + CBO   │ │
      └──────────┘                      └──────────┘ │
                                                     ▼
            ┌──────────┐    ┌─────────────────┐  ┌──────────┐
            │ Lotação  │    │ HistoricoSalario│  │Departam. │
            │(estabel.)│    │ (1:N c/vigência)│  └──────────┘
            └────┬─────┘    └─────────────────┘
                 │
                 ▼
            ┌──────────┐
            │ Empresa  │ (já existe)
            └──────────┘

      ┌──────────┐     ┌──────────────────────┐
      │ Jornada  │◄────┤  EscalaFuncionario   │────► Funcionario
      └──────────┘     │  (Func x Jornada     │
                       │   c/ vigência)       │
                       └──────────────────────┘

      Funcionario ──┬──► BeneficioFuncionario ──► BeneficioCatalogo
                    └──► Dependente
```

## Tabelas principais

### `jornadas`
```sql
CREATE TABLE jornadas (
  id CHAR(36) PRIMARY KEY,
  tenant_id CHAR(36) NOT NULL,
  nome VARCHAR(80) NOT NULL,                   -- "44h CLT", "12x36 diurno", "Estagiário 6h"
  tipo ENUM('Fixa','Escala12x36','Escala6x1','Escala5x2','Livre','Estagio','JovemAprendiz') NOT NULL,
  carga_semanal_horas DECIMAL(5,2) NOT NULL,   -- 44.00, 36.00, 20.00
  carga_diaria_horas DECIMAL(5,2),
  janelas_json JSON NOT NULL,                  -- [{ dia:'seg', entrada:'08:00', saidaAlmoco:'12:00', voltaAlmoco:'13:30', saida:'17:30' }, ...]
  permite_marcar_intervalo BOOLEAN NOT NULL DEFAULT TRUE,
  tolerancia_minutos INT NOT NULL DEFAULT 10,  -- antes/depois da janela
  created_at, updated_at, tenant_id_idx
);
```

### `cargos`
```sql
CREATE TABLE cargos (
  id, tenant_id,
  codigo VARCHAR(20),                          -- código interno (ex: "DEV-SR")
  descricao VARCHAR(200) NOT NULL,
  codigo_cbo CHAR(6),                          -- "212405" (Dev de sistemas)
  salario_base_sugerido DECIMAL(10,2),
  ativo BOOLEAN NOT NULL DEFAULT TRUE
);
```

### `lotacoes`
```sql
CREATE TABLE lotacoes (
  id, tenant_id,
  nome VARCHAR(120) NOT NULL,                  -- "Matriz - SP", "Filial - Belo Horizonte"
  empresa_id CHAR(36) NOT NULL,                -- FK empresas
  cnpj CHAR(14),                               -- se a lotação é uma filial com CNPJ próprio
  endereco_json JSON,                          -- {rua, num, cep, ...}
  ativo BOOLEAN
);
```

### `historico_salarios`
```sql
CREATE TABLE historico_salarios (
  id, tenant_id, funcionario_id,
  valor DECIMAL(10,2) NOT NULL,
  vigencia_inicio DATE NOT NULL,
  vigencia_fim DATE,                           -- null = vigente
  motivo ENUM('Admissao','Reajuste','Promocao','Dissidio','Merito','Acordo','Outro') NOT NULL,
  observacao TEXT,
  registrado_por_usuario_id CHAR(36),
  registrado_at DATETIME
);
```

### `beneficios_catalogo` (por tenant)
```sql
CREATE TABLE beneficios_catalogo (
  id, tenant_id,
  codigo VARCHAR(20),                          -- "VT", "VR", "VA", "PS"
  descricao VARCHAR(120),
  tipo ENUM('ValeTransporte','ValeRefeicao','ValeAlimentacao','PlanoSaude',
            'PlanoOdonto','AuxilioCreche','SeguroVida','Outro'),
  desconto_funcionario_pct DECIMAL(5,2),       -- VT: até 6% do salário base por lei
  custo_empresa_padrao DECIMAL(10,2),
  natureza_rubrica_esocial VARCHAR(20),        -- mapeamento futuro
  ativo BOOLEAN
);
```

### `beneficios_funcionario`
```sql
CREATE TABLE beneficios_funcionario (
  id, tenant_id, funcionario_id, beneficio_catalogo_id,
  valor DECIMAL(10,2),                         -- override do custo padrão
  vigencia_inicio DATE, vigencia_fim DATE,
  observacao TEXT
);
```

### `dependentes`
```sql
CREATE TABLE dependentes (
  id, tenant_id, funcionario_id,
  nome_completo VARCHAR(200),
  cpf CHAR(11),
  data_nascimento DATE,
  tipo ENUM('Filho','Enteado','Conjuge','Companheiro','PaiOuMae','Outro'),
  irrf BOOLEAN NOT NULL DEFAULT FALSE,        -- conta para dedução IRRF?
  salario_familia BOOLEAN NOT NULL DEFAULT FALSE,  -- conta para salário-família INSS?
  pensao_alimenticia_pct DECIMAL(5,2),
  data_inicio DATE, data_fim DATE
);
```

### `escalas_funcionario`
```sql
CREATE TABLE escalas_funcionario (
  id, tenant_id, funcionario_id, jornada_id,
  vigencia_inicio DATE NOT NULL,
  vigencia_fim DATE                            -- null = vigente
);
```

### Migração da entidade `Funcionario`

```sql
ALTER TABLE funcionarios
  ADD COLUMN cargo_id CHAR(36) NULL AFTER cargo,
  ADD COLUMN lotacao_id CHAR(36) NULL,
  ADD COLUMN departamento_id CHAR(36) NULL,
  ADD COLUMN tipo_contrato VARCHAR(40),
  ADD COLUMN regime_remuneracao VARCHAR(30),
  ADD COLUMN codigo_matricula VARCHAR(30),
  ADD COLUMN pis CHAR(11),
  ADD COLUMN ctps VARCHAR(20), ADD COLUMN ctps_serie VARCHAR(10), ADD COLUMN ctps_uf CHAR(2),
  ADD COLUMN rg VARCHAR(20), ADD COLUMN rg_orgao VARCHAR(20), ADD COLUMN rg_uf CHAR(2),
  ADD COLUMN estado_civil VARCHAR(20),
  ADD COLUMN naturalidade VARCHAR(80),
  ADD COLUMN nacionalidade VARCHAR(40) DEFAULT 'Brasileira',
  ADD COLUMN endereco_json JSON,
  ADD COLUMN conta_bancaria_json JSON,
  ADD UNIQUE KEY uk_matricula (tenant_id, codigo_matricula);
```

Etapa 2 da migration (post-deploy):
1. Para cada `funcionarios.cargo` texto, achar ou criar `cargos.descricao` correspondente, popular `cargo_id`.
2. Idem para `departamento`.
3. Para cada tenant, garantir lotação "Sede" e atribuir como default.
4. Para cada funcionário ativo sem `usuario_id`, criar `Usuario` desativado.
5. `cargo` (texto) e `departamento` (texto) ficam como NULL e marcados obsoletos.

## Permissions atualizadas

```csharp
// Acme.Sistemas.Core/Const/Permissions.cs

public static class Recursos
{
    // ... existentes ...
    public const string Rh = "rh";
    public const string RhFuncionario = "rh-funcionario";
    public const string RhJornada = "rh-jornada";
    public const string RhCargo = "rh-cargo";
    public const string RhLotacao = "rh-lotacao";
    public const string RhBeneficio = "rh-beneficio";
    public const string RhDependente = "rh-dependente";
    public const string RhDepartamento = "rh-departamento";
}

public static class Acoes
{
    // ... existentes ...
    public const string GerirEquipe = "gerir-equipe";
}
```

`SeedTenantCommandHandler` ganha:
```csharp
// nova role
await CriarRoleAsync("RH", new[]
{
    Permissions.Of(Recursos.Rh, Acoes.Listar),
    Permissions.Of(Recursos.RhFuncionario, Acoes.All()),
    Permissions.Of(Recursos.RhJornada, Acoes.All()),
    Permissions.Of(Recursos.RhCargo, Acoes.All()),
    Permissions.Of(Recursos.RhLotacao, Acoes.All()),
    Permissions.Of(Recursos.RhBeneficio, Acoes.All()),
    Permissions.Of(Recursos.RhDependente, Acoes.All()),
    Permissions.Of(Recursos.RhDepartamento, Acoes.All()),
    // outras permissões adicionadas por waves futuras
});

// jornada padrão
await CriarJornadaAsync("44h CLT", TipoJornada.Fixa, cargaSemanal: 44m, janelas: PadraoCltJanelasSemanais());
await CriarCargoAsync("Não classificado", codigoCbo: null);
await CriarDepartamentoAsync("Não classificado");
await CriarLotacaoAsync("Sede", empresaId: empresaDemo.Id);
```

## Frontend

```
site/atena-web/src/app/features/rh/
├── rh.routes.ts                  → /rh/* lazy-loaded
├── funcionarios/
│   ├── funcionario-list.component.ts
│   ├── funcionario-form.component.ts   (wizard 4 passos)
│   └── funcionario-form-pessoal-step.component.ts
│   └── funcionario-form-contrato-step.component.ts
│   └── funcionario-form-salario-step.component.ts
│   └── funcionario-form-beneficios-step.component.ts
├── jornadas/
├── cargos/
├── lotacoes/
├── departamentos/
├── beneficios/
└── rh.services.ts
```

Menu lateral:
- Antes: o card "Cadastros" tinha "Funcionários".
- Depois: card "RH" novo, e "Funcionários" sai de Cadastros (redirect mantido por 1 onda).

## Tradeoffs e decisões

### Por que `UsuarioId` obrigatório?

Sem login, funcionário não bate ponto, não vê holerite, não solicita ajuste. Manter `UsuarioId?` simplificaria a migration mas complicaria todas as ondas seguintes (cada query teria que tratar o caso de funcionário sem usuário).

**Decisão**: obrigatório, com migration que auto-cria usuário desativado para os existentes.

### Por que `endereco_json` JSON em vez de tabela separada?

Endereço de funcionário não é entidade compartilhada (cada funcionário tem o seu) e raramente é consultado por filtros estruturados (filtro por bairro? raro). JSON é mais barato e flexível para mudanças.

**Decisão**: JSON com schema validado em código.

### CBO — opt-in?

Tabela CBO oficial tem ~2600 entradas. Seguindo o padrão de `seed-tenant-fiscal-br`:
- Subset curado (~100 CBOs mais comuns) inline na migration.
- Tabela completa via `POST /api/v1/admin/seed-cbo` (opt-in).

### Histórico de salário — entidade separada?

**Sim.** Folha precisa do salário vigente em CADA dia do mês (não só hoje), 13º precisa da média anual, rescisão precisa do último valor. Sem entidade de histórico isso não funciona.

### Que tela substitui o cadastro atual de funcionário?

A tela atual (`features/cadastros/funcionarios/`) fica como **redirect** para `/rh/funcionarios/{id}` por 2 ondas (W1 e W2). Em W3 é removida.

## Test strategy

- **Unit**: serializadores de `endereco_json` e `janelas_json`; cálculo de DV do PIS; validação de CBO.
- **Integration**: criar funcionário completo via API → assertar 4 tabelas populadas.
- **Migration**: rodar migration em DB com 100 funcionários legados → assertar nenhum dado perdido + 100 usuários desativados criados + cargos/departamentos consolidados.
- **Convention**: `EndpointConventionTests` passa para todas as novas rotas `/api/v1/rh/*`.

## Documentação

- `documentacao/rh/funcionario-modelo.md` — descrição completa do modelo.
- `documentacao/rh/migracao-funcionario-legado.md` — guia para admins de tenants existentes (relatório dos usuários auto-criados e como ativá-los).
- `documentacao/seeds/cbo.json` — placeholder + README de como popular.
