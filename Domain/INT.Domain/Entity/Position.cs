using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Domain.Entity
{
    public class Position
    {
        #region Constructor

        public Position()
        {
            this.ListEmployeePosition = new HashSet<EmployeePosition>();
        }

        #endregion

        #region Miembros

        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }

        //PROPIEDADES DE NAVEGACION
        public ICollection<EmployeePosition> ListEmployeePosition { get; set; }

        #endregion
    }
}
