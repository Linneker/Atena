using acme.atena.domain.Interface.Repository.UnitOfWork;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly Context _context;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

        public bool Commit() => _context.SaveChanges() > 0;

        public async Task<bool> CommitAsync() => await _context.SaveChangesAsync() > 0;

        public void Dispose() => _context.Dispose();
    }
}
