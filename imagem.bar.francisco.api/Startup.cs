using imagem.bar.francisco.application.Application;
using imagem.bar.francisco.application.Application.Account;
using imagem.bar.francisco.application.Application.Security;
using imagem.bar.francisco.application.Application.Util;
using imagem.bar.francisco.application.Interface;
using imagem.bar.francisco.application.Interface.Account;
using imagem.bar.francisco.application.Interface.Security;
using imagem.bar.francisco.application.Interface.Util;
using imagem.bar.francisco.domain.DTO;
using imagem.bar.francisco.domain.DTO.Seguranca;
using imagem.bar.francisco.domain.Interface.Repository;
using imagem.bar.francisco.domain.Interface.Repository.Account;
using imagem.bar.francisco.domain.Interface.Repository.Security;
using imagem.bar.francisco.domain.Interface.Repository.Util;
using imagem.bar.francisco.domain.Interface.Service;
using imagem.bar.francisco.domain.Interface.Service.Account;
using imagem.bar.francisco.domain.Interface.Service.Security;
using imagem.bar.francisco.domain.Interface.Service.Util;
using imagem.bar.francisco.domain.Service;
using imagem.bar.francisco.domain.Service.Account;
using imagem.bar.francisco.domain.Service.Security;
using imagem.bar.francisco.domain.Service.Util;
using imagem.bar.francisco.infra.Config;
using imagem.bar.francisco.repository;
using imagem.bar.francisco.repository.Account;
using imagem.bar.francisco.repository.Security;
using imagem.bar.francisco.repository.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MySql.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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

            services.AddTransient<IServiceBase<AbstractEntity>, ServiceBase<AbstractEntity>>();
            services.AddTransient<IDespesaService, DespesaService>();
            services.AddTransient<IReceitaService, ReceitaService>();
            services.AddTransient<IFluxoDeCaixaService, FluxoDeCaixaService>();
            services.AddTransient<ICompetenciaService, CompetenciaService>();
            services.AddTransient<IAutorizacaoApiService, AutorizacaoApiService>();

            services.AddTransient<IRepositoryBase<AbstractEntity>, RepositoryBase<AbstractEntity>>();
            services.AddTransient<IDespesaRepository, DespesaRepository>();
            services.AddTransient<IReceitaRepository, ReceitaRepository>();
            services.AddTransient<IFluxoDeCaixaRepository, FluxoDeCaixaRepository>();
            services.AddTransient<ICompetenciaRepository, CompetenciaRepository>();
            services.AddTransient<IAutorizacaoApiRepository, AutorizacaoApiRepository>();

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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "imagem.bar.francisco.api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "imagem.bar.francisco.api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
