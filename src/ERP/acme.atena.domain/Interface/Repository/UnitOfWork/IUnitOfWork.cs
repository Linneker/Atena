using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Repository.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        bool Commit();

        Task<bool> CommitAsync();
    }
}
