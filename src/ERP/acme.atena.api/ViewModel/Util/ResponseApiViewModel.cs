using acme.atena.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Util
{
    public class ResponseApiViewModel
    {
        public string Mensagem { get; set; }
        public EnumResponseHttp ResponseHttp { get; set; }
        public List<NotificationViewModel> Notifications { get; set; }
    }
}
