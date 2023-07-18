using INT.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Infraestructure.Data.Context
{
    public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
    {
        #region Metodos

        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            ConfigTable(builder);
            ConfigRelationship(builder);
        }

        private void ConfigTable(EntityTypeBuilder<Perfil> builder)
        {
            try
            {
                builder.ToTable("ADM_Perfil");
                builder.Property(p => p.Id).HasColumnName("PERF_Id").IsRequired();
                builder.Property(p => p.Nombre).HasColumnName("PERF_Nombre").HasMaxLength(100).IsRequired();
                builder.Property(p => p.State).HasColumnName("PERF_State").HasColumnType("bit").IsRequired();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigRelationship(EntityTypeBuilder<Perfil> builder)
        {
            try
            {
                builder.HasKey(k => k.Id);
                builder.HasMany<UsuarioPerfil>(r => r.ListaUsuarioPerfil)
                    .WithOne(r => r.Perfil)
                    .HasForeignKey(r => r.IdPerfil);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
