using INT.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Infraestructure.Data
{
    public class UnitOfWorkBase : IUnitOfWorkBase
    {
        #region Constructor

        public UnitOfWorkBase()
        {

        }

        #endregion

        #region Miembros

        internal DbContext contextBase;

        #endregion

        #region Métodos

        public async Task<int> SaveChangesAsync()
        {
            return await this.contextBase.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return this.contextBase.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            contextBase.Dispose();
        }

        public IDatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(this.contextBase);
        }

        #endregion

    }
}
