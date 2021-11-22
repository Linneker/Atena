﻿using acme.cashflow.domain.DTO;
using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.domain.Interface.Repository
{
    public interface IRepositoryBase<TEntity> where TEntity : AbstractEntity
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Delete(TEntity entity);
        TEntity Delete(Guid id);
        
        TEntity GetById(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();

        void Dispose();
        Notification Commit();
        Task<Notification> CommitAsync();

    }
}