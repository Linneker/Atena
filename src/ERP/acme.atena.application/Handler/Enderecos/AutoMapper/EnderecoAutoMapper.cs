using acme.atena.application.Handler.Cliente.Commands.Input;
using acme.atena.application.Handler.Enderecos.Commands.Request;
using acme.atena.application.Handler.Enderecos.Commands.Response;
using acme.atena.domain.DTO.Util;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.AutoMapper
{
    public class EnderecoAutoMapper : Profile
    {
        public EnderecoAutoMapper() : this("Profile")
        {

        }
        public EnderecoAutoMapper(string bases) : base(bases)
        {
            CreateMap<Endereco, EnderecoCommand>();
            CreateMap<EnderecoCommand, Endereco>();


            CreateMap<Endereco, EnderecoCommandQuery>();
            CreateMap<EnderecoCommandQuery, Endereco>();

            CreateMap<EnderecoFornecedor, EnderecoFornecedorCommand>();
            CreateMap<EnderecoFornecedorCommand, EnderecoFornecedor>();
        }




    }
}
