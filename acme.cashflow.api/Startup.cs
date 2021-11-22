using acme.cashflow.application.Application;
using acme.cashflow.application.Application.Account;
using acme.cashflow.application.Application.Security;
using acme.cashflow.application.Application.Util;
using acme.cashflow.application.Interface;
using acme.cashflow.application.Interface.Account;
using acme.cashflow.application.Interface.Security;
using acme.cashflow.application.Interface.Util;
using acme.cashflow.domain.DTO;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Repository;
using acme.cashflow.domain.Interface.Repository.Account;
using acme.cashflow.domain.Interface.Repository.Security;
using acme.cashflow.domain.Interface.Repository.Util;
using acme.cashflow.domain.Interface.Service;
using acme.cashflow.domain.Interface.Service.Account;
using acme.cashflow.domain.Interface.Service.Security;
using acme.cashflow.domain.Interface.Service.Util;
using acme.cashflow.domain.Service;
using acme.cashflow.domain.Service.Account;
using acme.cashflow.domain.Service.Security;
using acme.cashflow.domain.Service.Util;
using acme.cashflow.infra.Config;
using acme.cashflow.repository;
using acme.cashflow.repository.Account;
using acme.cashflow.repository.Security;
using acme.cashflow.repository.Util;
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
using System.Threading.Tasks;

namespace acme.cashflow.api
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
            services.AddEntityFrameworkMySQL().AddDbContext<Context>(op => op.UseMySQL(Configuration.GetConnectionString("BarFrancisco")));


            services.AddTransient<IApplicationBase<AbstractEntity>, ApplicationBase<AbstractEntity>>();
            services.AddTransient<IDespesaApplication, DespesaApplication>();
            services.AddTransient<IReceitaApplication, ReceitaApplication>();
            services.AddTransient<IFluxoDeCaixaApplication, FluxoDeCaixaApplication>();
            services.AddTransient<ICompetenciaApplication, CompetenciaApplication>();
            services.AddTransient<IAutorizacaoApiApplication, AutorizacaoApiApplication>();

            services.AddTransient<IUsuarioApplication, UsuarioApplication>();

            services.AddTransient<IServiceBase<AbstractEntity>, ServiceBase<AbstractEntity>>();
            services.AddTransient<IDespesaService, DespesaService>();
            services.AddTransient<IReceitaService, ReceitaService>();
            services.AddTransient<IFluxoDeCaixaService, FluxoDeCaixaService>();
            services.AddTransient<ICompetenciaService, CompetenciaService>();
            services.AddTransient<IAutorizacaoApiService, AutorizacaoApiService>();
            services.AddTransient<IUsuarioService, UsuarioService>();

            services.AddTransient<IRepositoryBase<AbstractEntity>, RepositoryBase<AbstractEntity>>();
            services.AddTransient<IDespesaRepository, DespesaRepository>();
            services.AddTransient<IReceitaRepository, ReceitaRepository>();
            services.AddTransient<IFluxoDeCaixaRepository, FluxoDeCaixaRepository>();
            services.AddTransient<ICompetenciaRepository, CompetenciaRepository>();
            services.AddTransient<IAutorizacaoApiRepository, AutorizacaoApiRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();

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

                // Verifica se um token recebido ainda � v�lido
                paramsValidation.ValidateLifetime = true;
                paramsValidation.SaveSigninToken = true;

                // Tempo de toler�ncia para a expira��o de um token (utilizado
                // caso haja problemas de sincronismo de hor�rio entre diferentes
                // computadores envolvidos no processo de comunica��o)
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "acme.cashflow.api", Version = "v1" });
                c.AddSecurityDefinition(
                    "Bearer", 
                    new OpenApiSecurityScheme {
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "acme.cashflow.api v1"));
            }

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