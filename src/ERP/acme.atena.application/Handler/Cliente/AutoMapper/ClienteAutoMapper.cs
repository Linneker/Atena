using acme.atena.application.Handler.Cliente.Commands.Input;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Cliente.AutoMapper
{
    public class ClienteAutoMapper : Profile
    {
        public ClienteAutoMapper() : this("Profile")
        {

        }
        public ClienteAutoMapper(string bases) : base(bases)
        {
            CreateMap<domain.DTO.Person.Cliente, CadastroClienteCommand>();
            CreateMap<CadastroClienteCommand,domain.DTO.Person.Cliente>();

        }




    }
}
