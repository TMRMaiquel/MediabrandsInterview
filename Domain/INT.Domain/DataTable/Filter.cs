using System;
using System.Collections.Generic;
using System.Text;

namespace INT.Domain.DataTable
{
    public enum OptionFilter
    {
        None,
        Contains,
        Equals,
        NotEquals,
        StartsWith,
        EndsWith,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        NotContains,
        InRange,
        Set
    }

   public class Filter
    {
        #region Constructor

        public Filter()
        {
            this.Name = null;
            this.Value = null;
            this.Value2 = null;
            this.Values = null;
            this.Type = OptionFilter.None;
        }

        #endregion

        #region Miembros

        public string Name { get; set; }

        public object Value { get; set; }

        public object Value2 { get; set; }

        public List<object> Values { get; set; }

        public OptionFilter Type { get; set; }

        #endregion
    }
}
