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
    public class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        #region Metodos

        public void Configure(EntityTypeBuilder<Office> builder)
        {
            ConfigTable(builder);
            ConfigRelationship(builder);
        }

        private void ConfigTable(EntityTypeBuilder<Office> builder)
        {
            try
            {
                builder.ToTable("ADM_Office");
                builder.Property(p => p.Id).HasColumnName("OFFC_Id").IsRequired();
                builder.Property(p => p.Name).HasColumnName("OFFC_Name").HasMaxLength(100).IsRequired();
                builder.Property(p => p.State).HasColumnName("OFFC_State").HasColumnType("bit").IsRequired();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigRelationship(EntityTypeBuilder<Office> builder)
        {
            try
            {
                builder.HasKey(k => k.Id);
                builder.HasMany<Employee>(r => r.ListEmployee)
                    .WithOne(r => r.Office)
                    .HasForeignKey(r => r.IdOffice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
