using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.RegularExpressions;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Services
{
    /// <summary>
    /// Expression service used in custom criterias.
    /// </summary>
    public class ExpressionsService
    {

        #region Variables

        private readonly IUserService UserService;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService">The current user service based from IUserService.</param>
        public ExpressionsService(IUserService userService)
        {
            UserService = userService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the text for replacement values, if contains "CurrentUser" will return the current logged user, otherwise will return the text itself. 
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <returns>Returns the text or the IUserService.</returns>
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
        /// <summary>
        /// Get the text for replacement values, if contains "CurrentUser" will return the current logged user, otherwise will return the property from the object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="text">The text to search.</param>
        /// <param name="obj">The object to get the property from.</param>
        /// <returns>Returns the property value or the IUserService.</returns>
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

        /// <summary>
        /// Searchs all criteria arguments for [] and will try to replace them for the IUserService.
        /// </summary>
        /// <param name="criteriaArguments">The criteria arguments.</param>
        /// <returns>Return an array of arguments.</returns>
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
        /// <summary>
        /// earchs all criteria arguments for [] and will try to replace them for the IUserService or the object property.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="criteriaArguments">The criteria arguments.</param>
        /// <param name="obj">The object to get the properties values from.</param>
        /// <returns>Return an array of arguments.</returns>
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

        /// <summary>
        /// Checks if the condition is applicable for the object.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="obj">The object, if null will use a new object().</param>
        /// <returns>Returns if the condition is applicable (true) or not (false).</returns>
        public bool IsConditionApplicable(string condition, object obj = null)
        {
            if (obj == null)
                return (new[] { new object() }).AsQueryable().Any(condition);

            return (bool)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(IsConditionApplicable) && x.IsGenericMethod && x.GetParameters()[0].Name == nameof(condition)).MakeGenericMethod(obj.GetType()).Invoke(this, new[] { condition, obj });
        }
        /// <summary>
        /// Checks if the condition is applicable for the object.
        /// </summary>
        /// <param name="element">The condition to check.</param>
        /// <param name="obj">The object, if null will use a new object().</param>
        /// <returns>Returns if the condition is applicable (true) or not (false).</returns>
        public bool IsConditionApplicable(ICondition element, object obj = null)
        {
            if (obj == null)
                return (new[] { new object() }).AsQueryable().Any(element.Condition, FormatArguments(element.ConditionArguments));

            return (bool)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(IsConditionApplicable) && x.IsGenericMethod && x.GetParameters()[0].Name == nameof(element)).MakeGenericMethod(obj.GetType()).Invoke(this, new[] { element, obj });
        }
        /// <summary>
        /// Checks if the condition is applicable for the object.
        /// </summary>
        /// <param name="element">The condition to check.</param>
        /// <param name="obj">The object.</param>
        /// <returns>Returns if the condition is applicable (true) or not (false).</returns>
        public bool IsConditionApplicable<T>(ICondition element, T obj) where T : class
        {
            return (new[] { obj }).AsQueryable().Any(element.Condition, FormatArguments(element.ConditionArguments, obj));
        }
        /// <summary>
        /// Checks if the condition is applicable for the object.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="obj">The object.</param>
        /// <returns>Returns if the condition is applicable (true) or not (false).</returns>
        public bool IsConditionApplicable<T>(string condition, T obj) where T : class
        {
            return (new[] { obj }).AsQueryable().Any(condition);
        }

        /// <summary>
        /// Format an expression replacing [] by the objects values.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Format action parameters, replacing [] for the global value or the current object value.
        /// </summary>
        /// <param name="actionParameters">A string with all action paratemers.</param>
        /// <param name="obj">The object to search the properties from.</param>
        /// <returns>Return a dictionaty of parameters and respective values.</returns>
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

        /// <summary>
        /// Get a list of applicable items for that condition.
        /// </summary>
        /// <typeparam name="TCondition">The condition type.</typeparam>
        /// <param name="conditions">The condition object.</param>
        /// <param name="obj">The object to search for.</param>
        /// <returns>Return a list of object where the condition is true.</returns>
        public IEnumerable<TCondition> GetApplicableItems<TCondition>(TCondition[] conditions, object obj) where TCondition : class, ICondition
        {
            return (IEnumerable<TCondition>)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetApplicableItems) && x.GetGenericArguments().Count() == 2).MakeGenericMethod(typeof(TCondition), obj.GetType()).Invoke(this, new[] { conditions, obj });
        }
        /// <summary>
        /// Get a list of applicable items for that condition.
        /// </summary>
        /// <typeparam name="TCondition">The condition type.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="expressions">The condition object.</param>
        /// <param name="obj">The object to search for.</param>
        /// <returns>Return a list of object where the condition is true.</returns>
        public IEnumerable<TCondition> GetApplicableItems<TCondition, TObject>(TCondition[] expressions, TObject obj) where TCondition : class, ICondition where TObject : class
        {
            return expressions.Where(x => IsConditionApplicable(x, obj));
        }
        
        #endregion

    }
}