using imagem.bar.francisco.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.ViewModel
{
    public class AbstractEntityViewModel
    {
        public AbstractEntityViewModel()
        {
            Id = Guid.NewGuid();
            DataCriacao ??= DateTime.Now;
        }

        public virtual Guid Id { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public DateTime? DataInativacao { get; set; }

        public EnumStatus Status { get; set; }
    }
}
