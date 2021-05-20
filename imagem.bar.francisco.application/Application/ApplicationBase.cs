using imagem.bar.francisco.application.Interface;
using imagem.bar.francisco.domain.DTO;
using imagem.bar.francisco.domain.DTO.Enum;
using imagem.bar.francisco.domain.DTO.Util;
using imagem.bar.francisco.domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.application.Application
{
    public class ApplicationBase<TEntity> : IApplicationBase<TEntity> where TEntity : AbstractEntity
    {
        private readonly IServiceBase<TEntity> _serviceBase;

        public ApplicationBase(IServiceBase<TEntity> repositoryBase)
        {
            _serviceBase = repositoryBase;
        }

        public ResponseApi Add(TEntity entity, string servico)
        {
            ResponseApi responseApi = new ResponseApi();
            _serviceBase.Add(entity);
            if (entity.HasNotifications)
            {
                responseApi.Mensagem = $"PROBLEMA AO CADASTRADAR {servico}!";
                responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                responseApi.Notifications = entity.Notifications.ToList();
            }
            else
            {
                Notification notificacao = Commit();
                if (notificacao.Key == ((int)EnumCodigoMensagem.SUCESSO).ToString())
                {
                    responseApi.Mensagem = $"{servico} CADASTRADO COM SUCESSO!";
                    responseApi.ResponseHttp = EnumResponseHttp.SUCESSO;
                    responseApi.Notifications = entity.Notifications.ToList();
                }
                else
                {
                    responseApi.Mensagem = $"PROBLEMA AO CADASTRAR {servico}!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = new List<Notification>();
                    responseApi.Notifications.Add(notificacao);
                }
            }
            return responseApi;
        }

        public async Task<ResponseApi> AddAsync(TEntity entity, string servico)
        {
            await _serviceBase.AddAsync(entity);
            ResponseApi responseApi = new ResponseApi();
            if (entity.HasNotifications)
            {
                responseApi.Mensagem = $"PROBLEMA AO CADASTRAR {servico}!";
                responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                responseApi.Notifications = entity.Notifications.ToList();
            }
            else
            {
                Notification notificacao = await CommitAsync();
                if (notificacao.Key == ((int)EnumCodigoMensagem.SUCESSO).ToString())
                {
                    responseApi.Mensagem = $"{servico} CADASTRADO COM SUCESSO!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = entity.Notifications.ToList();
                }
                else
                {
                    responseApi.Mensagem = $"PROBLEMA AO CADASTRAR {servico}!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = new List<Notification>();
                    responseApi.Notifications.Add(notificacao);
                }
            }
            return responseApi;
        }
        public ResponseApi Update(TEntity entity, string servico)
        {
            _serviceBase.Update(entity);
            ResponseApi responseApi = new ResponseApi();
            if (entity.HasNotifications)
            {
                responseApi.Mensagem = $"PROBLEMA AO ATUALIZAR {servico}!";
                responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                responseApi.Notifications = entity.Notifications.ToList();
            }
            else
            {
                Notification notificacao = Commit();
                if (notificacao.Key == ((int)EnumCodigoMensagem.SUCESSO).ToString())
                {
                    responseApi.Mensagem = $"{servico} ATUALIZADO COM SUCESSO!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = entity.Notifications.ToList();
                }
                else
                {
                    responseApi.Mensagem = $"PROBLEMA AO ATUALIZAR {servico}!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = new List<Notification>();
                    responseApi.Notifications.Add(notificacao);
                }
            }
            return responseApi;
        }
        public Notification Commit()
        {
            return _serviceBase.Commit();
        }

        public Task<Notification> CommitAsync()
        {
            return _serviceBase.CommitAsync();
        }

        public ResponseApi Delete(TEntity entity, string servico)
        {
            _serviceBase.Delete(entity);
            ResponseApi responseApi = new ResponseApi();
            if (entity.HasNotifications)
            {
                responseApi.Mensagem = $"PROBLEMA AO REMOVER {servico}!";
                responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                responseApi.Notifications = entity.Notifications.ToList();
            }
            else
            {
                Notification notificacao = Commit();
                if (notificacao.Key == ((int)EnumCodigoMensagem.SUCESSO).ToString())
                {
                    responseApi.Mensagem = $"{servico} REMOVIDO COM SUCESSO!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = entity.Notifications.ToList();
                }
                else
                {
                    responseApi.Mensagem = $"PROBLEMA AO REMOVER {servico}!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = new List<Notification>();
                    responseApi.Notifications.Add(notificacao);
                }
            }
            return responseApi;

        }

        public ResponseApi Delete(Guid id, string servico)
        {
            var entity = _serviceBase.Delete(id);
            ResponseApi responseApi = new ResponseApi();
            if (entity.HasNotifications)
            {
                responseApi.Mensagem = $"PROBLEMA AO REMOVER {servico}!";
                responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                responseApi.Notifications = entity.Notifications.ToList();
            }
            else
            {
                Notification notificacao = Commit();
                if (notificacao.Key == ((int)EnumCodigoMensagem.SUCESSO).ToString())
                {
                    responseApi.Mensagem = $"{servico} REMOVIDO COM SUCESSO!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = entity.Notifications.ToList();
                }
                else
                {
                    responseApi.Mensagem = $"PROBLEMA AO REMOVER {servico}!";
                    responseApi.ResponseHttp = EnumResponseHttp.ERRO_SELECAO;
                    responseApi.Notifications = new List<Notification>();
                    responseApi.Notifications.Add(notificacao);
                }
            }
            return responseApi;
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



    }
}
