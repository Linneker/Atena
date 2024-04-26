using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Repository
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : AbstractEntity
    {
        void Add(TEntity entity);
        void Update (TEntity entity);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Delete(Guid id);
        
        TEntity GetById(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetQueryables();

        IUnitOfWork UnitOfWork { get; }
    }
}
