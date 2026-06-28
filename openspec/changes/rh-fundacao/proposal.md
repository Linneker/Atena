## Why

Pré-requisito de tudo no programa `programa-rh-folha-esocial`. Antes de bater ponto ou calcular folha, o Atena precisa entender **quem é o funcionário em profundidade** — não apenas seu cadastro CRUD básico.

Hoje a entidade `Funcionario` (`src/Service/Acme.Sistemas.Domain/Entities/Cadastros/Funcionario.cs`) tem 9 campos enxutos. RH/Folha precisa de:
- Jornada de trabalho estruturada (escala semanal, intervalos, turno).
- Cargo e CBO (Classificação Brasileira de Ocupações) — exigido pelo eSocial.
- Salário-base com vigência (precisa de histórico para 13º, férias proporcionais, rescisão).
- Benefícios (vale-transporte, vale-refeição, plano de saúde, auxílio-creche).
- Dependentes para IRRF (filhos, cônjuge — afetam cálculo do IR).
- Vínculo obrigatório com `Usuario` (todo funcionário do RH precisa logar para bater ponto e ver holerite).
- Tipo de contrato (CLT, estágio, jovem aprendiz, terceirizado, PJ, autônomo), data de admissão, tipo de regime (mensalista, horista, comissionista).
- Lotação (estabelecimento físico) — exigido pelo eSocial.
- Centro de custo (já existe, mas vai virar tenant-scoped por cargo).

Sem essas estruturas, nem ponto, nem folha, nem eSocial decolam.

## What Changes

### Backend — novas entidades em `Domain/Entities/Rh/`

- `Jornada` — modelo de escala semanal (dias da semana × janelas de horário × intervalos), tipo (fixa, escala 12x36, escala 6x1, livre).
- `EscalaFuncionario` — vínculo `Funcionario ↔ Jornada` com vigência.
- `Cargo` — código, descrição, CBO (`codigo_cbo` 6 dígitos), salário-base sugerido.
- `Lotacao` — estabelecimento físico (com CNPJ próprio se filial), endereço, vínculo opcional a `Empresa`.
- `Departamento` (promove o campo texto existente para entidade).
- `HistoricoSalario` — Funcionario × valor × data início × data fim (null = vigente) × motivo (admissão, reajuste, promoção, dissídio, mérito).
- `BeneficioCatalogo` — definição do benefício no tenant (VT, VR, VA, plano-saude, creche, etc.).
- `BeneficioFuncionario` — vínculo Funcionario × Beneficio × valor × vigência.
- `Dependente` — Funcionario × tipo (filho, cônjuge, enteado, etc.) × CPF × data nascimento × IRRF (sim/não) × pensão alimentícia.
- `TipoContrato` (enum: CLT, EstagioRemunerado, JovemAprendiz, TerceirizadoPj, AutonomoRpa).
- `RegimeRemuneracao` (enum: Mensalista, Horista, Comissionista, Misto).

### Backend — campos novos em `Funcionario`

- `UsuarioId` passa de `Guid?` para `Guid` **obrigatório** (com migration que cria usuário automaticamente para funcionários existentes ativos).
- `CargoId` (substitui campo texto `Cargo`).
- `LotacaoId`.
- `DepartamentoId` (substitui campo texto `Departamento`).
- `TipoContrato`, `RegimeRemuneracao`.
- `CodigoMatricula` (string, único por tenant — usado em eSocial e holerite).
- `Pis` (PIS/PASEP/NIT — 11 dígitos, exigido eSocial).
- `Ctps` (número CTPS), `CtpsSerie`, `CtpsUf`.
- `Rg`, `RgOrgaoEmissor`, `RgUf`.
- `EstadoCivil` (enum), `Naturalidade`, `Nacionalidade` (default Brasileira).
- `EnderecoCompleto` (rua, número, bairro, CEP, cidade, UF, complemento) — usa ViaCEP existente.
- `ContaBancaria` (banco, agência, conta, tipo conta) para pagamento.

### Permissions — novo recurso `Recursos.Rh`

Adicionar em `Acme.Sistemas.Core/Const/Permissions.cs`:
- `Recursos.Rh = "rh"` (root do módulo).
- `Recursos.RhFuncionario, RhJornada, RhCargo, RhLotacao, RhBeneficio, RhDependente`.
- Acoes específicas (além das CRUD padrão): `Acoes.GerirEquipe` (gestor vê só sua equipe).

### Role default `RH`

`SeedTenantCommandHandler` passa a semear role `RH` com permissões sobre todo `rh:*` (exceto `bater-ponto-outros`).

### Frontend — nova área `site/atena-web/src/app/features/rh/`

- Rota raiz `/rh` protegida por `permissaoGuard('rh:listar')`.
- Submenus: `Funcionários`, `Cargos`, `Jornadas`, `Lotações`, `Benefícios`, `Departamentos` (CRUDs).
- Tela de "Ficha do funcionário" com abas: Dados, Contrato, Salário, Benefícios, Dependentes, Documentos.
- Reusa `CrudListComponent` e `CrudFormComponent` do shared.

### Migration estratégica

- **NÃO quebrar** dados existentes. Funcionário sem `Cargo`/`Departamento` texto vira `Cargo`/`Departamento` "Não classificado" automaticamente, sem perda.
- Funcionário ativo sem `UsuarioId` ganha um usuário desativado autocriado com login = `matricula@<tenant-domain>.local` (admin do tenant deve editar e ativar).
- Campos antigos (`Cargo` string, `Departamento` string) marcados como obsoletos por 2 ondas e removidos em W3.

## Capabilities

### New Capabilities

- `rh-cadastros` — Cadastros estendidos de pessoas: jornada, cargo, lotação, benefícios, dependentes, salário com histórico.

### Modified Capabilities

- `seed-tenant-administrativo` — passa a semear role `RH`, cargo "Não classificado", departamento "Não classificado", lotação "Sede" automaticamente.
- `multi-tenancy` — novas tabelas RH passam pelo `BaseRepository`.

## Out of Scope

- Marcação de ponto (W2).
- Cálculo de folha (W6).
- CCT (W7).
- Documentos de admissão escaneados (vira W8 ou wave posterior — só metadado por ora).
- Foto do funcionário (sobe em W3 junto com mobile).
- Importação em lote de funcionários via CSV/Excel — change posterior.
- ATS/Recrutamento.

## Risks

- **R1**: Tornar `UsuarioId` obrigatório quebra tenants existentes com funcionários sem login.
  - **Mitigação**: migration de upgrade cria `Usuario` desativado para todo `Funcionario` ativo sem vínculo. Admin do tenant recebe relatório dos auto-criados após migration.
- **R2**: Cadastros pesados (15+ campos novos em Funcionario) tornam tela de criação assustadora.
  - **Mitigação**: wizard de 4 passos no frontend (Pessoal, Contrato, Salário, Benefícios) com possibilidade de salvar parcial.
- **R3**: Validações de PIS, CTPS, CBO precisam de tabelas oficiais.
  - **Mitigação**: PIS = algoritmo de dígito verificador (sem tabela). CBO = tabela ~2600 entradas → seed opt-in via `documentacao/seeds/cbo.json` (formato similar a CFOP no `seed-tenant-fiscal-br`).
- **R4**: Mudança em `Funcionario` impacta CrudFormComponent existente em `features/cadastros/funcionarios/`.
  - **Mitigação**: nova tela de Funcionário em `features/rh/funcionarios/` substitui a antiga; antiga vira redirect.

## Success Criteria

- Migration roda em CI e em DB existente sem perda de dados (testes).
- Tenant novo (via `seed-tenant`) já vem com role `RH`, cargo/dept/lotação default e jornada padrão "44h CLT" pré-cadastrada.
- Ficha completa de funcionário CLT pode ser criada via API e front em < 3 minutos.
- 100% das ~120 rotas /api/v1 mantém aderência ao blueprint (validado por `EndpointConventionTests`).
- Convenções de testes (`Trait Solucao`, `Trait Acao`, `DisplayName` GWT) seguidas em 100% dos testes novos.
- Specs aprovadas via `openspec validate rh-fundacao --strict`.
