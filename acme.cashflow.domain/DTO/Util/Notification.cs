using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace acme.cashflow.domain.DTO.Util
{
    [NotMapped]
    public class Notification
    {
        public Notification(string key, string mensagem)
        {
            Mensagem = mensagem;
            Key = key;
        }

        public string Key { get; private set; }
        public string Mensagem { get; private set; }
    }
}
