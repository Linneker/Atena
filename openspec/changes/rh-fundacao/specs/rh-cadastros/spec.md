## ADDED Requirements

### Requirement: Funcionário com vínculo obrigatório a Usuário

O sistema SHALL exigir que toda entidade `Funcionario` ativa esteja vinculada a uma entidade `Usuario` (relação 1:1). A coluna `usuario_id` SHALL ser obrigatória após a migração desta onda. Funcionários históricos (já no banco) SHALL receber automaticamente um `Usuario` desativado durante a migração de upgrade.

#### Scenario: Criar funcionário novo sem usuário falha

- **WHEN** chamada `POST /api/v1/rh/funcionarios` com body sem `usuarioId`
- **THEN** a API retorna 400 com mensagem `usuarioId é obrigatório — vincule a um usuário existente ou crie um junto`
- **AND** nenhuma linha é gravada em `funcionarios`

#### Scenario: Wizard cria usuário automaticamente

- **WHEN** wizard de "Novo funcionário" submete dados pessoais com flag `criarUsuarioAutomaticamente=true` e email
- **THEN** a API cria `Usuario` ativo com role `Funcionario` (sem permissões administrativas) e senha temporária
- **AND** vincula esse usuário ao funcionário criado
- **AND** envia e-mail de boas-vindas com link de definição de senha

#### Scenario: Migração de funcionário legado cria usuário desativado

- **GIVEN** banco tem 50 funcionários ativos sem `usuario_id`
- **WHEN** migração `CriarUsuariosDesativadosParaFuncionariosLegados` roda
- **THEN** 50 entidades `Usuario` são criadas com `status=Desativado`, login `matricula@<tenant>.local`, e vinculadas
- **AND** relatório `documentacao/rh/relatorio-migracao-<timestamp>.md` lista todos os auto-criados para ação manual do admin

### Requirement: Jornada de trabalho estruturada

O sistema SHALL modelar jornadas de trabalho como entidade com tipo (Fixa, Escala 12x36, Escala 6x1, Livre, Estagio, JovemAprendiz), carga semanal, e janelas semanais detalhadas (dia da semana × entrada × pausas × saída). Cada `Funcionario` SHALL ter ao menos uma `EscalaFuncionario` vigente apontando para uma `Jornada`.

#### Scenario: Tenant novo recebe jornada padrão 44h CLT

- **GIVEN** seed-tenant cria tenant novo
- **WHEN** consulta `GET /api/v1/rh/jornadas`
- **THEN** retorna ao menos 1 jornada `{ nome: "44h CLT", tipo: "Fixa", cargaSemanal: 44 }`

#### Scenario: Jornada 12x36 com tolerância personalizada

- **WHEN** RH cria jornada `{ nome: "12x36 enfermagem", tipo: "Escala12x36", cargaSemanal: 42, toleranciaMinutos: 15, janelasJson: [...] }`
- **THEN** sistema persiste com sucesso e valida estrutura do JSON de janelas
- **AND** janelas inválidas (entrada > saída, sobreposição) retornam 400 com erro estruturado

### Requirement: Cargo com CBO opcional

O sistema SHALL modelar `Cargo` como entidade tenant-scoped com código interno, descrição, CBO (Classificação Brasileira de Ocupações) opcional de 6 dígitos, e salário-base sugerido. O catálogo CBO oficial SHALL ser opt-in via endpoint admin.

#### Scenario: CBO inválido é rejeitado

- **WHEN** cria cargo com `codigoCbo: "999999"` (não existe na tabela CBO)
- **GIVEN** catálogo CBO foi semeado neste tenant
- **THEN** API retorna 400 com mensagem `codigoCbo inexistente`

#### Scenario: CBO ausente é permitido (catálogo opt-in)

- **WHEN** cria cargo sem `codigoCbo`
- **THEN** sistema aceita e marca `codigo_cbo = NULL`
- **AND** dispara warning em log `Cargo sem CBO — obrigatório para eSocial`

### Requirement: Histórico de salário com vigência

O sistema SHALL manter histórico completo de salários de cada funcionário em `historico_salarios`, com cada registro tendo `vigencia_inicio` obrigatória, `vigencia_fim` opcional (NULL = vigente), `valor`, e `motivo`. Apenas uma linha por funcionário SHALL ter `vigencia_fim = NULL` em qualquer momento.

#### Scenario: Registrar reajuste fecha vigência anterior

- **GIVEN** funcionário tem salário vigente { valor: 3000, vigenciaInicio: 2025-01-01, vigenciaFim: NULL }
- **WHEN** RH executa `POST /api/v1/rh/funcionarios/{id}/reajuste { valor: 3500, vigenciaInicio: 2026-06-01, motivo: "Dissidio" }`
- **THEN** linha anterior recebe `vigenciaFim = 2026-05-31`
- **AND** nova linha é criada com `vigenciaInicio = 2026-06-01, vigenciaFim = NULL`

#### Scenario: Consultar salário vigente em data específica

- **WHEN** folha solicita `GET /api/v1/rh/funcionarios/{id}/salario-vigente?em=2026-03-15`
- **THEN** sistema retorna o valor cuja vigência inclui essa data

### Requirement: Benefícios catalogados por tenant

O sistema SHALL permitir cada tenant catalogar seus próprios benefícios (VT, VR, VA, plano-saúde, etc.) em `beneficios_catalogo`, e atribuí-los a funcionários com valor e vigência específicos em `beneficios_funcionario`.

#### Scenario: Funcionário recebe VT com desconto legal

- **WHEN** RH atribui benefício VT a funcionário com `descontoFuncionarioPct: 6`
- **THEN** sistema persiste e folha (W6) descontará até 6% do salário do funcionário

### Requirement: Dependentes para IRRF e salário-família

O sistema SHALL modelar dependentes do funcionário em `dependentes` com tipo, CPF, data de nascimento, e flags `irrf` e `salario_familia` indicando se o dependente conta para dedução IRRF e/ou salário-família INSS.

#### Scenario: Cadastrar filho menor de 14 anos

- **WHEN** funcionário cadastra dependente `{ tipo: "Filho", dataNascimento: "2018-05-10", irrf: true, salarioFamilia: true }`
- **THEN** sistema persiste com sucesso
- **AND** folha (W6) considerará dedução IRRF e salário-família

### Requirement: Role RH semeada por padrão em novo tenant

O sistema SHALL semear automaticamente a role `RH` em todo tenant criado via `seed-tenant`, contendo permissões `rh:*` exceto `rh-funcionario:bater-ponto-outros`.

#### Scenario: Tenant novo possui role RH

- **GIVEN** seed-tenant criou tenant
- **WHEN** consulta `GET /api/v1/seguranca/roles`
- **THEN** lista inclui role `RH` com nome traduzido `Recursos Humanos`

### Requirement: Endpoints RH seguem blueprint Acme

Todas as novas rotas `/api/v1/rh/*` SHALL ser implementadas seguindo o padrão de um endpoint por pasta com 4 arquivos (Endpoint, Request, Response, Map), validado em runtime por `EndpointConventionTests`.

#### Scenario: PR com endpoint não-aderente é reprovado

- **WHEN** PR cria endpoint em arquivo plural `RhFuncionariosEndpoints.cs`
- **THEN** `EndpointConventionTests` falha localmente e em CI
