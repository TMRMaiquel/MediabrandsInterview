using INT.Domain;
using INT.Domain.DataTable;
using INT.Infraestructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace TMR.Infraestructure.Data
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public readonly DbContext context;
        private readonly DbSet<TEntity> dbSet;
        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });
        private static readonly MethodInfo equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(StringComparison) });
        private static readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });
        private static readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) });

        public RepositoryBase(DbSet<TEntity> dbSet, DbContext context)
        {
            this.dbSet = dbSet;
            this.context = context;
        }

        public async Task AddAsync(TEntity entityToAdd)
        {
            try
            {
                await dbSet.AddAsync(entityToAdd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddRangeAsync(ICollection<TEntity> collection)
        {
            try
            {
                await dbSet.AddRangeAsync(collection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task AddRangeAsync(ICollection<TEntity> collection, CancellationToken cancellationToken)
        {
            try
            {
                return dbSet.AddRangeAsync(collection, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AttachAsync(TEntity entityToAttach)
        {
            try
            {
                await Task.Run(() => dbSet.Attach(entityToAttach));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task AttachAsync(TEntity entityToAttach, CancellationToken cancellationToken)
        {
            try
            {
                return Task.Run(() => dbSet.Attach(entityToAttach), cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AttachRangeAsync(ICollection<TEntity> collection)
        {
            try
            {
                await Task.Run(() => dbSet.AttachRange(collection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task AttachRangeAsync(ICollection<TEntity> collection, CancellationToken cancellationToken)
        {
            try
            {
                return Task.Run(() => dbSet.AttachRange(collection), cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expression<Func<TEntityCustom, bool>> CreateFilter<TEntityCustom>(List<Filter> filters)
        {
            try
            {
                if (filters.Count == 0) { return null; }

                //Compone el árbol de expresiones y representa el parámetro del predicado (t =>)
                ParameterExpression pe = Expression.Parameter(typeof(TEntityCustom), "t");
                Expression exp = null;

                if (filters.Count == 1) { exp = this.GetExpression<TEntityCustom>(pe, filters[0]); }
                else if (filters.Count == 2) { exp = this.GetExpression<TEntityCustom>(pe, filters[0], filters[1]); }
                else
                {
                    while (filters.Count > 0)
                    {
                        var filter1 = filters[0];
                        var filter2 = filters[1];

                        if (exp == null) { exp = this.GetExpression<TEntityCustom>(pe, filter1, filter2); }
                        else { exp = Expression.AndAlso(exp, this.GetExpression<TEntityCustom>(pe, filter1, filter2)); }

                        filters.Remove(filter1);
                        filters.Remove(filter2);

                        if (filters.Count == 1)
                        {
                            exp = Expression.AndAlso(exp, this.GetExpression<TEntityCustom>(pe, filters[0]));
                            filters.RemoveAt(0);
                        }
                    }
                }

                return Expression.Lambda<Func<TEntityCustom, bool>>(exp, pe);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TEntityCustom> CreateSort<TEntityCustom, TPropertyType>(IEnumerable<TEntityCustom> collection, Sort sort)
        {
            try
            {
                if (string.IsNullOrEmpty(sort.Name)) { return collection.ToList(); }

                List<TEntityCustom> sortedlist = null;

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
                    sortedlist = collection.OrderByDescending<TEntityCustom, TPropertyType>(expr.Compile()).ToList();
                else if (sort.Type == OptionSort.IsAsc)
                    sortedlist = collection.OrderBy<TEntityCustom, TPropertyType>(expr.Compile()).ToList();
                else if (sort.Type == OptionSort.None)
                    sortedlist = collection.ToList();

                return sortedlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(object id)
        {
            try
            {
                void delete()
                {
                    TEntity entityToDelete = dbSet.Find(id);

                    PropertyInfo propertyInfo = entityToDelete.GetType().GetProperty("Status");

                    propertyInfo.SetValue(entityToDelete, Convert.ChangeType(false, propertyInfo.PropertyType), null);

                    context.Entry<TEntity>(entityToDelete).State = EntityState.Modified;
                }

                await Task.Run(() => delete());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task DeleteAsync(object id, CancellationToken cancellationToken)
        {
            try
            {
                void delete()
                {
                    TEntity entityToDelete = dbSet.Find(id);

                    PropertyInfo propertyInfo = entityToDelete.GetType().GetProperty("Status");

                    propertyInfo.SetValue(entityToDelete, Convert.ChangeType(false, propertyInfo.PropertyType), null);

                    context.Entry<TEntity>(entityToDelete).State = EntityState.Modified;
                }

                return Task.Run(() => delete(), cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(TEntity entityToDelete)
        {
            try
            {
                void delete()
                {
                    dbSet.Attach(entityToDelete);
                    dbSet.Remove(entityToDelete);
                }

                await Task.Run(() => delete());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task DeleteAsync(TEntity entityToDelete, CancellationToken cancellationToken)
        {
            try
            {
                void delete()
                {
                    dbSet.Attach(entityToDelete);
                    dbSet.Remove(entityToDelete);
                }

                return Task.Run(() => delete(), cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteRangeAsync(ICollection<TEntity> collection)
        {
            try
            {
                await Task.Run(() => dbSet.RemoveRange(collection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task DeleteRangeAsync(ICollection<TEntity> collection, CancellationToken cancellationToken)
        {
            try
            {
                return Task.Run(() => dbSet.RemoveRange(collection), cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await dbSet.AsNoTracking().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                return Task.Run(async () =>
                {
                    var result = (ICollection<TEntity>)(await dbSet.AsNoTracking().Where(predicate).ToListAsync());

                    return result;
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await dbSet.Where(predicate).FirstAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                return dbSet.Where(predicate).FirstAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await dbSet.Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                return dbSet.Where(predicate).FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int countRecords = default(int), bool asNoTracking = false)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                {
                    if (countRecords != default(int))
                        query = query.Where(filter).Take(countRecords);
                    else
                        query = query.Where(filter);
                }

                if (!asNoTracking)
                {
                    if (orderBy != null)
                        return await orderBy(query).ToListAsync();
                    else
                        return await query.ToListAsync();
                }
                else
                {
                    if (orderBy != null)
                        return await orderBy(query).AsNoTracking().ToListAsync();
                    else
                        return await query.AsNoTracking().ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<ICollection<TEntity>> GetAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int countRecords = default(int), bool asNoTracking = false)
        {
            try
            {
                return Task.Run(async () =>
                {
                    IQueryable<TEntity> query = dbSet;

                    if (filter != null)
                    {
                        if (countRecords != default(int))
                            query = query.Where(filter).Take(countRecords);
                        else
                            query = query.Where(filter);
                    }

                    if (!asNoTracking)
                    {
                        if (orderBy != null)
                            return await orderBy(query).ToListAsync();
                        else
                            return (ICollection<TEntity>)await query.ToListAsync();
                    }
                    else
                    {
                        if (orderBy != null)
                            return await orderBy(query).AsNoTracking().ToListAsync();
                        else
                            return (ICollection<TEntity>)await query.AsNoTracking().ToListAsync();
                    }
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValueTask<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            try
            {
                return dbSet.FindAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {

            return Task.Run(async () =>
            {
                var result = (ICollection<TEntity>)(await context.Set<TEntity>().ToListAsync(cancellationToken));

                return result;

            }, cancellationToken);
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            try
            {
                await Task.Run(() => context.Entry<TEntity>(entityToUpdate).State = EntityState.Modified);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken)
        {
            try
            {
                return Task.Run(() => { context.Entry<TEntity>(entityToUpdate).State = EntityState.Modified; }, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateRangeAsync(ICollection<TEntity> collection)
        {
            try
            {
                await Task.Run(() => dbSet.UpdateRange(collection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task UpdateRangeAsync(ICollection<TEntity> collection, CancellationToken cancellationToken)
        {
            try
            {
                return Task.Run(() => dbSet.UpdateRange(collection), cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task NoTrackingCollection<TPropertyType>(TEntity entity, Expression<Func<TEntity, IEnumerable<TPropertyType>>> propertyExpression) where TPropertyType : class
        {
            try
            {
                await Task.Run(() => context.Entry(entity).Collection(propertyExpression).IsModified = false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task NoTrackingObject<TPropertyType>(TEntity entity, Expression<Func<TEntity, TPropertyType>> propertyExpression) where TPropertyType : class
        {
            try
            {
                await Task.Run(() => context.Entry(entity).Reference(propertyExpression).IsModified = false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task NoTrackingProperty<TPropertyType>(TEntity entity, Expression<Func<TEntity, TPropertyType>> propertyExpression)
        {
            try
            {
                await Task.Run(() => context.Entry(entity).Property(propertyExpression).IsModified = false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ChangeStateTracking(TEntity entity, EntityTracking entityTracking)
        {
            try
            {
                switch (entityTracking)
                {
                    case EntityTracking.Detached:
                        await Task.Run(() => context.Entry<TEntity>(entity).State = EntityState.Detached);
                        break;
                    case EntityTracking.Unchanged:
                        await Task.Run(() => context.Entry<TEntity>(entity).State = EntityState.Unchanged);
                        break;
                    case EntityTracking.Deleted:
                        await Task.Run(() => context.Entry<TEntity>(entity).State = EntityState.Deleted);
                        break;
                    case EntityTracking.Modified:
                        await Task.Run(() => context.Entry<TEntity>(entity).State = EntityState.Modified);
                        break;
                    case EntityTracking.Added:
                        await Task.Run(() => context.Entry<TEntity>(entity).State = EntityState.Added);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<TEntity>> Include(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (includes != null && filter != null)
                {
                    query = includes.Aggregate(query,
                              (current, include) => current.Include(include));

                    return await query.Where(filter).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<TEntity>> Include(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int countRecords = 0, bool asNoTracking = false, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (includes != null)
                {
                    query = includes.Aggregate(query,
                              (current, include) => current.Include(include));

                    if (filter != null)
                    {
                        if (countRecords != default(int))
                            query = query.Where(filter).Take(countRecords);
                        else
                            query = query.Where(filter);
                    }

                    if (!asNoTracking)
                    {
                        if (orderBy != null)
                            return await orderBy(query).ToListAsync();
                        else
                            return await query.ToListAsync();
                    }
                    else
                    {
                        if (orderBy != null)
                            return await orderBy(query).AsNoTracking().ToListAsync();
                        else
                            return await query.AsNoTracking().ToListAsync();
                    }
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                int result = default(int);

                if (filter != null) { result = await query.CountAsync(filter); }
                else { result = await query.CountAsync(); }

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Expression GetExpression<T>(ParameterExpression pe, Filter filter)
        {
            try
            {
                //Expresión que permite evaluar valores nules
                BinaryExpression nullCheck = null;
                //Expresión para invocar al método IndexOf
                MethodCallExpression methodIndexOf = null;

                //Almacena el parámetro del predicado de la expresión(t =>)
                Expression bodyExpression = pe;

                //Crea el valor o la constante que evaluará la expresión
                ConstantExpression constantExpression = Expression.Constant(filter.Value);
                //Crea el valor 2 o la constante 2 que evaluará la expresión, si se desea utilizarlo para el analisis de rangos.
                ConstantExpression constantExpression2 = Expression.Constant(filter.Value2);

                //Si el valor del filtro que es enviado como string resulta ser de tipo fecha. Cambiamos el formato de la constante que evaluará la expresión a ese tipo.
                //(Por lo general el método converter realiza esta labor; pero en el caso de propiedades que son de tipo fecha, este último se encuentra con problemas en 
                //tiempo de ejecución, es por esto que se determina este cambio)
                if (MethodsCommon.IsDate(filter.Value))
                {
                    constantExpression = Expression.Constant(DateTime.Parse(filter.Value.ToString()));
                }

                if (MethodsCommon.IsDate(filter.Value2))
                {
                    constantExpression2 = Expression.Constant(DateTime.Parse(filter.Value2.ToString()));
                }

                if (!filter.Name.Contains("."))
                {
                    //Añade al parámetro una propiedad(t => t.Property)
                    bodyExpression = Expression.Property(pe, filter.Name);
                }
                else
                {
                    foreach (var property in filter.Name.Split('.'))
                    {
                        //Añade al parámetro una propiedad o un campo, utilizado para las propiedades complejas(t => t.Property.AnotherProperty)
                        bodyExpression = Expression.PropertyOrField(bodyExpression, property);
                    }
                }
                //Se añade la propiedad t => t.Property.Date para descartar en la evaluación de fechas el tiempo
                bodyExpression = this.FormatExpressionToEvaluateOnlyDate(bodyExpression, filter.Value);
                bodyExpression = this.FormatExpressionToEvaluateOnlyDate(bodyExpression, filter.Value2);

                //Añade el filtro al cuerpo de la expresión(t => t.Porperty.Contains(Constant Expression))
                switch (filter.Type)
                {
                    case OptionFilter.Contains:
                        methodIndexOf = Expression.Call(bodyExpression, containsMethod, constantExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                        var notEqual = Expression.NotEqual(methodIndexOf, Expression.Constant(-1));
                        nullCheck = Expression.NotEqual(bodyExpression, Expression.Constant(null, typeof(object)));
                        return Expression.AndAlso(notEqual, nullCheck);

                    case OptionFilter.NotContains:
                        methodIndexOf = Expression.Call(bodyExpression, containsMethod, constantExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                        var equal = Expression.Equal(methodIndexOf, Expression.Constant(-1));
                        nullCheck = Expression.NotEqual(bodyExpression, Expression.Constant(null, typeof(object)));
                        return Expression.AndAlso(equal, nullCheck);

                    case OptionFilter.Equals:
                        if (bodyExpression.Type == typeof(string))
                        {
                            var callEquals = Expression.Call(bodyExpression, equalsMethod, Expression.Convert(constantExpression, bodyExpression.Type), Expression.Constant(StringComparison.OrdinalIgnoreCase));
                            nullCheck = Expression.NotEqual(bodyExpression, Expression.Constant(null, typeof(object)));
                            return Expression.AndAlso(callEquals, nullCheck);
                        }
                        else
                        {
                            return Expression.Equal(bodyExpression, Expression.Convert(constantExpression, bodyExpression.Type));
                        }
                    case OptionFilter.NotEquals:
                        if (bodyExpression.Type == typeof(string))
                        {
                            var notEquals = Expression.Not(Expression.Call(bodyExpression, equalsMethod, Expression.Convert(constantExpression, bodyExpression.Type), Expression.Constant(StringComparison.OrdinalIgnoreCase)));
                            nullCheck = Expression.NotEqual(bodyExpression, Expression.Constant(null, typeof(object)));
                            return Expression.AndAlso(notEquals, nullCheck);
                        }
                        else
                        {
                            return Expression.NotEqual(bodyExpression, Expression.Convert(constantExpression, bodyExpression.Type));
                        }

                    case OptionFilter.StartsWith:
                        var callStartsWith = Expression.Call(bodyExpression, startsWithMethod, constantExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                        nullCheck = Expression.NotEqual(bodyExpression, Expression.Constant(null, typeof(object)));
                        return Expression.AndAlso(callStartsWith, nullCheck);

                    case OptionFilter.EndsWith:
                        var callEndsWith = Expression.Call(bodyExpression, endsWithMethod, constantExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                        nullCheck = Expression.NotEqual(bodyExpression, Expression.Constant(null, typeof(object)));
                        return Expression.AndAlso(callEndsWith, nullCheck);

                    case OptionFilter.GreaterThan:
                        return Expression.GreaterThan(bodyExpression, Expression.Convert(constantExpression, bodyExpression.Type));

                    case OptionFilter.GreaterThanOrEqual:
                        return Expression.GreaterThanOrEqual(bodyExpression, Expression.Convert(constantExpression, bodyExpression.Type));

                    case OptionFilter.LessThan:
                        return Expression.LessThan(bodyExpression, Expression.Convert(constantExpression, bodyExpression.Type));

                    case OptionFilter.LessThanOrEqual:
                        return Expression.LessThanOrEqual(bodyExpression, Expression.Convert(constantExpression, bodyExpression.Type));

                    case OptionFilter.InRange:
                        var before = Expression.GreaterThanOrEqual(bodyExpression, Expression.Convert(constantExpression, bodyExpression.Type));
                        var after = Expression.LessThanOrEqual(bodyExpression, Expression.Convert(constantExpression2, bodyExpression.Type));
                        return Expression.And(before, after);
                }

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private BinaryExpression GetExpression<T>(ParameterExpression pe, Filter filter1, Filter filter2)
        {
            try
            {
                Expression exp1 = GetExpression<T>(pe, filter1);
                Expression exp2 = GetExpression<T>(pe, filter2);

                return Expression.AndAlso(exp1, exp2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Expression FormatExpressionToEvaluateOnlyDate(Expression expression, object value)
        {
            try
            {
                if (MethodsCommon.IsDate(value))
                {
                    if (expression.Type == typeof(DateTime?))
                    {
                        expression = Expression.PropertyOrField(expression, "Value");
                        expression = Expression.PropertyOrField(expression, "Date");
                    }
                    else
                    {
                        expression = Expression.PropertyOrField(expression, "Date");
                    }
                }

                return expression;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
