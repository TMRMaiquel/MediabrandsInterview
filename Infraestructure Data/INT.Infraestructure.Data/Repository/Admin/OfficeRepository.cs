using INT.Domain.Entity;
using INT.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMR.Infraestructure.Data;

namespace INT.Infraestructure.Data.Repository
{
    public class OfficeRepository : RepositoryBase<Office>, IOfficeRepository
    {
        public OfficeRepository(DbSet<Office> dbSet, DbContext context) : base(dbSet, context) { }
    }
}
