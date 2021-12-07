using acme.atena.domain.Interface.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace acme.atena.api.Controllers
{
    public class UnitOfWorkFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            try
            {
                IUnitOfWork baseApplication = (IUnitOfWork)context.HttpContext.RequestServices.GetService(typeof(IUnitOfWork));
                bool salvo = baseApplication.Commit();
               
            }
            catch
            {
                throw;
            }
        }
    }
}
