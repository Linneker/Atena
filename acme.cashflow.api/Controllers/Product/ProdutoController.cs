﻿using acme.cashflow.api.ViewModel.Product;
using acme.cashflow.application.Interface.Product;
using acme.cashflow.domain.DTO.Product;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.cashflow.api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : BaseApiController<Produto, ProdutoViewModel>
    {
        private readonly IProdutoApplication _produtoApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "PRODUTO";

        public ProdutoController(IProdutoApplication produtoApplication, IMapper mapper):base(mapper,produtoApplication,NOME_SERVICO)
        {
            _produtoApplication = produtoApplication;
            _mapper = mapper;
        }
    }
}
