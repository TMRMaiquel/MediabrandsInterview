using INT.Domain.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Application.Service
{
    public interface IUsuarioService
    {
        Task<dynamic> GetDependencies(int Id);
        Task<Usuario> Insert(JObject objectJSON);
        Task<Usuario> Update(JObject objectJSON);
        Task<bool> Delete(JObject objectJSON);
        Task<List<Usuario>> GetByParameters(JObject objectJSON);
    }
}
