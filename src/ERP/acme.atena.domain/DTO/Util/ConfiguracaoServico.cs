using acme.atena.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Util
{
    public class ConfiguracaoServico: AbstractEntity
    {
        public ConfiguracaoServico()
        {
        }

        public ConfiguracaoServico(string url, string endpointDefault, string endpointComplement, bool isSecurity, EnumTypeService typeService, string identifierConfigutionSerivices, string accessKey)
        {
            Url = url;
            EndpointDefault = endpointDefault;
            EndpointComplement = endpointComplement;
            IsSecurity = isSecurity;
            TypeService = typeService;
            IdentifierConfigutionSerivices = identifierConfigutionSerivices;
            AccessKey = accessKey;
        }

        public string Url { get; set; }
        public string EndpointDefault { get; set; }
        public string EndpointComplement { get; set; }
        public bool IsSecurity { get; set; }
        public EnumTypeService TypeService { get; set; }
        public string IdentifierConfigutionSerivices { get; set; }
        public string AccessKey { get; set; }
    }
}
