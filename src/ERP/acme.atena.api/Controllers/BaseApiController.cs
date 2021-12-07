﻿using AutoMapper;
using acme.atena.api.ViewModel;
using acme.atena.api.ViewModel.Util;
using acme.atena.application.Interface;
using acme.atena.domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acme.atena.api.Configurations.Filtler;

namespace acme.atena.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController<TEntity, TEntityViewModel> : ControllerBase where TEntity : AbstractEntity where TEntityViewModel : AbstractEntityViewModel
    {
        private readonly IMapper _mapper;
        private IApplicationBase<TEntity> _applicationBase;
        public BaseApiController(IMapper mapper, IApplicationBase<TEntity> applicationBase, string servico)
        {
            _mapper = mapper;
            _applicationBase = applicationBase;
        }
        [Authorize("Bearer")]
        [UnitOfWorkFilter]
        [HttpPost("Add")]
        public virtual void Add(TEntityViewModel entity)
        {
            _applicationBase.Add(_mapper.Map<TEntity>(entity));
        }
        [Authorize("Bearer")]
        [UnitOfWorkFilterAsync]
        [HttpPost("Add/Async")]
        public virtual void AddAsync(TEntityViewModel entity)
        {
            _applicationBase.AddAsync(_mapper.Map<TEntity>(entity));
        }
        [Authorize("Bearer")]
        [UnitOfWorkFilter]
        [HttpDelete("Remove/{id}")]
        public virtual void Delete(Guid id)
        {
            _applicationBase.Delete(id);
        }
        private void Dispose() => _applicationBase.Dispose();

        [Authorize("Bearer")]
        [HttpGet]
        public virtual List<TEntityViewModel> GetAll() => _mapper.Map<List<TEntityViewModel>>(_applicationBase.GetAll());

        [Authorize("Bearer")]
        [HttpGet("GetAll/Async")]
        public Task<List<TEntityViewModel>> GetAllAsync() => _mapper.Map<Task<List<TEntityViewModel>>>(_applicationBase.GetAllAsync());

        [Authorize("Bearer")]
        [HttpGet("GetById/{id}")]
        public virtual TEntityViewModel GetById(Guid id) => _mapper.Map<TEntityViewModel>(_applicationBase.GetById(id));

        [Authorize("Bearer")]
        [HttpGet("GetById/Async/{id}")]
        public virtual Task<TEntityViewModel> GetByIdAsync(Guid id) => _mapper.Map<Task<TEntityViewModel>>(_applicationBase.GetByIdAsync(id));

        [Authorize("Bearer")]
        [HttpGet("GetQueryables")]
        public virtual IQueryable<TEntityViewModel> GetQueryables(Guid id) => _mapper.Map<IQueryable<TEntityViewModel>>(_applicationBase.GetQueryables());

        [Authorize("Bearer")]
        [UnitOfWorkFilter]
        [HttpPut]
        public virtual void Update(TEntityViewModel entity)
        {
            _applicationBase.Update(_mapper.Map<TEntity>(entity));
        }
    }
}