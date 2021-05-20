using imagem.bar.francisco.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace imagem.bar.francisco.domain.DTO.Util
{
    [NotMapped]
    public class ResponseApi
    {
        public string Mensagem { get; set; }
        public EnumResponseHttp ResponseHttp { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}
