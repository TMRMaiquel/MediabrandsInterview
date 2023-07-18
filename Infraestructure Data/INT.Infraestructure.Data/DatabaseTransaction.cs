using INT.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace INT.Infraestructure.Data
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly IDbContextTransaction transaction;

        public DatabaseTransaction(DbContext context)
        {
            this.transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        public void Dispose()
        {
            this.transaction.Dispose();
        }

        public void Rollback()
        {
            this.transaction.Rollback();
        }
    }
}
