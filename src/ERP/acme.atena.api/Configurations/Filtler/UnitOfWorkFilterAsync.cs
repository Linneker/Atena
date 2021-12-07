using acme.atena.domain.Interface.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.Configurations.Filtler
{
    public class UnitOfWorkFilterAsync : Attribute, IAsyncResultFilter 
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            try
            {
                IUnitOfWork baseApplication = (IUnitOfWork)context.HttpContext.RequestServices.GetService(typeof(IUnitOfWork));
                bool salvo = await baseApplication.CommitAsync();
                if (!salvo)
                    throw (new Exception("Problema ao salvar elemento"));
                else
                    await next();
            }
            catch
            {
                throw;
            }
        }
    }
}
