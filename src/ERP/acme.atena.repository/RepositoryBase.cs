using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : AbstractEntity
    {
        protected internal readonly Context _db;
        public RepositoryBase(Context db)
        {
            _db = db;
        }

        public void Add(TEntity entity)
        {
            try
            {
                _db.Add(entity);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void AddAsync(TEntity entity)
        {
            try
            {
                _db.AddAsync(entity);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void Delete(TEntity entity)
        {
            try
            {
                _db.Entry(entity).State = EntityState.Deleted;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void Delete(Guid id)
        {
            TEntity entity = GetById(id);
            try
            {
                _db.Entry(entity).State = EntityState.Deleted;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public void Dispose() => GC.Collect();


        public List<TEntity> GetAll() => _db.Set<TEntity>().ToList();
        public IQueryable<TEntity> GetQueryables() => _db.Set<TEntity>().AsNoTracking().AsQueryable();


        public Task<List<TEntity>> GetAllAsync() => _db.Set<TEntity>().ToListAsync();

        public TEntity GetById(Guid id) => _db.Set<TEntity>().AsNoTracking().Where(t => t.Id == id).FirstOrDefault();


        public Task<TEntity> GetByIdAsync(Guid id) => _db.Set<TEntity>().AsNoTracking().Where(t => t.Id == id).FirstOrDefaultAsync();
        
        public void Update(TEntity entity)
        {
            try
            {
                _db.Set<TEntity>().Update(entity);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
