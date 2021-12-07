using acme.atena.application.Interface.Util;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Util
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
