using imagem.bar.francisco.domain.DTO;
using imagem.bar.francisco.domain.DTO.Enum;
using imagem.bar.francisco.domain.DTO.Util;
using imagem.bar.francisco.domain.Interface.Repository;
using imagem.bar.francisco.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : AbstractEntity
    {
        protected internal readonly Context _db;
        public RepositoryBase(Context db)
        {
            _db = db;
        }

        public TEntity Add(TEntity entity)
        {
            try
            {
                _db.Add(entity);
            }
            catch (Exception e)
            {
                entity.AddNotification($"{(int)EnumCodigoMensagem.ERRO_ADD}", $"Mensagem: {e.Message}, InnerException: {e.InnerException}");
            }
            return entity;

        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                await _db.AddAsync(entity);
            }
            catch (Exception e)
            {
                entity.AddNotification($"{(int)EnumCodigoMensagem.ERRO_ADD_ASYNC}", $"Mensagem: {e.Message}, InnerException: {e.InnerException}");
            }
            return entity;
        }

        public Notification Commit()
        {
            Notification notification;
            try
            {
                bool salvo = _db.SaveChanges() > 0;
                notification = new Notification($"{(int)EnumCodigoMensagem.SUCESSO}", "CADASTRADO COM SUCESSO!");
            }
            catch (Exception e)
            {
                notification = new Notification($"{(int)EnumCodigoMensagem.ERRO_COMMIT}", $"Mensagem: {e.Message}, InnerException: {e.InnerException}");
            }
            return notification;
        }

        public async Task<Notification> CommitAsync()
        {
            Notification notification;
            try
            {
                bool salvo = await _db.SaveChangesAsync() > 0;
                notification = new Notification(salvo ? $"{(int)EnumCodigoMensagem.SUCESSO}" : $"{(int)EnumCodigoMensagem.ERRO_COMMIT_ASYNC}", salvo ? "CADASTRADO COM SUCESSO!" : "ERRO AO SALVAR!");
            }
            catch (Exception e)
            {
                notification = new Notification($"{(int)EnumCodigoMensagem.ERRO_COMMIT_ASYNC}", $"Mensagem: {e.Message}, InnerException: {e.InnerException}");
            }
            return notification;
        }

        public TEntity Delete(TEntity entity)
        {
            try
            {
                _db.Entry(entity).State = EntityState.Deleted;
            }
            catch (Exception e)
            {
                entity.AddNotification($"{(int)EnumCodigoMensagem.ERRO_COMMIT_ASYNC}", $"Mensagem: {e.Message}, InnerException: {e.InnerException}");
            }
            return entity;
        }

        public TEntity Delete(Guid id)
        {
            TEntity entity = GetById(id);
            try
            {
                _db.Entry(entity).State = EntityState.Deleted;
            }
            catch (Exception e)
            {
                entity.AddNotification($"{(int)EnumCodigoMensagem.ERRO_COMMIT_ASYNC}", $"Mensagem: {e.Message}, InnerException: {e.InnerException}");
            }
            return entity;
        }


        public void Dispose() => GC.Collect();


        public List<TEntity> GetAll() => _db.Set<TEntity>().ToList();

        public Task<List<TEntity>> GetAllAsync() => _db.Set<TEntity>().ToListAsync();

        public TEntity GetById(Guid id) => _db.Set<TEntity>().AsNoTracking().Where(t => t.Id == id).FirstOrDefault();


        public Task<TEntity> GetByIdAsync(Guid id) => _db.Set<TEntity>().AsNoTracking().Where(t => t.Id == id).FirstOrDefaultAsync();
        
        public TEntity Update(TEntity entity)
        {
            try
            {
                _db.Set<TEntity>().Update(entity);
            }
            catch (Exception e)
            {
                entity.AddNotification($"{(int)EnumCodigoMensagem.ERRO_UPDATE}", $"Mensagem: {e.Message}, InnerException: {e.InnerException}");
            }
            return entity;
        }

    }
}
