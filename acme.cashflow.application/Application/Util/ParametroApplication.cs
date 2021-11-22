using acme.cashflow.application.Interface.Util;
using acme.cashflow.domain.DTO.Util;
using acme.cashflow.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Util
{
    public class ParametroApplication: ApplicationBase<Parametro>,IParametroApplication
    {
        private readonly IParametroService _parametroService;

        public ParametroApplication(IParametroService parametroService):base(parametroService)
        {
            _parametroService = parametroService;
        }
    }
}
