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
            services.IDRepository();
            services.DIService();
            return services;
        }
    }
}
