using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Service
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : AbstractEntity
    {
        private readonly IRepositoryBase<TEntity> _repositoryBase;

        public ServiceBase(IRepositoryBase<TEntity> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public virtual void Add(TEntity entity)
        {
            if (entity.IsValid())
            {
                _repositoryBase.Add(entity);
            }
        }

        public virtual Task AddAsync(TEntity entity)
        {
            if (entity.IsValid())
            {
                return _repositoryBase.AddAsync(entity);
            }
            throw new Exception("Dados invalidos!");
        }


        public void Delete(TEntity entity)
        {
            _repositoryBase.Delete(entity);
        }

        public void Delete(Guid id)
        {
            _repositoryBase.Delete(id);
        }

        public void Dispose()
        {
            _repositoryBase.Dispose();
        }

        public List<TEntity> GetAll()
        {
            return _repositoryBase.GetAll();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _repositoryBase.GetAllAsync();
        }

        public TEntity GetById(Guid id)
        {
            return _repositoryBase.GetById(id);
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            return _repositoryBase.GetByIdAsync(id);
        }

        public IQueryable<TEntity> GetQueryables()
        {
            return _repositoryBase.GetQueryables();
        }

        public void Update(TEntity entity)
        {
            _repositoryBase.Update(entity);
        }


    }
}
