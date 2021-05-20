using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.ViewModel.Util
{
    public class NotificationViewModel 
    {
        public NotificationViewModel(string key, string mensagem)
        {
            Mensagem = mensagem;
            Key = key;
        }

        public string Key { get; private set; }
        public string Mensagem { get; private set; }
    }
}
