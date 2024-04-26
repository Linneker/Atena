using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.core.Message
{
    public abstract class Event: Message, INotification
    {
        protected Event()
        {
            DataHora = DateTime.Now;
        }

        public DateTime DataHora { get; private set; }
    }
}
