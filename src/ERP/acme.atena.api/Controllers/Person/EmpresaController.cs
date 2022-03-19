using acme.atena.api.Configurations.Filtler;
using acme.atena.api.DTO.Person;
using acme.atena.api.ViewModel.Person;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Service.Person;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace acme.atena.api.Controllers.Person
{
    public class EmpresaController : BaseApiController<Empresa, EmpresaViewModel>
    {
        private readonly IEmpresaService _empresaApplication;
        private readonly IMapper _mapper;
        private const string SERVICO = "EMPRESA";
        public EmpresaController(IMapper mapper, IEmpresaService empresaApplication) : base(mapper, empresaApplication, SERVICO)
        {
            _mapper = mapper;
            _empresaApplication = empresaApplication;
        }
        [Authorize("Bearer")]
        [UnitOfWorkFilterAsync]
        [HttpPost("Add/Async/DTO")]
        public void AddAsyncWhitDTO(EmpresaDTO empresaDTO)
        {
            Empresa empresa = new Empresa()
            {
                Celular = empresaDTO.Celular,
                DataNascimento = empresaDTO.DataNascimento,
                Email=  empresaDTO.Email,
                EmpresaMatrizId = empresaDTO.EmpresaMatrizId,
                IsFilial = empresaDTO.IsFilial,
                Nome = empresaDTO.Nome,
                RazaoSocial = empresaDTO.RazaoSocial
            };
            empresaDTO.EnderecoEmpresas.ToList().ForEach(t =>
            {
                EnderecoEmpresa enderecoEmpresa = new EnderecoEmpresa()
                {
                    Complemento = t.Complemento,
                    Endereco = new Endereco()
                    {
                        Estado = t.Endereco.Estado,
                        Bairro = t.Endereco.Bairro,
                        Cep = t.Endereco.Cep,
                        Cidade = t.Endereco.Cidade,
                        Pais = t.Endereco.Pais,
                        Rua = t.Endereco.Rua
                    },
                    Latitude = t.Latitude,
                    Longitude = t.Longitude,
                    Numero = t.Numero
                };

                empresa.EnderecoEmpresas.Add(enderecoEmpresa);
            });

            _empresaApplication.AddAsync(empresa);
        }
    }
}
