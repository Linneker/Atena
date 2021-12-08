using acme.atena.api.Controllers;
using acme.atena.application.Application;
using acme.atena.application.Application.Account;
using acme.atena.application.Application.Inventory;
using acme.atena.application.Application.Person;
using acme.atena.application.Application.Product;
using acme.atena.application.Application.Security;
using acme.atena.application.Application.Util;
using acme.atena.application.Interface;
using acme.atena.application.Interface.Account;
using acme.atena.application.Interface.Inventory;
using acme.atena.application.Interface.Person;
using acme.atena.application.Interface.Product;
using acme.atena.application.Interface.Security;
using acme.atena.application.Interface.Util;
using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Repository.Inventory;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.domain.Interface.Repository.UnitOfWork;
using acme.atena.domain.Interface.Repository.Util;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Account;
using acme.atena.domain.Interface.Service.Inventory;
using acme.atena.domain.Interface.Service.Person;
using acme.atena.domain.Interface.Service.Product;
using acme.atena.domain.Interface.Service.Security;
using acme.atena.domain.Interface.Service.Util;
using acme.atena.domain.Service;
using acme.atena.domain.Service.Account;
using acme.atena.domain.Service.Inventory;
using acme.atena.domain.Service.Person;
using acme.atena.domain.Service.Product;
using acme.atena.domain.Service.Security;
using acme.atena.domain.Service.Util;
using acme.atena.infra.Config;
using acme.atena.repository;
using acme.atena.repository.Account;
using acme.atena.repository.Inventory;
using acme.atena.repository.Person;
using acme.atena.repository.Product;
using acme.atena.repository.Security;
using acme.atena.repository.UnitOfWork;
using acme.atena.repository.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace acme.atena.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(op => op.UseMySQL(Configuration.GetConnectionString("Atena"))
            .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

            //Application
            services.AddTransient<IApplicationBase<AbstractEntity>, ApplicationBase<AbstractEntity>>();
            services.AddTransient<IDespesaApplication, DespesaApplication>();
            services.AddTransient<IReceitaApplication, ReceitaApplication>();
            services.AddTransient<IFluxoDeCaixaApplication, FluxoDeCaixaApplication>();
            services.AddTransient<IDividaApplication, DividaApplication>();
            services.AddTransient<IPagamentoApplication, PagamentoApplication>();
            services.AddTransient<IPagamentoVendaApplication, PagamentoVendaApplication>();

            services.AddTransient<IEstoqueApplication, EstoqueApplication>();
            services.AddTransient<IEstoqueProdutoApplication, EstoqueProdutoApplication>();
            services.AddTransient<IEntradaProdutoEstoqueApplication, EntradaProdutoEstoqueApplication>();
            services.AddTransient<ISaidaProdutoEstoqueApplication, SaidaProdutoEstoqueApplication>();


            services.AddTransient<ICompraApplication, CompraApplication>();
            services.AddTransient<ICompraProdutoApplication, CompraProdutoApplication>();
            services.AddTransient<IProdutoApplication, ProdutoApplication>();
            services.AddTransient<IVendaApplication, VendaApplication>();
            services.AddTransient<IVendaProdutoApplication, VendaProdutoApplication>();
            services.AddTransient<IDevolucaoCompraApplication, DevolucaoCompraApplication>();
            services.AddTransient<IDevolucaoVendaApplication, DevolucaoVendaApplication>();

            services.AddTransient<IClienteApplication, ClienteApplication>();
            services.AddTransient<IFornecedorApplication, FornecedorApplication>();
            services.AddTransient<IEmpresaApplication, EmpresaApplication>();

            services.AddTransient<ICompetenciaApplication, CompetenciaApplication>();
            services.AddTransient<IParametroApplication, ParametroApplication>();
            services.AddTransient<IEnderecoApplication, EnderecoApplication>();

            services.AddTransient<IAutorizacaoApiApplication, AutorizacaoApiApplication>();
            services.AddTransient<IUsuarioApplication, UsuarioApplication>();
            services.AddTransient<IPermissaoApplication, PermissaoApplication>();
            services.AddTransient<IPermissaoUsuarioApplication, PermissaoUsuarioApplication>();
            
            //SERVICE
            services.AddTransient<IServiceBase<AbstractEntity>, ServiceBase<AbstractEntity>>();
            services.AddTransient<IDespesaService, DespesaService>();
            services.AddTransient<IReceitaService, ReceitaService>();
            services.AddTransient<IFluxoDeCaixaService, FluxoDeCaixaService>();
            services.AddTransient<IDividaService, DividaService>();
            services.AddTransient<IPagamentoService, PagamentoService>();
            services.AddTransient<IPagamentoVendaService, PagamentoVendaService>();

            services.AddTransient<IEstoqueService, EstoqueService>();
            services.AddTransient<IEstoqueProdutoService, EstoqueProdutoService>();
            services.AddTransient<IEntradaProdutoEstoqueService, EntradaProdutoEstoqueService>();
            services.AddTransient<ISaidaProdutoEstoqueService, SaidaProdutoEstoqueService>();


            services.AddTransient<ICompraService, CompraService>();
            services.AddTransient<ICompraProdutoService, CompraProdutoService>();
            services.AddTransient<IProdutoService, ProdutoService>();
            services.AddTransient<IVendaService, VendaService>();
            services.AddTransient<IVendaProdutoService, VendaProdutoService>();
            services.AddTransient<IDevolucaoCompraService, DevolucaoCompraService>();
            services.AddTransient<IDevolucaoVendaService, DevolucaoVendaService>();

            services.AddTransient<IEmpresaService, EmpresaService>();
            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<IFornecedorService, FornecedorService>();

            services.AddTransient<ICompetenciaService, CompetenciaService>();
            services.AddTransient<IParametroService, ParametroService>();
            services.AddTransient<IEnderecoService, EnderecoService>();

            services.AddTransient<IAutorizacaoApiService, AutorizacaoApiService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IPermissaoService, PermissaoService>();
            services.AddTransient<IPermissaoUsuarioService, PermissaoUsuarioService>();

            //REPOSITORY
            services.AddTransient<IRepositoryBase<AbstractEntity>, RepositoryBase<AbstractEntity>>();
            services.AddTransient<IDespesaRepository, DespesaRepository>();
            services.AddTransient<IReceitaRepository, ReceitaRepository>();
            services.AddTransient<IFluxoDeCaixaRepository, FluxoDeCaixaRepository>();
            services.AddTransient<IDividaRepository, DividaRepository>();
            services.AddTransient<IPagamentoRepository, PagamentoRepository>();

            services.AddTransient<IEstoqueRepository, EstoqueRepository>();
            services.AddTransient<IEstoqueProdutoRepository, EstoqueProdutoRepository>();
            services.AddTransient<IEntradaProdutoEstoqueRepository, EntradaProdutoEstoqueRepository>();
            services.AddTransient<ISaidaProdutoEstoqueRepository, SaidaProdutoEstoqueRepository>();

            services.AddTransient<ICompraRepository, CompraRepository>();
            services.AddTransient<ICompraProdutoRepository, CompraProdutoRepository>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IVendaRepository, VendaRepository>();
            services.AddTransient<IVendaProdutoRepository, VendaProdutoRepository>();
            services.AddTransient<IDevolucaoCompraRepository, DevolucaoCompraRepository>();
            services.AddTransient<IDevolucaoVendaRepository, DevolucaoVendaRepository>();
            services.AddTransient<IPagamentoVendaRepository, PagamentoVendaRepository>();

            services.AddTransient<IEmpresaRepository, EmpresaRepository>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IFornecedorRepository, FornecedorRepository>();

            services.AddTransient<ICompetenciaRepository, CompetenciaRepository>();
            services.AddTransient<IParametroRepository, ParametroRepository>();
            services.AddTransient<IEnderecoRepository, EnderecoRepository>();

            services.AddTransient<IAutorizacaoApiRepository, AutorizacaoApiRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IPermissaoRepository, PermissaoRepository>();
            services.AddTransient<IPermissaoUsuarioRepository, PermissaoUsuarioRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<UnitOfWorkFilter>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;
                paramsValidation.SaveSigninToken = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;

            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200",
                                                          "https://localhost:4200",
                                                          "https://bardochiquinho.acmesistemas.com.br/",
                                                          "https://bardochiquinho.acmesistemas.com.br/api/").
                                                          AllowAnyHeader().
                                                          AllowAnyMethod().
                                                          AllowAnyOrigin();
                                  });
            });
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Atena", Version = "v1" });
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Please enter JWT with Bearer into field"

                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "acme.atena.api v1"));

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
