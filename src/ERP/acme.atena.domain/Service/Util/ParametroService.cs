using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Util;
using acme.atena.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Service.Util
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
