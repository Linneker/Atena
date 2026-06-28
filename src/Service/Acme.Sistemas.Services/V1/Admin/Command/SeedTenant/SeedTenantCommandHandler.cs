using System.Security.Cryptography;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Entities.Produtos;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Entities.Tenants;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using EmpresaEntity = Acme.Sistemas.Domain.Entities.Cadastros.Empresa;
using UsuarioEntity = Acme.Sistemas.Domain.Entities.Users.Usuario;
using TenantEntity = Acme.Sistemas.Domain.Entities.Tenants.Tenant;
using PlanoDeContasEntity = Acme.Sistemas.Domain.Entities.Financeiro.PlanoDeContas;
using CentroDeCustoEntity = Acme.Sistemas.Domain.Entities.Financeiro.CentroDeCusto;
using ClienteEntity = Acme.Sistemas.Domain.Entities.Cadastros.Cliente;
using FornecedorEntity = Acme.Sistemas.Domain.Entities.Cadastros.Fornecedor;
using ProdutoEntity = Acme.Sistemas.Domain.Entities.Produtos.Produto;

namespace Acme.Sistemas.Services.V1.Admin.Command.SeedTenant;

/// <summary>
/// Provisiona um tenant completo de forma idempotente. Após criar o tenant, faz
/// <see cref="IMutableTenantContext.Override"/> para que os repositórios tenant-scoped
/// (empresa, plano de contas, cliente, etc.) gravem no tenant recém-criado, e não no
/// tenant do admin que disparou a chamada.
/// </summary>
public sealed class SeedTenantCommandHandler
    : IRequestHandler<SeedTenantCommand, ResponseDefault<SeedTenantCommandResult>>
{
    private readonly ITenantRepository _tenants;
    private readonly IMutableTenantContext _tenantCtx;
    private readonly IRoleRepository _roles;
    private readonly IPermissionRepository _permissions;
    private readonly IRolePermissionRepository _rolePermissions;
    private readonly IUsuarioRepository _usuarios;
    private readonly IUserRoleRepository _userRoles;
    private readonly IEmpresaRepository _empresas;
    private readonly IPlanoDeContasRepository _planoContas;
    private readonly ICentroDeCustoRepository _centrosCusto;
    private readonly IClienteRepository _clientes;
    private readonly IFornecedorRepository _fornecedores;
    private readonly IProdutoRepository _produtos;
    private readonly IConfiguracaoFiscalRepository _configFiscal;
    private readonly IJornadaRepository _jornadas;
    private readonly ICargoRepository _cargos;
    private readonly IDepartamentoRepository _departamentos;
    private readonly ILotacaoRepository _lotacoes;

    public SeedTenantCommandHandler(
        ITenantRepository tenants,
        IMutableTenantContext tenantCtx,
        IRoleRepository roles,
        IPermissionRepository permissions,
        IRolePermissionRepository rolePermissions,
        IUsuarioRepository usuarios,
        IUserRoleRepository userRoles,
        IEmpresaRepository empresas,
        IPlanoDeContasRepository planoContas,
        ICentroDeCustoRepository centrosCusto,
        IClienteRepository clientes,
        IFornecedorRepository fornecedores,
        IProdutoRepository produtos,
        IConfiguracaoFiscalRepository configFiscal,
        IJornadaRepository jornadas,
        ICargoRepository cargos,
        IDepartamentoRepository departamentos,
        ILotacaoRepository lotacoes)
    {
        _tenants = tenants;
        _tenantCtx = tenantCtx;
        _roles = roles;
        _permissions = permissions;
        _rolePermissions = rolePermissions;
        _usuarios = usuarios;
        _userRoles = userRoles;
        _empresas = empresas;
        _planoContas = planoContas;
        _centrosCusto = centrosCusto;
        _clientes = clientes;
        _fornecedores = fornecedores;
        _produtos = produtos;
        _configFiscal = configFiscal;
        _jornadas = jornadas;
        _cargos = cargos;
        _departamentos = departamentos;
        _lotacoes = lotacoes;
    }

    public async Task<ResponseDefault<SeedTenantCommandResult>> Handle(
        SeedTenantCommand request, CancellationToken cancellationToken)
    {
        var cnpj = new string(request.Cnpj.Where(char.IsDigit).ToArray());

        var existing = await _tenants.GetByCnpjAsync(cnpj, cancellationToken);
        if (existing is not null)
        {
            // Idempotente: não recria nada nem re-exibe senha.
            _tenantCtx.Override(existing.Id);
            var users = await _usuarios.ListAsync(0, 1, cancellationToken);
            return ResponseDefault<SeedTenantCommandResult>.Ok(
                new SeedTenantCommandResult(existing.Id, users.FirstOrDefault()?.Id, null, EhNovo: false));
        }

        var tenant = new TenantEntity
        {
            RazaoSocial = request.RazaoSocial,
            Cnpj = cnpj,
            Plano = "FREE",
            Status = StatusAtivo.Ativo,
            FusoHorario = "America/Sao_Paulo",
            CreatedAt = DateTime.UtcNow,
        };
        await _tenants.AddAsync(tenant, cancellationToken);
        await _tenants.UpsertLimitesAsync(new TenantLimites
        {
            TenantId = tenant.Id,
            MaxUsuarios = 3,
            MaxNFeMes = 50,
            MaxStorageGb = 1,
        }, cancellationToken);

        // A partir daqui, os repositórios tenant-scoped gravam no novo tenant.
        _tenantCtx.Override(tenant.Id);

        var adminRoleId = await SeedRolesAsync(tenant.Id, cancellationToken);

        var senha = GerarSenhaAleatoria(16);
        var admin = new UsuarioEntity
        {
            TenantId = tenant.Id,
            NomeCompleto = "Administrador",
            Email = request.AdminEmail,
            PasswordHash = PasswordHelper.Hash(senha),
            Status = StatusAtivo.Ativo,
            EmailConfirmedAt = DateTime.UtcNow, // demo: já confirmado, login imediato
        };
        await _usuarios.AddAsync(admin, cancellationToken);
        await _userRoles.AssignAsync(new UserRole
        {
            UserId = admin.Id,
            RoleId = adminRoleId,
            TenantId = tenant.Id,
            GrantedAt = DateTime.UtcNow,
        }, cancellationToken);

        await SeedEmpresaAsync(request, cancellationToken);
        await SeedPlanoDeContasAsync(cancellationToken);
        await SeedCentrosDeCustoAsync(cancellationToken);
        await SeedCadastrosDemoAsync(cancellationToken);
        await SeedConfiguracaoFiscalAsync(request, cnpj, cancellationToken);
        await SeedRhDefaultsAsync(cancellationToken);

        return ResponseDefault<SeedTenantCommandResult>.Ok(
            new SeedTenantCommandResult(tenant.Id, admin.Id, senha, EhNovo: true));
    }

    private async Task<Guid> SeedRolesAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var all = await _permissions.ListAllAsync(cancellationToken);

        var rootOnly = Permissions.Of(Permissions.Recursos.Tenant, Permissions.Acoes.Criar);
        var seedTenant = Permissions.Of(Permissions.Recursos.Admin, Permissions.Acoes.SeedTenant);
        bool Grantable(Permission p) =>
            !string.Equals(p.Codigo, rootOnly, StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(p.Codigo, seedTenant, StringComparison.OrdinalIgnoreCase);

        var adminId = await CriarRoleAsync(tenantId, "Administrador", "Acesso total.",
            all.Where(Grantable), cancellationToken);

        var financeiroRecursos = new HashSet<string>
        {
            Permissions.Recursos.Despesa, Permissions.Recursos.Receita,
            Permissions.Recursos.ContaPagar, Permissions.Recursos.ContaReceber,
            Permissions.Recursos.FluxoDeCaixa, Permissions.Recursos.Relatorio,
            Permissions.Recursos.PlanoDeContas, Permissions.Recursos.CentroDeCusto,
        };
        await CriarRoleAsync(tenantId, "Financeiro", "Recursos financeiros e relatórios.",
            all.Where(p => financeiroRecursos.Contains(p.Recurso)), cancellationToken);

        var operadorRecursos = new HashSet<string>
        {
            Permissions.Recursos.Cliente, Permissions.Recursos.Fornecedor,
            Permissions.Recursos.Produto, Permissions.Recursos.Estoque,
            Permissions.Recursos.PedidoVenda, Permissions.Recursos.PedidoCompra,
            Permissions.Recursos.Orcamento, Permissions.Recursos.SolicitacaoCompra,
            Permissions.Recursos.Faturamento,
        };
        var operadorAcoes = new HashSet<string>
        {
            Permissions.Acoes.Ler, Permissions.Acoes.Criar, Permissions.Acoes.Editar,
        };
        await CriarRoleAsync(tenantId, "Operador", "Cadastros, vendas, compras e estoque (read+write).",
            all.Where(p => operadorRecursos.Contains(p.Recurso) && operadorAcoes.Contains(p.Acao)), cancellationToken);

        var fiscalRecursos = new HashSet<string>
        {
            Permissions.Recursos.NFe, Permissions.Recursos.ConfiguracaoFiscal,
            Permissions.Recursos.Auditoria,
        };
        await CriarRoleAsync(tenantId, "Fiscal", "NF-e, configuração fiscal e auditoria.",
            all.Where(p => fiscalRecursos.Contains(p.Recurso)), cancellationToken);

        await CriarRoleAsync(tenantId, "Visualizador", "Somente leitura.",
            all.Where(p => p.Acao == Permissions.Acoes.Ler), cancellationToken);

        var rhRecursos = new HashSet<string>
        {
            Permissions.Recursos.Rh, Permissions.Recursos.RhFuncionario,
            Permissions.Recursos.RhJornada, Permissions.Recursos.RhCargo,
            Permissions.Recursos.RhLotacao, Permissions.Recursos.RhBeneficio,
            Permissions.Recursos.RhDependente, Permissions.Recursos.RhDepartamento,
            // W2 — RH gerencia ponto interno (incluindo aprovações + fechamento + banco de horas)
            Permissions.Recursos.RhPonto, Permissions.Recursos.RhBancoHoras,
            Permissions.Recursos.RhPoliticasPonto,
        };
        await CriarRoleAsync(tenantId, "RH",
            "Recursos Humanos: funcionários, jornadas, cargos, benefícios, dependentes e ponto interno.",
            all.Where(p => rhRecursos.Contains(p.Recurso)), cancellationToken);

        // Role Gestor — aprova ponto da equipe e vê hierarquia direta.
        var gestorRecursos = new HashSet<string>
        {
            Permissions.Recursos.RhPonto, Permissions.Recursos.RhBancoHoras,
            Permissions.Recursos.RhFuncionario,
        };
        var gestorAcoes = new HashSet<string>
        {
            Permissions.Acoes.Ler, Permissions.Acoes.Editar,
            Permissions.Acoes.BaterPonto, Permissions.Acoes.AjustarPonto,
            Permissions.Acoes.AprovarPonto, Permissions.Acoes.GerirEquipe,
        };
        await CriarRoleAsync(tenantId, "Gestor",
            "Aprova ponto e ajustes da própria equipe; visualiza ficha dos subordinados.",
            all.Where(p => gestorRecursos.Contains(p.Recurso) && gestorAcoes.Contains(p.Acao)),
            cancellationToken);

        // Role Funcionário — vinculada automaticamente a todo funcionário criado.
        // Permite bater o próprio ponto + ler/ajustar próprio + ver banco de horas próprio.
        var funcionarioPerms = new[]
        {
            Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.BaterPonto),
            Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.Ler),
            Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.AjustarPonto),
            Permissions.Of(Permissions.Recursos.RhBancoHoras, Permissions.Acoes.Ler),
        };
        await CriarRoleAsync(tenantId, "Funcionario",
            "Bate ponto próprio + visualiza próprio espelho + solicita ajustes próprios.",
            all.Where(p => funcionarioPerms.Contains(p.Codigo, StringComparer.OrdinalIgnoreCase)),
            cancellationToken);

        return adminId;
    }

    private async Task<Guid> CriarRoleAsync(
        Guid tenantId, string nome, string descricao,
        IEnumerable<Permission> permissoes, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Nome = nome,
            Descricao = descricao,
            IsSystem = true,
            CreatedAt = DateTime.UtcNow,
        };
        await _roles.AddAsync(role, cancellationToken);

        var ids = permissoes.Select(p => p.Id).Distinct().ToList();
        if (ids.Count > 0)
            await _rolePermissions.GrantAllToRoleAsync(role.Id, ids, grantedBy: null, cancellationToken);

        return role.Id;
    }

    private Task SeedEmpresaAsync(SeedTenantCommand request, CancellationToken cancellationToken)
        => _empresas.AddAsync(new EmpresaEntity
        {
            RazaoSocial = request.RazaoSocial,
            NomeFantasia = request.RazaoSocial,
            Cnpj = new string(request.Cnpj.Where(char.IsDigit).ToArray()),
            Email = request.AdminEmail,
            Status = StatusAtivo.Ativo,
            Endereco = new Endereco { Uf = "SP", Cidade = "São Paulo", Pais = "Brasil" },
        }, cancellationToken);

    private async Task SeedPlanoDeContasAsync(CancellationToken cancellationToken)
    {
        // Grupos raiz (nível 1) + filhas (nível 2). Estrutura mínima funcional; o tenant
        // customiza via UI.
        var grupos = new (string Codigo, string Nome, TipoConta Tipo, (string Codigo, string Nome)[] Filhas)[]
        {
            ("1", "Ativo", TipoConta.Ativo, new[]
            {
                ("1.1", "Caixa e equivalentes"), ("1.2", "Contas a Receber"), ("1.3", "Estoque"),
            }),
            ("2", "Passivo", TipoConta.Passivo, new[]
            {
                ("2.1", "Fornecedores"), ("2.2", "Contas a Pagar"), ("2.3", "Obrigações Tributárias"),
            }),
            ("3", "Patrimônio Líquido", TipoConta.PatrimonioLiquido, new[]
            {
                ("3.1", "Capital Social"),
            }),
            ("4", "Receitas", TipoConta.Receita, new[]
            {
                ("4.1", "Receita de Vendas"), ("4.2", "Receita de Serviços"),
            }),
            ("5", "Despesas", TipoConta.Despesa, new[]
            {
                ("5.1", "Despesas Operacionais"), ("5.2", "Despesas Tributárias"),
                ("5.3", "Despesas com Pessoal"),
            }),
        };

        foreach (var grupo in grupos)
        {
            var pai = new PlanoDeContasEntity
            {
                Codigo = grupo.Codigo, Nome = grupo.Nome, Tipo = grupo.Tipo,
                Nivel = 1, Aceita_Lancamento = false, Ativo = true,
            };
            await _planoContas.AddAsync(pai, cancellationToken);

            foreach (var (codigo, nome) in grupo.Filhas)
            {
                await _planoContas.AddAsync(new PlanoDeContasEntity
                {
                    Codigo = codigo, Nome = nome, Tipo = grupo.Tipo, PaiId = pai.Id,
                    Nivel = 2, Aceita_Lancamento = true, Ativo = true,
                }, cancellationToken);
            }
        }
    }

    private async Task SeedCentrosDeCustoAsync(CancellationToken cancellationToken)
    {
        var centros = new[]
        {
            ("ADM", "Administrativo"), ("COM", "Comercial"), ("OPE", "Operacional"),
        };
        foreach (var (codigo, nome) in centros)
        {
            await _centrosCusto.AddAsync(new CentroDeCustoEntity
            {
                Codigo = codigo, Nome = nome, Ativo = true,
            }, cancellationToken);
        }
    }

    private async Task SeedCadastrosDemoAsync(CancellationToken cancellationToken)
    {
        await _clientes.AddAsync(new ClienteEntity
        {
            Tipo = TipoPessoa.Juridica,
            Nome = "Cliente Demo",
            NomeFantasia = "Cliente Demo",
            Documento = "11222333000181",
            Email = "cliente@demo.test",
            Status = StatusAtivo.Ativo,
            Endereco = new Endereco { Uf = "SP", Cidade = "São Paulo", Pais = "Brasil" },
        }, cancellationToken);

        await _fornecedores.AddAsync(new FornecedorEntity
        {
            Tipo = TipoPessoa.Juridica,
            Nome = "Fornecedor Demo",
            NomeFantasia = "Fornecedor Demo",
            Documento = "44555666000172",
            Email = "fornecedor@demo.test",
            Status = StatusAtivo.Ativo,
            Endereco = new Endereco { Uf = "SP", Cidade = "São Paulo", Pais = "Brasil" },
        }, cancellationToken);

        await _produtos.AddAsync(new ProdutoEntity
        {
            Codigo = "DEMO-001",
            Nome = "Produto Demo",
            Descricao = "Produto de demonstração para fluxos E2E.",
            UnidadeMedida = "UN",
            CustoMedio = 10m,
            EstoqueMinimo = 1m,
            Status = StatusAtivo.Ativo,
        }, cancellationToken);
    }

    private Task SeedConfiguracaoFiscalAsync(SeedTenantCommand request, string cnpj, CancellationToken cancellationToken)
        => _configFiscal.UpsertAsync(new ConfiguracaoFiscal
        {
            Ambiente = AmbienteFiscal.Homologacao,
            Modo = ModoTransmissao.Normal,
            Uf = "SP",
            CnpjEmitente = cnpj,
            RazaoSocialEmitente = request.RazaoSocial,
            SerieNFe = 1,
            ProximoNumero = 1,
        }, cancellationToken);

    private async Task SeedRhDefaultsAsync(CancellationToken cancellationToken)
    {
        // Jornada padrão CLT 44h semanais, seg-sex 08:00-12:00 / 13:30-17:30 + sáb 08:00-12:00.
        // Janelas em JSON são consumidas pelo engine de ponto (W2) e folha (W6).
        const string janelas44h = """
            [
              {"dia":"seg","entrada":"08:00","saidaAlmoco":"12:00","voltaAlmoco":"13:30","saida":"17:30"},
              {"dia":"ter","entrada":"08:00","saidaAlmoco":"12:00","voltaAlmoco":"13:30","saida":"17:30"},
              {"dia":"qua","entrada":"08:00","saidaAlmoco":"12:00","voltaAlmoco":"13:30","saida":"17:30"},
              {"dia":"qui","entrada":"08:00","saidaAlmoco":"12:00","voltaAlmoco":"13:30","saida":"17:30"},
              {"dia":"sex","entrada":"08:00","saidaAlmoco":"12:00","voltaAlmoco":"13:30","saida":"17:30"},
              {"dia":"sab","entrada":"08:00","saida":"12:00"}
            ]
            """;

        if (await _jornadas.GetByNomeAsync("44h CLT", cancellationToken) is null)
        {
            await _jornadas.AddAsync(new Jornada
            {
                Nome = "44h CLT",
                Tipo = TipoJornada.Fixa,
                CargaSemanalHoras = 44m,
                CargaDiariaHoras = 8m,
                JanelasJson = janelas44h,
                PermiteMarcarIntervalo = true,
                ToleranciaMinutos = 10,
                Ativo = true,
            }, cancellationToken);
        }

        if (await _cargos.GetByCodigoAsync("NAO-CLASS", cancellationToken) is null)
        {
            await _cargos.AddAsync(new Cargo
            {
                Codigo = "NAO-CLASS",
                Descricao = "Não classificado",
                Ativo = true,
            }, cancellationToken);
        }

        if (await _departamentos.GetByCodigoAsync("NAO-CLASS", cancellationToken) is null)
        {
            await _departamentos.AddAsync(new Departamento
            {
                Codigo = "NAO-CLASS",
                Nome = "Não classificado",
                Ativo = true,
            }, cancellationToken);
        }

        if (await _lotacoes.GetByNomeAsync("Sede", cancellationToken) is null)
        {
            await _lotacoes.AddAsync(new Lotacao
            {
                Nome = "Sede",
                Ativo = true,
            }, cancellationToken);
        }
    }

    private static string GerarSenhaAleatoria(int tamanho)
    {
        const string alfabeto = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
        var bytes = RandomNumberGenerator.GetBytes(tamanho);
        var chars = new char[tamanho];
        for (var i = 0; i < tamanho; i++)
            chars[i] = alfabeto[bytes[i] % alfabeto.Length];
        return new string(chars);
    }
}
