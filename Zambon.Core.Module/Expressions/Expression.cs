using System.Linq;
using System.Linq.Dynamic.Core;

namespace Zambon.Core.Module.Expressions
{
    public static class Expression
    {

        public static bool ConditionIsApplicable(string _condition, object _obj)
        {
            return (bool)typeof(Expression).GetMethods().FirstOrDefault(x => x.Name == "ConditionIsApplicable" && x.GetGenericArguments().Count() == 1 && x.GetParameters().Count() == 2).MakeGenericMethod(_obj.GetType()).Invoke(null, new[] { _condition, _obj });
        }

        public static bool ConditionIsApplicable<T>(string _condition, T _obj) where T : class
        {
            return (new[] { _obj }).AsQueryable().Where(_condition, new object[] { }).Count() > 0;
        }

    }
}