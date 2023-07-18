using INT.Domain.Entity;
using INT.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using TMR.Infraestructure.Data;

namespace INT.Infraestructure.Data.Repository
{
    public class EmployeePositionRepository : RepositoryBase<EmployeePosition>, IEmployeePositionRepository
    {
        public EmployeePositionRepository(DbSet<EmployeePosition> dbSet, DbContext context) : base(dbSet, context) { }
    }
}
