using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO
{
    public abstract class AbstractEntityCompetencia : AbstractEntity
    {
        public  Guid CompetenciaId { get; set; }
        public virtual Competencia Competencia { get; set; }

    }
}
