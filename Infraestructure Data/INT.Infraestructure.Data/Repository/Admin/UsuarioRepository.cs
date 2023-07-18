using INT.Domain.Entity;
using INT.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using TMR.Infraestructure.Data;

namespace INT.Infraestructure.Data.Repository
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(DbSet<Usuario> dbSet, DbContext context) : base(dbSet, context) { }
    }
}
