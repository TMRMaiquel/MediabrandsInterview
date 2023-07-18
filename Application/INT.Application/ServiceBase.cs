using INT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Application
{
    public class ServiceBase : IServiceBase
    {
        public IUnitOfWorkAdmin UnitOfWork { get; set; }
    }
}
