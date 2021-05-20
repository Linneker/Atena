using AutoMapper;
using imagem.bar.francisco.api.ViewModel;
using imagem.bar.francisco.api.ViewModel.Util;
using imagem.bar.francisco.application.Interface;
using imagem.bar.francisco.domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController<TEntity, TEntityViewModel> : ControllerBase where TEntity : AbstractEntity where TEntityViewModel : AbstractEntityViewModel
    {
        private readonly IMapper _mapper;
        private IApplicationBase<TEntity> _applicationBase;
        private string _servico;
        public BaseApiController(IMapper mapper, IApplicationBase<TEntity> applicationBase, string servico)
        {
            _mapper = mapper;
            _applicationBase = applicationBase;
            _servico = servico;
        }
        [Authorize("Bearer")]
        [HttpPost("Add")]
        public virtual ResponseApiViewModel Add(TEntityViewModel entity)
        {
            ResponseApiViewModel apiViewModel = _mapper.Map<ResponseApiViewModel>(_applicationBase.Add(_mapper.Map<TEntity>(entity), _servico));
            this.HttpContext.Response.StatusCode = (int)apiViewModel.ResponseHttp;
            return apiViewModel;
        }
        [Authorize("Bearer")]
        [HttpPost("Add/Async")]
        public virtual async Task<ResponseApiViewModel> AddAsync(TEntityViewModel entity)
        {
            var apiViewModel = await _mapper.Map<Task<ResponseApiViewModel>>(_applicationBase.AddAsync(_mapper.Map<TEntity>(entity), _servico));
            this.HttpContext.Response.StatusCode = (int)apiViewModel.ResponseHttp;
            return apiViewModel;
        }
        [Authorize("Bearer")]
        [HttpDelete("Remove/{id}")]
        public virtual ResponseApiViewModel Delete(Guid id)
        {
            var apiViewModel = _mapper.Map<ResponseApiViewModel>(_applicationBase.Delete(id, _servico));
            this.HttpContext.Response.StatusCode = (int)apiViewModel.ResponseHttp;
            return apiViewModel;
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
        [HttpPut]
        public virtual ResponseApiViewModel Update(TEntityViewModel entity)
        {
            var apiViewModel = _mapper.Map<ResponseApiViewModel>(_applicationBase.Update(_mapper.Map<TEntity>(entity), _servico));
            this.HttpContext.Response.StatusCode = (int)apiViewModel.ResponseHttp;
            return apiViewModel;
        }
    }
}
