using acme.atena.api.DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace acme.atena.api.DTO.Util
{
    [NotMapped]
    public class ResponseApi
    {
        public string Mensagem { get; set; }
        public EnumResponseHttp ResponseHttp { get; set; }
        public List<NotificationDTO> Notifications { get; set; }
    }
}
