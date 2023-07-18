using INT.Domain.DataTable;
using System.Linq.Expressions;
using System.Text;

namespace INT.Infraestructure.Data.Common
{
    public static class MethodsCommon
    {
        public static IOrderedQueryable<TEntityCustom> CreateSort<TEntityCustom, TPropertyType>(this IQueryable<TEntityCustom> collection, Sort sort)
        {
            try
            {
                if (string.IsNullOrEmpty(sort.Name)) { return collection.OrderBy(x => 1); }

                IOrderedQueryable<TEntityCustom> sortedlist = null;

                ParameterExpression pe = Expression.Parameter(typeof(TEntityCustom), "t");

                Expression bodyExpression = pe;

                if (!sort.Name.Contains("."))
                {
                    bodyExpression = Expression.Property(pe, sort.Name);
                }
                else
                {
                    foreach (var property in sort.Name.Split('.'))
                    {
                        bodyExpression = Expression.PropertyOrField(bodyExpression, property);
                    }
                }

                Expression<Func<TEntityCustom, TPropertyType>> expr = Expression.Lambda<Func<TEntityCustom, TPropertyType>>(Expression.Convert(bodyExpression, typeof(TPropertyType)), pe);

                if (sort.Type == OptionSort.IsDesc)
                    sortedlist = collection.OrderByDescending<TEntityCustom, TPropertyType>(expr);
                else if (sort.Type == OptionSort.IsAsc)
                    sortedlist = collection.OrderBy<TEntityCustom, TPropertyType>(expr);
                else if (sort.Type == OptionSort.None)
                    sortedlist = collection.OrderBy(x => 1);

                return sortedlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RemoveSignsAccents(this string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) { return text; }

                string withSigns = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜÑçÇ";
                string withoutSigns = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUNcC";
                StringBuilder textWithoutAccents = new StringBuilder(text.Length);
                int indexWithAccents;

                foreach (char character in text)
                {
                    indexWithAccents = withSigns.IndexOf(character);
                    if (indexWithAccents > -1)
                        textWithoutAccents.Append(withoutSigns.Substring(indexWithAccents, 1));
                    else
                        textWithoutAccents.Append(character);
                }
                return textWithoutAccents.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsDate(object strDate)
        {
            try
            {
                if (strDate is string)
                {
                    if (DateTime.TryParse(strDate.ToString(), out DateTime Temp) == true)
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
