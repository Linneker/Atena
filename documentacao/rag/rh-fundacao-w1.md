# RH Fundação (W1)

## Propósito

Primeira onda do programa **rh-folha-esocial**. Estabelece o modelo de dados de
RH: Funcionário enriquecido + Cargo + Lotação + Departamento + Jornada +
Benefícios + Dependentes + tabela de referência CBO (Classificação Brasileira
de Ocupações). Sem ela, o W2 (ponto), W3 (mobile) e W4 (671) não existem.

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `Cargo` | `Domain/Entities/Rh/Cargo.cs` | Nome, descrição, CBO, faixa salarial |
| `Lotacao` | `Domain/Entities/Rh/Lotacao.cs` | Local físico de trabalho |
| `Departamento` | `Domain/Entities/Rh/Departamento.cs` | Hierarquia opcional (parent) |
| `Jornada` | `Domain/Entities/Rh/Jornada.cs` | Definição base; usada por `EscalaFuncionario` |
| `EscalaFuncionario` | `Domain/Entities/Rh/EscalaFuncionario.cs` | Jornada vigente do funcionário |
| `HistoricoSalario` | `Domain/Entities/Rh/HistoricoSalario.cs` | Trilha de salário com motivo (Admissão, Reajuste, Promoção, etc.) |
| `BeneficioCatalogo` | `Domain/Entities/Rh/BeneficioCatalogo.cs` | VR, VA, plano saúde, etc. — catálogo do tenant |
| `BeneficioFuncionario` | `Domain/Entities/Rh/BeneficioFuncionario.cs` | Vínculo funcionário × benefício |
| `Dependente` | `Domain/Entities/Rh/Dependente.cs` | Cônjuge, filhos para IR + plano saúde |
| `Cbo` | `Domain/Entities/Rh/Cbo.cs` | Referência nacional (seed estático) |
| `Funcionario` (estendido) | `Domain/Entities/Cadastros/Funcionario.cs` | + CargoId, LotacaoId, DepartamentoId, PIS, CTPS, RG, Endereço (JSON), Conta bancária (JSON) |

## Enums

- `TipoContrato`: CLT, Pj, Autonomo, EstagioRemunerado, Aprendiz, Estatutario, Temporario
- `RegimeRemuneracao`: Mensalista, Horista, Comissionado, Misto
- `MotivoSalario`: Admissao, Reajuste, Promocao, MudancaCargo, Equiparacao
- `EstadoCivil`, `Sexo`, `RacaCor` (padrão eSocial)

## Validadores (Core/Helper)

- `PisHelper.Validar(string pis)` — algoritmo DV PIS/PASEP (11 dígitos)
- `CtpsHelper.Validar(numero, uf)` — número + UF emissora
- `ContaBancariaHelper` — código de banco + agência + conta DV
- `CboHelper.Existe(codigo)` — consulta CBO seedado

## Endpoints REST

| Método | Rota | Permissão |
|--------|------|-----------|
| GET/POST/PUT/DEL | `/api/v1/rh/funcionarios` | `rh-funcionario:*` |
| GET/POST/PUT/DEL | `/api/v1/rh/cargos` | `rh-cargo:*` |
| GET/POST/PUT/DEL | `/api/v1/rh/lotacoes` | `rh-lotacao:*` |
| GET/POST/PUT/DEL | `/api/v1/rh/departamentos` | `rh-departamento:*` |
| GET/POST/PUT/DEL | `/api/v1/rh/jornadas` | `rh-jornada:*` |
| GET/POST/PUT/DEL | `/api/v1/rh/beneficios/catalogo` | `rh-beneficio:*` |
| GET/POST/PUT/DEL | `/api/v1/rh/beneficios/funcionarios` | `rh-beneficio:*` |
| GET/POST/PUT/DEL | `/api/v1/rh/dependentes` | `rh-dependente:*` |
| GET | `/api/v1/rh/cbo` | autenticado |

## Frontend

- `site/atena-web/src/app/features/rh/funcionarios/` — wizard 4 passos para
  cadastro novo + ficha completa em abas (Dados pessoais, Profissionais,
  Endereço/Conta, Benefícios/Dependentes).
- `site/atena-web/src/app/features/rh/{cargos,lotacoes,departamentos,jornadas,beneficios}/`
  — CRUDs simples via `CrudListComponent` / `CrudFormComponent`.

## Decisões

- **Endereço JSON** em `Funcionario` (ao invés de FK para `enderecos`) —
  histórico simples + mudança não afeta dependentes.
- **ContaBancariaJson** idem — RH troca conta com frequência, histórico fica no
  log.
- **Domain por área ERP** (Rh, Fiscal, Financeiro, Cadastros, etc.) — decisão
  deliberada do projeto Atena, **não** divergência do blueprint Acme.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Rh/` (sem `Oficial671`, sem `Mobile` específicos)
- `src/Service/Acme.Sistemas.Domain/Enums/Rh/`
- `src/Service/Acme.Sistemas.Core/Helper/{Pis,Ctps,ContaBancaria,Cbo}Helper.cs`
- `src/Service/Acme.Sistemas.Services/V1/Rh/` (sem subpastas Ponto/Mobile/Oficial671)
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Rh/` (sem Ponto/Mobile/Oficial671)
- `site/atena-web/src/app/features/rh/`
- `documentacao/rh/funcionario-modelo.md`
- Migrations `V20260628*` (tabelas RH fundação)

## Follow-ups conhecidos

- Documentos digitalizados do funcionário (RG, contrato) — via GED.
- Cálculo automático de tempo de serviço.
- Plano de carreira (cargos com pré-requisitos).
