using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.core.Message
{
    public abstract class Message
    {
        protected Message()
        {
            TipoMensagem = GetType().Name;
        }

        public string TipoMensagem { get; protected set; }
        public Guid AggregateId { get; protected set; }
    }
}
