using acme.atena.api.ViewModel.Product;
using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : BaseApiController<Venda,VendaViewModel>
    {
        private readonly IVendaApplication _vendaApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "VENDA";

        public VendaController(IVendaApplication vendaApplication, IMapper mapper):base(mapper,vendaApplication, NOME_SERVICO)
        {
            _vendaApplication = vendaApplication;
            _mapper = mapper;
        }
        [Authorize("Bearer")]
        [UnitOfWorkFilter]
        [HttpPut("{empresaId}", Name = "Conclusao")]
        public void Conclusao(Guid empresaId, [FromBody] VendaViewModel venda)
        {
            _vendaApplication.Conclusao(_mapper.Map<Venda>(venda), empresaId);
        }
    }
}
