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
    public class EmployeePositionConfiguration : IEntityTypeConfiguration<EmployeePosition>
    {
        public void Configure(EntityTypeBuilder<EmployeePosition> builder)
        {
            ConfigTable(builder);
            ConfigRelationship(builder);
        }

        private void ConfigTable(EntityTypeBuilder<EmployeePosition> builder)
        {
            try
            {
                builder.ToTable("ADM_Employee_Position");
                builder.Property(p => p.IdEmployee).HasColumnName("EMPY_Id").IsRequired();
                builder.Property(p => p.IdPosition).HasColumnName("POSI_Id").IsRequired();
                builder.Property(p => p.State).HasColumnName("EMPS_State").HasColumnType("bit").IsRequired();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigRelationship(EntityTypeBuilder<EmployeePosition> builder)
        {
            try
            {
                builder.HasKey(k => new { k.IdEmployee, k.IdPosition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
