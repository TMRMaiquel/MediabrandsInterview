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
    public class PositionRepository : RepositoryBase<Position>, IPositionRepository
    {
        public PositionRepository(DbSet<Position> dbSet, DbContext context) : base(dbSet, context) { }
    }
}
