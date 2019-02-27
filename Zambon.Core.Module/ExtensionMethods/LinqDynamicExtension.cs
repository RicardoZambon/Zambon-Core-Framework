using System.Linq;
using System.Linq.Dynamic.Core;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.ExtensionMethods
{
    public static class LinqDynamicExtension
    {

        public static IQueryable<TSource> Where<TSource, TCriteria>(this IQueryable<TSource> source, GlobalExpressionsService expressions, TCriteria objCriteria) where TSource : class where TCriteria : class, ICriteria
        {
            return source.Where(objCriteria.Criteria, expressions.FormatArguments(objCriteria.CriteriaArguments));
        }

    }
}