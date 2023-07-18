using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Domain.Entity
{
    public class Office
    {
        #region Constructor

        public Office()
        {
            this.ListEmployee = new HashSet<Employee>();
        }

        #endregion

        #region Miembros

        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }

        //PROPIEDADES DE NAVEGACION
        public ICollection<Employee> ListEmployee { get; set; }

        #endregion
    }
}
