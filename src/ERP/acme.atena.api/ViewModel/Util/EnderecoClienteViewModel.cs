﻿using acme.atena.domain.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Util
{
    public class EnderecoClienteViewModel : EnderecoPessoaViewModel
    {
        public Guid ClienteId { get; set; }

        public virtual Cliente Cliente{ get; set; }
    }
}
