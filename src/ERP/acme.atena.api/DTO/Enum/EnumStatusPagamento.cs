using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.DTO.Enum
{
    public enum EnumStatusPagamento
    {
        [Description("Pago")]
        Pago = 1,
        [Description("Em Atraso")]
        EmAtraso = 2,
        [Description("Em Aberto")]
        EmAberto = 3,
        [Description("Parcela Paga")]
        ParcelaPaga = 4,
        [Description("Parcela Em Atraso")]
        ParcelaEmAtraso = 5,
        [Description("Parcela Em Aberto")]
        ParcelaEmAberto = 6
    }
}
