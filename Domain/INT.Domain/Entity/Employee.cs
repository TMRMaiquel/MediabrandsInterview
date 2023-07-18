using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Domain.Entity
{
    public class Employee
    {
        #region Constructor

        public Employee()
        {
            this.Office = new Office();
            this.ListEmployeePosition = new HashSet<EmployeePosition>();
        }

        #endregion

        #region Miembros

        public int Id { get; set; }
        public int IdOffice { get; set; }
        public string Name { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public bool State { get; set; }

        //PROPIEDADES DE NAVEGACION
        public Office Office { get; set; }
        public ICollection<EmployeePosition> ListEmployeePosition { get; set; }

        #endregion
    }
}
