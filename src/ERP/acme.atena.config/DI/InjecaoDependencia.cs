using acme.atena.application.Handler.Cliente.AutoMapper;
using acme.atena.application.Handler.Enderecos.AutoMapper;
using acme.atena.application.Handler.Fornecedores.AutoMapper;
using acme.atena.config.DI.Pacotes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.config.DI
{
    public static class InjecaoDependencia
    {
        public static IServiceCollection DI(this IServiceCollection services)
        {
            services.RepositoryDI();
            services.ServiceDI();

            AutoMapperDI(services);

            return services;
        }

        public static void AutoMapperDI(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(EnderecoAutoMapper));
            services.AddAutoMapper(typeof(ClienteAutoMapper));
            services.AddAutoMapper(typeof(FornecedorAutoMapper));
        }
    }
}
