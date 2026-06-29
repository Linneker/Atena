# Cadastros

## Propósito

Entidades base do ERP — Empresa, Cliente, Fornecedor, Funcionário, Produto.
São referenciadas por todas as outras áreas (financeiro fala de fornecedor,
estoque de produto, vendas de cliente).

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `Empresa` | `Domain/Entities/Cadastros/Empresa.cs` | RazaoSocial, CNPJ, IE, IM, `Endereco`, `Status`, **`UsaRepOficial`** (W4) |
| `Cliente` | `Domain/Entities/Cadastros/Cliente.cs` | PF/PJ, CPF/CNPJ, contato, endereço |
| `Fornecedor` | `Domain/Entities/Cadastros/Fornecedor.cs` | PF/PJ, condições de pagamento padrão |
| `Funcionario` | `Domain/Entities/Cadastros/Funcionario.cs` | NomeCompleto, CPF, vínculo `UsuarioId`, `CargoId`, `LotacaoId`, `DepartamentoId`, PIS/CTPS/RG, Endereço/Conta JSON (W1) |
| `Produto` | `Domain/Entities/Cadastros/Produto.cs` | Código, descrição, NCM, unidade, preço, tipo (mercadoria/serviço) |
| `Endereco` (VO) | `Domain/Entities/Cadastros/Endereco.cs` | CEP, logradouro, número, complemento, bairro, cidade, UF, país |

## Repositórios

| Interface | Impl | Métodos não-base |
|-----------|------|------------------|
| `IEmpresaRepository` | `EmpresaRepository` | `GetByCnpjAsync`, **`GetPrimeiraAtivaAsync`** (W4) |
| `IClienteRepository` | `ClienteRepository` | `GetByDocumentoAsync` |
| `IFornecedorRepository` | `FornecedorRepository` | `GetByCnpjAsync` |
| `IFuncionarioRepository` | `FuncionarioRepository` | `GetByCpfAsync`, `ListByCargoAsync` |
| `IProdutoRepository` | `ProdutoRepository` | `GetByCodigoAsync`, `ListByTipoAsync` |

Todos herdam `BaseRepository<T>` que aplica `WHERE tenant_id = @tenantId`
automático.

## ViaCEP — integração externa

`IViaCepExternalClient` em `ExternalIntegration/Clients/ViaCep/`. HTTP via
`Refit` proxy (`viacep.com.br/ws/{cep}/json/`). Usado nas telas de cadastro
de empresa/cliente/fornecedor para auto-preencher endereço.

## Endpoints REST

CRUD padrão para cada entidade (substituir `<entidade>` por empresas, clientes,
fornecedores, funcionarios, produtos):

| Método | Rota | Permissão |
|--------|------|-----------|
| GET | `/api/v1/cadastros/<entidade>` | `<recurso>:ler` |
| GET | `/api/v1/cadastros/<entidade>/{id}` | `<recurso>:ler` |
| POST | `/api/v1/cadastros/<entidade>` | `<recurso>:criar` |
| PUT | `/api/v1/cadastros/<entidade>/{id}` | `<recurso>:editar` |
| DELETE | `/api/v1/cadastros/<entidade>/{id}` | `<recurso>:excluir` |

E adicionalmente:
- `GET /api/v1/cadastros/cep/{cep}` — ViaCEP wrapper
- `GET /api/v1/cadastros/ufs` — catálogo UF (27 estados, seed estático)

## Decisões

- **CPF/CNPJ** sempre validados via FluentValidation com algoritmo de DV
  (`CpfHelper`, `CnpjHelper` em `Core/Helper/`).
- **PIS/CTPS** validados via `PisHelper`, `CtpsHelper` (W1 RH).
- **Endereço** é Value Object embutido — colunas `endereco_*` direto na tabela
  pai (não FK para `enderecos` separada).
- **Funcionario.UsuarioId** é opcional (Funcionário pode existir sem login).
  Quando presente, é a ponte para JWT → contexto.

## Frontend

- `site/atena-web/src/app/features/cadastros/` — uma pasta por entidade com
  `*-list.component.ts` + `*-form.component.ts`, usando `CrudService` e
  `CrudListComponent`/`CrudFormComponent` genéricos.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Cadastros/`
- `src/Service/Acme.Sistemas.Domain/Interfaces/Repository/I{Empresa,Cliente,Fornecedor,Funcionario,Produto}Repository.cs`
- `src/Data/Acme.Sistemas.Repository/Repositories/V1/Cadastros/`
- `src/Service/Acme.Sistemas.Services/V1/{Empresa,Cliente,Fornecedor,Funcionario,Produto}/`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Cadastros/`
- `src/Data/Acme.Sistemas.ExternalIntegration/Clients/ViaCep/`
- `src/Service/Acme.Sistemas.Core/Helper/Cpf`/`Cnpj`/`Pis`/`Ctps*.cs`
- `site/atena-web/src/app/features/cadastros/`
- `documentacao/seeds/README.md` (catálogos UF + CFOP + NCM)

## Follow-ups conhecidos

- Anexos por entidade (contratos, RG, comprovantes) → integrar com GED.
- Cliente PF/PJ deveria ser herança/discriminator (atualmente flag).
