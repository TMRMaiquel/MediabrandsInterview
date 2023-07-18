using INT.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace INT.Infraestructure.Data.Context
{
    public class UsuarioPerfilConfiguration : IEntityTypeConfiguration<UsuarioPerfil>
    {
        public void Configure(EntityTypeBuilder<UsuarioPerfil> builder)
        {
            ConfigTable(builder);
            ConfigRelationship(builder);
        }

        private void ConfigTable(EntityTypeBuilder<UsuarioPerfil> builder)
        {
            try
            {
                builder.ToTable("ADM_UsuarioPerfil");
                builder.Property(p => p.IdUsuario).HasColumnName("USUA_Id").IsRequired();
                builder.Property(p => p.IdPerfil).HasColumnName("PERF_Id").IsRequired();
                builder.Property(p => p.State).HasColumnName("PERF_State").HasColumnType("bit").IsRequired();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigRelationship(EntityTypeBuilder<UsuarioPerfil> builder)
        {
            try
            {
                builder.HasKey(k => new { k.IdUsuario, k.IdPerfil });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
