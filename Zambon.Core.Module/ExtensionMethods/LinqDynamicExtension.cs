using System.Linq;
using System.Linq.Dynamic.Core;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.ExtensionMethods
{
    /// <summary>
    /// Extension methods used in Linq.Dynamic.
    /// </summary>
    public static class LinqDynamicExtension
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TCriteria"></typeparam>
        /// <param name="source"></param>
        /// <param name="expressions"></param>
        /// <param name="objCriteria"></param>
        /// <returns></returns>
        public static IQueryable<TSource> Where<TSource, TCriteria>(this IQueryable<TSource> source, ExpressionsService expressions, TCriteria objCriteria) where TSource : class where TCriteria : class, ICriteria
        {
            return source.Where(objCriteria.Criteria, expressions.FormatArguments(objCriteria.CriteriaArguments));
        }

    }
}