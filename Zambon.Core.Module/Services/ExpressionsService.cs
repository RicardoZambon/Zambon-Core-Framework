using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.RegularExpressions;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Services
{
    public class ExpressionsService
    {

        #region Variables

        private readonly IUserService UserService;

        #endregion

        #region Constructors

        public ExpressionsService(IUserService userService)
        {
            UserService = userService;
        }

        #endregion

        #region Methods

        public object GetReplacementValue(string text)
        {
            switch (text)
            {
                case "CurrentUser":
                    return UserService.CurrentUser;
                default:
                    return text;
            }
        }
        public object GetReplacementValue<T>(string text, T obj)
        {
            switch (text)
            {
                case "CurrentUser":
                    return UserService.CurrentUser;
                default:
                    return (new[] { obj }).AsQueryable().Select(text).FirstOrDefault();
            }
        }

        public object[] FormatArguments(string criteriaArguments)
        {
            if (!string.IsNullOrWhiteSpace(criteriaArguments))
            {
                var arrayArguments = criteriaArguments.Split(",");
                if ((arrayArguments?.Length ?? 0) > 0)
                    return arrayArguments.Select(x =>
                        Regex.Match(x, @"\[(.*?)\]").Success
                            ? GetReplacementValue(Regex.Match(x, @"\[(.*?)\]").Groups[1].Value)
                            : x
                    ).ToArray();
            }
            return new object[0];
        }
        public object[] FormatArguments<T>(string criteriaArguments, T obj)
        {
            if (obj == null)
                return FormatArguments(criteriaArguments);

            if (!string.IsNullOrWhiteSpace(criteriaArguments))
            {
                var arrayArguments = criteriaArguments.Split(",");
                if ((arrayArguments?.Length ?? 0) > 0)
                    return arrayArguments.Select(x =>
                        Regex.Match(x, @"\[(.*?)\]").Success
                            ? GetReplacementValue(Regex.Match(x, @"\[(.*?)\]").Groups[1].Value, obj)
                            : x
                    ).ToArray();
            }
            return new object[0];
        }

        public bool IsConditionApplicable(string condition, object obj = null)
        {
            if (obj == null)
                return (new[] { new object() }).AsQueryable().Any(condition);

            return (bool)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(IsConditionApplicable) && x.IsGenericMethod && x.GetParameters()[0].Name == nameof(condition)).MakeGenericMethod(obj.GetType()).Invoke(this, new[] { condition, obj });
        }
        public bool IsConditionApplicable(ICondition element, object obj = null)
        {
            if (obj == null)
                return (new[] { new object() }).AsQueryable().Any(element.Condition, FormatArguments(element.ConditionArguments));

            return (bool)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(IsConditionApplicable) && x.IsGenericMethod && x.GetParameters()[0].Name == nameof(element)).MakeGenericMethod(obj.GetType()).Invoke(this, new[] { element, obj });
        }
        public bool IsConditionApplicable<T>(ICondition element, T obj) where T : class
        {
            return (new[] { obj }).AsQueryable().Any(element.Condition, FormatArguments(element.ConditionArguments, obj));
        }
        public bool IsConditionApplicable<T>(string condition, T obj) where T : class
        {
            return (new[] { obj }).AsQueryable().Any(condition);
        }

        public string FormatExpression(string expression, object obj = null)
        {
            MethodInfo method = null;
            if (obj != null)
                method = GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetReplacementValue) && x.IsGenericMethod).MakeGenericMethod(obj.GetType().GetCorrectType());

            if (!string.IsNullOrWhiteSpace(expression))
                return Regex.Replace(
                    expression,
                    @"\[(.*?)\]",
                    m => (obj != null ? method.Invoke(this, new object[] { m.Groups[1].ToString(), obj }) : GetReplacementValue(m.Groups[1].ToString())).ToString()
                );
            return string.Empty;
        }

        public IDictionary<string, string> FormatActionParameters(string actionParameters, object obj = null)
        {
            MethodInfo method = null;
            if (obj != null)
                method = GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetReplacementValue) && x.IsGenericMethod).MakeGenericMethod(obj.GetType().GetCorrectType());

            if (!string.IsNullOrWhiteSpace(actionParameters))
            {
                var parameters = actionParameters.Split("&").ToDictionary(k => k.Split("=")[0], v => v.Split("=")[1]);
                if ((parameters?.Count() ?? 0) > 0)
                    return parameters.ToDictionary(
                        k => k.Key,
                        v => (Regex.Match(v.Value, @"\[(.*?)\]").Success && obj != null
                            ? method.Invoke(this, new object[] { Regex.Match(v.Value, @"\[(.*?)\]").Groups[1].Value, obj })
                            : v).ToString()
                    );
            }
            return new Dictionary<string, string>();
        }

        public IEnumerable<TCondition> GetApplicableItems<TCondition>(TCondition[] conditions, object obj) where TCondition : class, ICondition
        {
            return (IEnumerable<TCondition>)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetApplicableItems) && x.GetGenericArguments().Count() == 2).MakeGenericMethod(typeof(TCondition), obj.GetType()).Invoke(this, new[] { conditions, obj });
        }
        public IEnumerable<TCondition> GetApplicableItems<TCondition, TObject>(TCondition[] expressions, TObject obj) where TCondition : class, ICondition where TObject : class
        {
            return expressions.Where(x => IsConditionApplicable(x, obj));
        }
        
        #endregion

    }
}