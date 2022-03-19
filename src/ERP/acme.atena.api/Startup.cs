using acme.atena.api.Controllers;
using acme.atena.config.DI;
using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Account;
using acme.atena.domain.Interface.Service.Inventory;
using acme.atena.domain.Interface.Service.Person;
using acme.atena.domain.Interface.Service.Product;
using acme.atena.domain.Interface.Service.Product.Price;
using acme.atena.domain.Interface.Service.Security;
using acme.atena.domain.Interface.Service.Util;
using acme.atena.infra.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
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
            .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
            //services.AddDbContext<Context>(op => op.UseSqlServer(Configuration.GetConnectionString("Atena_SqlServer"))
            //.LogTo(Console.WriteLine, LogLevel.Information)
            //    .EnableSensitiveDataLogging()
            //    .EnableDetailedErrors());

            services = InjecaoDependencia(services);

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200",
                                                          "https://localhost:4200",
                                                          "https://localhost:5001",
                                                          "https://bardochiquinho.acmesistemas.com.br",
                                                          "http://atena.acmesistemas.com.br")
                                                            .AllowAnyHeader()
                                                            .AllowAnyMethod();
                                  });
            });


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

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder() 
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                .AddOData((opt, srv) =>
                    {
                        opt.AddRouteComponents(GetEdmModel()).Filter()
                        .Count()
                        .Expand()
                        .OrderBy()
                        .SkipToken()
                        .EnableQueryFeatures()
                        .Select();
                        srv.CreateScope();
                     });

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "acme.atena.api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
            logger.CreateLogger($"log_atena_${DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}");
            app.UseODataRouteDebug();
        }
        public IServiceCollection InjecaoDependencia(IServiceCollection services)
        {
            services.AddTransient<UnitOfWorkFilter>();
            services.DI();
            return services;
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();
            // Habilita funções OData como $filter, $select e etc..
            builder.EntitySet<Usuario>(nameof(Usuario))
                   .EntityType
                   .Filter()
                   .Count()
                   .Expand()
                   .OrderBy()
                   .Page()
                   .Select();
            builder.EntitySet<Permissao>(nameof(Permissao))
       .EntityType
       .Filter()
       .Count()
       .Expand()
       .OrderBy()
       .Page()
       .Select();
            builder.EntitySet<PermissaoUsuario>(nameof(PermissaoUsuario))
       .EntityType
       .Filter()
       .Count()
       .Expand()
       .OrderBy()
       .Page()
       .Select();
            builder.EntitySet<Venda>(nameof(Venda))
       .EntityType
       .Filter()
       .Count()
       .Expand()
       .OrderBy()
       .Page()
       .Select();
            builder.EntitySet<Compra>(nameof(Compra))
      .EntityType
      .Filter()
      .Count()
      .Expand()
      .OrderBy()
      .Page()
      .Select();

            builder.EntitySet<TipoProduto>(nameof(TipoProduto))
      .EntityType
      .Filter()
      .Count()
      .Expand()
      .OrderBy()
      .Page()
      .Select();
            builder.EntitySet<Produto>(nameof(Produto))
.EntityType
.Filter()
.Count()
.Expand()
.OrderBy()
.Page()
.Select();

            var valor = builder.GetEdmModel();
            return valor;


        }

        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Trace);

            builder.AddNLog();
            builder.AddJsonConsole();
            builder.AddConsole();
        });
    }


}
