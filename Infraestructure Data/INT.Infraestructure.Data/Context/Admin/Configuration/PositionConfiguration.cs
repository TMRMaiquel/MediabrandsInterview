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
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        #region Metodos

        public void Configure(EntityTypeBuilder<Position> builder)
        {
            ConfigTable(builder);
            ConfigRelationship(builder);
        }

        private void ConfigTable(EntityTypeBuilder<Position> builder)
        {
            try
            {
                builder.ToTable("ADM_Position");
                builder.Property(p => p.Id).HasColumnName("POSI_Id").IsRequired();
                builder.Property(p => p.Name).HasColumnName("POSI_Name").HasMaxLength(100).IsRequired();
                builder.Property(p => p.State).HasColumnName("POSI_State").HasColumnType("bit").IsRequired();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigRelationship(EntityTypeBuilder<Position> builder)
        {
            try
            {
                builder.HasKey(k => k.Id);
                builder.HasMany<EmployeePosition>(r => r.ListEmployeePosition)
                    .WithOne(r => r.Position)
                    .HasForeignKey(r => r.IdPosition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
