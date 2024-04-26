using acme.atena.application.Handler.Cliente.Commands.Input;
using acme.atena.application.Handler.Fornecedores.Commands.Request;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Fornecedores.AutoMapper
{
    public class FornecedorAutoMapper : Profile
    {
        public FornecedorAutoMapper() : this("Profile")
        {

        }
        public FornecedorAutoMapper(string bases) : base(bases)
        {
            CreateMap<domain.DTO.Person.Fornecedor, FornecedorCommand>();
            CreateMap<FornecedorCommand, domain.DTO.Person.Fornecedor>();

        }
    }
}
