using INT.Domain.Entity;
using INT.Domain.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using TMR.Infraestructure.Data;

namespace INT.Infraestructure.Data.Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbSet<Employee> dbSet, DbContext context) : base(dbSet, context) { }

        public async Task InsertBySP(Employee employee)
        {
            try
            {
                var cnx = this.context.Database.GetDbConnection();

                if (cnx.State == ConnectionState.Closed)
                {
                    await cnx.OpenAsync();
                }

                var parameters = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@EMPY_Id",
                            SqlDbType =  SqlDbType.Int,
                            Direction = ParameterDirection.Output,
                            Value = null
                        },
                        new SqlParameter() {
                            ParameterName = "@OFFC_Id",
                            SqlDbType =  SqlDbType.Int,
                            Direction = ParameterDirection.Input,
                            Value = employee.IdOffice
                        },
                        new SqlParameter() {
                            ParameterName = "@EMPY_Name",
                            SqlDbType =  SqlDbType.VarChar,
                            Size = 100,
                            Direction = ParameterDirection.Input,
                            Value = employee.Name
                        },
                        new SqlParameter() {
                            ParameterName = "@EMPY_FirstLastName",
                            SqlDbType =  SqlDbType.VarChar,
                            Size = 100,
                            Direction = ParameterDirection.Input,
                            Value = employee.FirstLastName
                        },
                        new SqlParameter() {
                            ParameterName = "@EMPY_SecondLastName",
                            SqlDbType =  SqlDbType.VarChar,
                            Size = 100,
                            Direction = ParameterDirection.Input,
                            Value = employee.SecondLastName
                        },
                        new SqlParameter() {
                            ParameterName = "@EMPY_Address",
                            SqlDbType =  SqlDbType.VarChar,
                            Size = 500,
                            Direction = ParameterDirection.Input,
                            Value = employee.Address
                        },
                        new SqlParameter() {
                            ParameterName = "@EMPY_BithDate",
                            SqlDbType =  SqlDbType.Date,
                            Direction = ParameterDirection.Input,
                            Value = employee.BirthDate
                        },
                        new SqlParameter() {
                            ParameterName = "@EMPY_HireDate",
                            SqlDbType =  SqlDbType.Date,
                            Direction = ParameterDirection.Input,
                            Value = employee.HireDate
                        },
                        new SqlParameter() {
                            ParameterName = "@EMPY_Phone",
                            SqlDbType =  SqlDbType.VarChar,
                            Size = 20,
                            Direction = ParameterDirection.Input,
                            Value = employee.Phone
                        },
                        new SqlParameter() {
                            ParameterName = "@EMPY_Note",
                            SqlDbType =  SqlDbType.Text,
                            Direction = ParameterDirection.Input,
                            Value = employee.Note
                        },
                };

                var cmn = cnx.CreateCommand();

                cmn.Transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                cmn.CommandText = "[dbo].[SP_EMPY_Insert]";
                cmn.CommandType = CommandType.StoredProcedure;
                cmn.Parameters.AddRange(parameters);

                await cmn.ExecuteNonQueryAsync();

                employee.Id = (int)cmn.Parameters["@EMPY_Id"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task UpdateBySP(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBySP(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Employee>> GetByParameters(dynamic filterToSearch)
        {
            try
            {
                ICollection<Employee> listEmployee = new HashSet<Employee>();

                var cnx = this.context.Database.GetDbConnection();

                if (cnx.State == ConnectionState.Closed)
                {
                    await cnx.OpenAsync();
                }

                var cmn = cnx.CreateCommand();

                var parameters = new SqlParameter[] {
                     new SqlParameter() {
                            ParameterName = "@EMPY_Name",
                            SqlDbType =  SqlDbType.VarChar,
                            Size = 100,
                            Direction = ParameterDirection.Input,
                            Value = filterToSearch.name
                        }};

                cmn.Transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                cmn.CommandText = "[dbo].[SP_EMPY_GetByParameters]";
                cmn.CommandType = CommandType.StoredProcedure;
                cmn.Parameters.AddRange(parameters);

                using (var reader = await cmn.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var employee = new Employee()
                        {
                            //IdSaleOrder = Convert.ToInt32(reader["IdSaleOrder"]),
                            //IdCharge = Convert.ToInt32(reader["IdCharge"]),
                            //IdCollege = Convert.ToInt32(reader["IdCollege"]),
                            //Status = Convert.ToBoolean(reader["Status"]),
                            //UserCreation = Convert.ToString(reader["UserCreation"]),
                            //DateCreation = Convert.ToDateTime(reader["DateCreation"]),
                            Name = reader["Name"] != DBNull.Value ? Convert.ToString(reader["name"]) : string.Empty,
                            BirthDate = (DateTime)reader["BirthDate"],
                            //College = new College()
                            //{
                            //    IdCollege = Convert.ToInt32(reader["IdCollege"]),
                            //    CAP = Convert.ToString(reader["CAP"]),
                            //    FullName = Convert.ToString(reader["FullName"])
                            //}
                        };

                        listEmployee.Add(employee);
                    }
                }

                return listEmployee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
