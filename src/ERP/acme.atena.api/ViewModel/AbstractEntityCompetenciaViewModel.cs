using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel
{
    public abstract class AbstractEntityCompetenciaViewModel : AbstractEntityViewModel
    {
        public  Guid CompetenciaId { get; set; }
        public virtual Competencia Competencia { get; set; }

    }
}
