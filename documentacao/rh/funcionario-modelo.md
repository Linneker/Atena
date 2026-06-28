# Modelo de dados do Funcionário (RH/Folha/eSocial)

Documento de referência rápida do modelo expandido pela change `rh-fundacao` (W1).
Cobre as tabelas, FKs, JSON columns, enums, comandos e queries disponíveis.

## Tabelas

### `funcionarios` (estendida)

Mantém colunas anteriores (`cargo`, `departamento` — agora obsoletas) e adiciona:

| Coluna | Tipo | Notas |
|--------|------|-------|
| `cargo_id` | CHAR(36) NULL | FK para `cargos.id` (substitui `cargo` texto livre) |
| `lotacao_id` | CHAR(36) NULL | FK para `lotacoes.id` |
| `departamento_id` | CHAR(36) NULL | FK para `departamentos.id` (substitui `departamento` texto livre) |
| `tipo_contrato` | VARCHAR(40) | `Clt`, `EstagioRemunerado`, `JovemAprendiz`, `TerceirizadoPj`, `AutonomoRpa`, `Cooperado`, `Diretor` |
| `regime_remuneracao` | VARCHAR(30) | `Mensalista`, `Horista`, `Diarista`, `Comissionado` |
| `codigo_matricula` | VARCHAR(30) | único por tenant — `UNIQUE KEY ux_funcionarios_tenant_matricula` |
| `pis` | CHAR(11) | validado por `PisHelper.IsValid` |
| `ctps`, `ctps_serie`, `ctps_uf` | VARCHAR | validado por `CtpsHelper.IsValid` |
| `rg`, `rg_orgao`, `rg_uf` | VARCHAR | |
| `estado_civil`, `naturalidade`, `nacionalidade` | VARCHAR | |
| `endereco_json`, `conta_bancaria_json` | JSON | serializados via `EnderecoFuncionario` e `ContaBancariaFuncionario` |

Índices novos: `ix_funcionarios_cargo`, `ix_funcionarios_departamento`, `ix_funcionarios_lotacao`.

### Tabelas vinculadas

- `cargos` (tenant-scoped, soft delete): código + descrição + CBO + salário base sugerido.
- `lotacoes` (tenant-scoped): endereço da unidade onde o funcionário trabalha (CNPJ opcional).
- `departamentos` (tenant-scoped): hierarquia organizacional + opcional `centro_de_custo_id`.
- `jornadas` (tenant-scoped): perfil de trabalho (`Fixa`, `Escala12x36`, etc.) com `janelas_json`.
- `escalas_funcionario` (vigência temporal): qual jornada cada funcionário cumpre em cada período.
- `historico_salarios` (vigência temporal): histórico salarial — folha lê a vigente em data X.
- `beneficios_catalogo` (tenant-scoped): catálogo de benefícios disponíveis no tenant.
- `beneficios_funcionario` (vigência temporal): vínculos de benefício do funcionário.
- `dependentes`: filhos, cônjuge, etc., para IRRF e salário-família.
- `cbos` (nacional, não tenant-scoped): catálogo CBO opt-in via seed admin.

### Padrão de vigência temporal

Tabelas `historico_salarios`, `beneficios_funcionario`, `escalas_funcionario` usam pares
`vigencia_inicio`/`vigencia_fim`. Convenção:

- `vigencia_fim = NULL` significa "vigente até hoje".
- Ao registrar reajuste, o handler **fecha** a vigência anterior em `D-1` e cria nova linha.
- Query `ListarSalarioVigenteEm(funcionarioId, em)` retorna a linha com `vigencia_inicio <= em <= COALESCE(vigencia_fim, ∞)`.

## Commands

| Command | Recurso | Notas |
|---------|---------|-------|
| `CriarFuncionarioCompleto` | rh-funcionario | Atômico: cria func + salário inicial + escala opcional + benefícios + dependentes |
| `AlterarFuncionarioDados` | rh-funcionario | Apenas dados pessoais (nome, email, endereço, conta) |
| `AlterarFuncionarioContrato` | rh-funcionario | Cargo, lotação, depto, tipo, matrícula, demissão, status |
| `RegistrarReajusteSalarial` | rh-funcionario | Fecha vigência anterior, cria nova; rejeita data anterior à vigente |
| `VincularBeneficioAoFuncionario` | rh-funcionario | Conflict se já há benefício vigente |
| `RemoverBeneficioDoFuncionario` | rh-funcionario | Soft delete |
| `CadastrarDependente` | rh-dependente | Filho/Cônjuge/Pais/Outro |
| `RemoverDependente` | rh-dependente | Soft delete |
| `AtribuirEscalaAoFuncionario` | rh-funcionario | Fecha escala anterior, cria nova |

## Queries

| Query | Retorno |
|-------|---------|
| `ObterFichaCompletaFuncionario(id)` | DadosPessoais + Contrato + SalarioVigente + HistoricoSalarial + Beneficios + Dependentes + Escalas |
| `ListarSalarioVigenteEm(funcId, em)` | Linha de `historico_salarios` vigente; usada pela engine de folha (W6) |

## Validadores brasileiros

Todos em `Acme.Sistemas.Core/Helper/`:

- `CpfHelper.IsValid` — DV mod 11
- `PisHelper.IsValid` — DV mod 11 com pesos 3,2,9,8,7,6,5,4,3,2
- `CtpsHelper.IsValid(numero, serie, uf)` — formato + UF brasileira
- `ContaBancariaHelper.IsValid` — banco (3 dígitos) + agência (3-5 dígitos + DV opcional) + conta (4-12 dígitos + DV 1 caractere)

Validação CBO: regex `^\d{6}$` no command + opcional verificação em `cbos.codigo` via `ICboRepository.GetByCodigoAsync`.

## Endpoints (resumo)

- `GET/POST/PUT/DELETE /api/v1/rh/jornadas[/{id}]`
- `GET/POST/PUT/DELETE /api/v1/rh/cargos[/{id}]`
- `GET/POST/PUT/DELETE /api/v1/rh/lotacoes[/{id}]`
- `GET/POST/PUT/DELETE /api/v1/rh/departamentos[/{id}]`
- `GET/POST/PUT/DELETE /api/v1/rh/beneficios/catalogo[/{id}]`
- `GET /api/v1/rh/cbos`
- `POST /api/v1/admin/rh/cbos/seed` (Root only)
- `POST /api/v1/rh/funcionarios` (criação completa, transacional)
- `PUT /api/v1/rh/funcionarios/{id}/dados`
- `PUT /api/v1/rh/funcionarios/{id}/contrato`
- `POST /api/v1/rh/funcionarios/{id}/salarios` (reajuste)
- `POST/DELETE /api/v1/rh/funcionarios/{id}/beneficios[/{vinculoId}]`
- `POST/DELETE /api/v1/rh/funcionarios/{id}/dependentes[/{depId}]`
- `POST /api/v1/rh/funcionarios/{id}/escalas`
- `GET /api/v1/rh/funcionarios/{id}/ficha`
- `GET /api/v1/rh/funcionarios/{id}/salario-vigente?em=YYYY-MM-DD`

Permissões em `Permissions.Recursos`: `Rh`, `RhFuncionario`, `RhJornada`, `RhCargo`, `RhLotacao`,
`RhDepartamento`, `RhBeneficio`, `RhDependente`. Ações: `Ler`, `Criar`, `Editar`, `Excluir`, `GerirEquipe`.
