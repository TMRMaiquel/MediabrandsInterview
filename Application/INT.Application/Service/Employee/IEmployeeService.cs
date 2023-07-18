using INT.Domain.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Application.Service
{
    public interface IEmployeeService
    {
        Task<dynamic> GetDependencies(int Id);
        Task<Employee> Insert(JObject objectJSON);
        Task<Employee> Update(JObject objectJSON);
        Task<bool> Delete(JObject objectJSON);
        Task<List<Employee>> GetByParameters(JObject objectJSON);
    }
}
