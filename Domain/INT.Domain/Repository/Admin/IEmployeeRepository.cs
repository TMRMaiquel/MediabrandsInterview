using INT.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Domain.Repository
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task InsertBySP(Employee employee);
        Task UpdateBySP(Employee employee);
        Task DeleteBySP(int id);
        Task<ICollection<Employee>> GetByParameters(dynamic filterToSearch);
    }
}
