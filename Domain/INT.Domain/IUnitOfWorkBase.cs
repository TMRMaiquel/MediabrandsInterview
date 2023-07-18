using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INT.Domain
{
    public interface IUnitOfWorkBase: IDisposable
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IDatabaseTransaction BeginTransaction();
    }
}
