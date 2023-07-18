using INT.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Infraestructure.Data.Context
{
    public class AdminContext : DbContext
    {
        public AdminContext()
        {
            Database.EnsureCreated();
        }

        //TABLES
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeePosition> EmployeePosition { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TABLES
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeePositionConfiguration());
            modelBuilder.ApplyConfiguration(new OfficeConfiguration());
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new PerfilConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioPerfilConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=InterviewDB;Integrated Security=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
