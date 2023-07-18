using System;
using System.Collections.Generic;
using System.Text;

namespace INT.Domain.DataTable
{
    public class Pagination
    {
        #region Constructor

        public Pagination()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        #endregion

        #region Miembros

        public int Page { get; set; }

        public int PageSize { get; set; }

        #endregion
    }
}
