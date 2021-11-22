using acme.cashflow.domain.DTO.Util;
using acme.cashflow.domain.Interface.Repository;
using acme.cashflow.domain.Interface.Repository.Util;
using acme.cashflow.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Util
{
    public class ParametroService : ServiceBase<Parametro>, IParametroService
    {
        private readonly IParametroRepository _parametroRepository;

        public ParametroService(IParametroRepository  repositoryBase) : base(repositoryBase)
        {
            _parametroRepository = repositoryBase;
        }
    }
}
