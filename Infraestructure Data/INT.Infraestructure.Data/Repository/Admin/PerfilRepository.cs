using INT.Domain.Entity;
using INT.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using TMR.Infraestructure.Data;

namespace INT.Infraestructure.Data.Repository
{
    public class PerfilRepository : RepositoryBase<Perfil>, IPerfilRepository
    {
        public PerfilRepository(DbSet<Perfil> dbSet, DbContext context) : base(dbSet, context) { }
    }
}
