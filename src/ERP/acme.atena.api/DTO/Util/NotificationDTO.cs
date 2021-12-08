using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace acme.atena.api.DTO.Util
{
    [NotMapped]
    public class NotificationDTO
    {
        public NotificationDTO(string key, string mensagem)
        {
            Mensagem = mensagem;
            Key = key;
        }

        public string Key { get; private set; }
        public string Mensagem { get; private set; }
    }
}
