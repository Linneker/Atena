using acme.cashflow.domain.DTO;
using acme.cashflow.domain.DTO.Util;
using acme.cashflow.domain.Interface.Repository;
using acme.cashflow.domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.domain.Service
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : AbstractEntity
    {
        private readonly IRepositoryBase<TEntity> _repositoryBase;

        public ServiceBase(IRepositoryBase<TEntity> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public TEntity Add(TEntity entity)
        {
            if (entity.IsValid())
            {
                return _repositoryBase.Add(entity);
            }
            else
            {
                return entity;
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity.IsValid())
            {
                return await _repositoryBase.AddAsync(entity);
            }
            else
            {
                return entity;
            }
        }

        public Notification Commit()
        {
            return _repositoryBase.Commit();
        }

        public Task<Notification> CommitAsync()
        {
            return _repositoryBase.CommitAsync();
        }

        public TEntity Delete(TEntity entity)
        {
            return _repositoryBase.Delete(entity);
        }

        public TEntity Delete(Guid id)
        {
            return _repositoryBase.Delete(id);
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

        public TEntity Update(TEntity entity)
        {
            return _repositoryBase.Update(entity);
        }


    }
}
