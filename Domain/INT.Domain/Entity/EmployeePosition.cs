using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Domain.Entity
{
    public class EmployeePosition
    {
        #region Constructor

        public EmployeePosition()
        {
            this.Employee = new Employee();
            this.Position = new Position();
        }

        #endregion

        #region Miembros

        public int IdEmployee { get; set; }
        public int IdPosition { get; set; }
        public bool State { get; set; }

        //PROPIEDADES DE NAVEGACION
        public Employee Employee { get; set; }
        public Position Position { get; set; }

        #endregion
    }
}
