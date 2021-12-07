using acme.atena.application.Interface;
using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application
{
    public class ApplicationBase<TEntity> : IApplicationBase<TEntity> where TEntity : AbstractEntity
    {
        private readonly IServiceBase<TEntity> _serviceBase;

        public ApplicationBase(IServiceBase<TEntity> repositoryBase)
        {
            _serviceBase = repositoryBase;
        }

        public void Add(TEntity entity)
        {
            _serviceBase.Add(entity);
        }

        public void AddAsync(TEntity entity)
        {
             _serviceBase.AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _serviceBase.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _serviceBase.Delete(entity);
        }

        public void Delete(Guid id)
        {
            _serviceBase.Delete(id);
        }

        public void Dispose()
        {
            _serviceBase.Dispose();
        }

        public List<TEntity> GetAll()
        {
            return _serviceBase.GetAll();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _serviceBase.GetAllAsync();
        }

        public TEntity GetById(Guid id)
        {
            return _serviceBase.GetById(id);
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            return _serviceBase.GetByIdAsync(id);
        }

        public IQueryable<TEntity> GetQueryables()
        {
            return _serviceBase.GetQueryables();
        }
    }
}
