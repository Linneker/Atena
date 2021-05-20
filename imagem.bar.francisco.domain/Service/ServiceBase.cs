using imagem.bar.francisco.domain.DTO;
using imagem.bar.francisco.domain.DTO.Util;
using imagem.bar.francisco.domain.Interface.Repository;
using imagem.bar.francisco.domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.domain.Service
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

        public TEntity Update(TEntity entity)
        {
            return _repositoryBase.Update(entity);
        }


    }
}
