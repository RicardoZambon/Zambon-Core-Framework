using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.Expressions
{
    public class GlobalExpressionsService
    {

        #region Variables

        private readonly ICurrentUserService currentUserService;

        #endregion

        #region Constructors

        public GlobalExpressionsService(ICurrentUserService _currentUserService)
        {
            currentUserService = _currentUserService;
        }

        #endregion

        #region Methods

        private object GetReplacementValue(string text)
        {
            switch (text)
            {
                case "CurrentUser":
                    return currentUserService.CurrentUser;
                default:
                    return text;
            }
        }
        private object GetReplacementValue<T>(string text, T obj)
        {
            switch (text)
            {
                case "CurrentUser":
                    return currentUserService.CurrentUser;
                default:
                    return (new[] { obj }).AsQueryable().Select(text).FirstOrDefault();
            }
        }

        public object[] FormatArguments(string criteriaArguments)
        {
            var arrayArguments = criteriaArguments.Split(",");
            if ((arrayArguments?.Length ?? 0 ) > 0)
                return arrayArguments.Select(x =>
                    Regex.Match(x, @"\[(.*?)\]").Success
                        ? GetReplacementValue(Regex.Match(x, @"\[(.*?)\]").Groups[1].Value)
                        : x
                ).ToArray();
            return new object[0];
        }
        public object[] FormatArguments<T>(string criteriaArguments, T obj)
        {
            var arrayArguments = criteriaArguments.Split(",");
            if ((arrayArguments?.Length ?? 0) > 0)
                return arrayArguments.Select(x =>
                    Regex.Match(x, @"\[(.*?)\]").Success
                        ? GetReplacementValue(Regex.Match(x, @"\[(.*?)\]").Groups[1].Value, obj)
                        : x
                ).ToArray();
            return new object[0];
        }

        public bool IsConditionApplicable(ICondition element, object obj = null)
        {
            if (obj == null)
                return (new[] { new object() }).AsQueryable().Any(element.Condition, FormatArguments(element.ConditionArguments));

            return (bool)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(IsConditionApplicable) && x.IsGenericMethod).MakeGenericMethod(obj.GetType()).Invoke(this, new[] { element, obj });
        }
        public bool IsConditionApplicable<T>(ICondition element, T obj) where T : class
        {
            return (new[] { obj }).AsQueryable().Any(element.Condition, FormatArguments(element.ConditionArguments, obj));
        }

        public string FormatActionParameters(IActionParameters element, object obj)
        {
            var method = GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetReplacementValue) && x.IsGenericMethod).MakeGenericMethod(obj.GetType().GetCorrectType());

            var parameters = element.ActionParameters.Split("&").ToDictionary(k => k.Split("=")[0], v => v.Split("=")[1]);
            if ((parameters?.Count() ?? 0) > 0)
                return string.Join("&", parameters.Select(x =>
                    x.Key + "=" +
                    (Regex.Match(x.Value, @"\[(.*?)\]").Success
                        ? method.Invoke(this, new object[] { Regex.Match(x.Value, @"\[(.*?)\]").Groups[1].Value, obj })
                        : x)
                ));
            return string.Empty;
        }

        public IEnumerable<TCondition> GetApplicableItems<TCondition>(TCondition[] conditions, object obj) where TCondition : class, ICondition
        {
            return (IEnumerable<TCondition>)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetApplicableItems) && x.IsGenericMethod).MakeGenericMethod(typeof(TCondition), obj.GetType()).Invoke(this, new[] { conditions, obj });
        }
        public IEnumerable<TCondition> GetApplicableItems<TCondition, TObject>(TCondition[] expressions, TObject obj) where TCondition : class, ICondition where TObject : class
        {
            return expressions.Where(x => IsConditionApplicable(x, obj));
        }
        
        #endregion

    }
}