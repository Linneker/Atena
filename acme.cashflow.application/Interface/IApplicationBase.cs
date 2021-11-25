﻿using acme.cashflow.domain.DTO;
using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.application.Interface
{
    public interface IApplicationBase<TEntity> where TEntity : AbstractEntity 
    {
        ResponseApi Add(TEntity entity,string servico);
        ResponseApi Update(TEntity entity, string servico);
        Task<ResponseApi> AddAsync(TEntity entity, string servico);
        ResponseApi Delete(TEntity entity, string servico);
        ResponseApi Delete(Guid id, string servico);

        TEntity GetById(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetQueryables();
        void Dispose();
        Notification Commit();
        Task<Notification> CommitAsync();

    }
}
