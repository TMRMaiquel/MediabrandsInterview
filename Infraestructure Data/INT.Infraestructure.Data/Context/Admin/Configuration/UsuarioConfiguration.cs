using INT.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace INT.Infraestructure.Data.Context
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        #region Metodos

        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            ConfigTable(builder);
            ConfigRelationship(builder);
        }

        private void ConfigTable(EntityTypeBuilder<Usuario> builder)
        {
            try
            {
                builder.ToTable("ADM_Usuario");
                builder.Property(p => p.Id).HasColumnName("USUA_Id").IsRequired();
                builder.Property(p => p.Nombre).HasColumnName("USUA_Nombre").HasMaxLength(100).IsRequired();
                builder.Property(p => p.ApellidoPaterno).HasColumnName("USUA_ApellidoPaterno").HasMaxLength(100).IsRequired();
                builder.Property(p => p.ApellidoMaterno).HasColumnName("USUA_ApellidoMaterno").HasMaxLength(100).IsRequired();
                builder.Property(p => p.State).HasColumnName("USUA_State").HasColumnType("bit").IsRequired();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigRelationship(EntityTypeBuilder<Usuario> builder)
        {
            try
            {
                builder.HasKey(k => k.Id);
                builder.HasMany<UsuarioPerfil>(r => r.ListaUsuarioPerfil)
                    .WithOne(r => r.Usuario)
                    .HasForeignKey(r => r.IdUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
