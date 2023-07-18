using INT.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace INT.Infraestructure.Data.Context
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        #region Metodos

        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            ConfigTable(builder);
            ConfigRelationship(builder);
        }

        private void ConfigTable(EntityTypeBuilder<Employee> builder)
        {
            try
            {
                builder.ToTable("ADM_Employee");
                builder.Property(p => p.Id).HasColumnName("EMPY_Id").IsRequired();
                builder.Property(p => p.IdOffice).HasColumnName("OFFC_Id").IsRequired();
                builder.Property(p => p.Name).HasColumnName("EMPY_Name").HasMaxLength(100).IsRequired();
                builder.Property(p => p.FirstLastName).HasColumnName("EMPY_FirstLastName").HasMaxLength(100).IsRequired();
                builder.Property(p => p.SecondLastName).HasColumnName("EMPY_SecondLastName").HasMaxLength(100);
                builder.Property(p => p.Address).HasColumnName("EMPY_Address").HasMaxLength(500);
                builder.Property(p => p.BirthDate).HasColumnName("EMPY_BithDate").HasColumnType("date").IsRequired();
                builder.Property(p => p.HireDate).HasColumnName("EMPY_HireDate").HasColumnType("date").IsRequired();
                builder.Property(p => p.Phone).HasColumnName("EMPY_Phone").HasMaxLength(20).IsRequired();
                builder.Property(p => p.Note).HasColumnName("EMPY_Note").HasColumnType("text");
                builder.Property(p => p.State).HasColumnName("EMPY_State").HasColumnType("bit").IsRequired();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigRelationship(EntityTypeBuilder<Employee> builder)
        {
            try
            {
                builder.HasKey(k => k.Id);
                builder.HasMany<EmployeePosition>(r => r.ListEmployeePosition)
                    .WithOne(r => r.Employee)
                    .HasForeignKey(r => r.IdEmployee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
