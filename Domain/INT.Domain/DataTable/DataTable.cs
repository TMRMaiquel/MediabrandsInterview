using System;
using System.Collections.Generic;
using System.Text;

namespace INT.Domain.DataTable
{
    public class DataTable
    {
        #region Constructor

        public DataTable() {

            this.Filters = new List<Filter>();
            this.Sort = new Sort();
            this.Pagination = new Pagination();
            this.WaitValidationExt = false;        }

        #endregion

        #region Miembros

        public List<Filter> Filters { get; set; }

        public Sort Sort { get; set; }

        public Pagination Pagination { get; set; }

        //Indica si el DataTable espera alguna validación externa
        //antes de traer los registros: (0) No espera validación y (1) Espera validación.
        public bool WaitValidationExt { get; set; }

        #endregion
    }
}
