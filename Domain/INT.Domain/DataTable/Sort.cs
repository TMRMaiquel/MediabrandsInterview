using System;
using System.Collections.Generic;
using System.Text;

namespace INT.Domain.DataTable
{
    public enum OptionSort
    {
        None,
        IsAsc,
        IsDesc,
    }

    public class Sort
    {
        #region Constructor

        public Sort()
        {
            this.Name = null;
            this.Type = OptionSort.None;
        }

        #endregion

        #region Miembros

        public string Name { get; set; }

        public OptionSort Type { get; set; }

        #endregion
    }
}
