using acme.atena.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace acme.atena.application.Handler.Comum.Commands.Response
{
    public class OutputAbstractEntityCommand
    {
        public virtual Guid Id { get; set; }
        [Display(Name = "Criação")]
        public DateTime? DataCriacao { get; set; }
        [Display(Name = "Modificação")]
        public DateTime? DataModificacao { get; set; }
        [Display(Name = "Inativação")]
        public DateTime? DataInativacao { get; set; }
        [Display(Name = "Usuario")]
        public Guid? UsuarioCriacao { get; set; }
        public Guid? UsuarioModificacao { get; set; }

        public EnumStatus Status { get; set; }
    }
}
