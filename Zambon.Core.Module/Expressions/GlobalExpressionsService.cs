using Zambon.Core.Database.Entity;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Module.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;

namespace Zambon.Core.Module.Expressions
{
    public class GlobalExpressionsService
    {

        #region Variables

        //private readonly ICurrentUserService _currentUser;

        private readonly IDictionary<string, object> GlobalParameters = new Dictionary<string, object>();

        #endregion

        #region Constructors

        public GlobalExpressionsService(ICurrentUserService currentUserService)
        {
            Insert("_CurrentUserService", currentUserService);
        }

        #endregion

        #region Methods

        private void Insert<T>(string key, T _obj) where T : class
        {
            if (!GlobalParameters.ContainsKey(key))
                GlobalParameters.Add(key, _obj);
            else
                GlobalParameters[key] = _obj;
        }

        public object Get(string key)
        {
            if (GlobalParameters.ContainsKey(key))
                return GlobalParameters[key];
            return null;
        }
        public T Get<T>(string key) where T : class
        {
            return (T)Get(key);
        }


        private object GetReplacementValue<T>(string _text, T _obj) where T : class
        {
            return (_text.StartsWith("_"))
                ? Get(_text)
                : (_obj != null ? (new[] { _obj }).AsQueryable().Select(_text).FirstOrDefault() : null);
        }

        public string FormatExpressionValue(string _exp, object _obj)
        {
            var method = typeof(GlobalExpressionsService).GetMethod("FormatExpressionTypedValue").MakeGenericMethod(_obj.GetType().GetCorrectType());
            return method.Invoke(this, new object[] { _exp, _obj }).ToString();
        }
        public string FormatExpressionTypedValue<T>(string _exp, T _obj) where T : class
        {
            if (_exp != null)
                return Regex.Replace(
                        _exp,
                        @"\[(.*?)\]",
                        m => GetReplacementValue(m.Groups[1].ToString(), _obj).ToString()
                    );
            return null;
        }
        public object[] FormatExpressionValues(string[] _exp, object _obj = null)
        {
            if (_exp != null)
            {
                var items = _exp.Select(x =>
                    Regex.Match(x, @"\[(.*?)\]").Success
                        ? GetReplacementValue(Regex.Match(x, @"\[(.*?)\]").Groups[1].Value, _obj)
                        : x
                ).ToArray();
                if (items.Length > 0)
                    return items;
            }
            return new object[] { };
        }

        public IEnumerable<IExpression> GetApplicableExpressionsItems(IExpression[] expressions, object _obj)
        {
            return (IEnumerable<IExpression>)typeof(GlobalExpressionsService).GetMethods().FirstOrDefault(x => x.Name == "GetApplicableExpressionsItems" && x.GetGenericArguments().Count() == 1).MakeGenericMethod(_obj.GetType()).Invoke(this, new[] { expressions, _obj });
        }
        public IEnumerable<IExpression> GetApplicableExpressionsItems<T>(IExpression[] expressions, T _obj) where T : class
        {
            var check = (new[] { _obj }).AsQueryable();
            return expressions.Where(x =>
                check.Where(
                    x.ConditionExpression,
                    FormatExpressionValues(x.ConditionArgumentsList, _obj)).Count() > 0
            );
        }

        public bool IsApplicableItem(IExpression expression)
        {
            return (new[] { new object() }).AsQueryable().Count(
                    expression.ConditionExpression,
                    FormatExpressionValues(expression.ConditionArgumentsList)) > 0;
        }
        public bool IsApplicableItem(IExpression expression, object _obj)
        {
            return (bool)typeof(GlobalExpressionsService).GetMethods().FirstOrDefault(x => x.Name == "IsApplicableItem" && x.GetGenericArguments().Count() == 1 && x.GetParameters().Count() == 2).MakeGenericMethod(_obj?.GetType() ?? typeof(object)).Invoke(this, new[] { expression, _obj ?? new object() });
        }
        public bool IsApplicableItem<T>(IExpression expression, T _obj) where T : class
        {
            return (new[] { _obj }).AsQueryable().Where(
                    expression.ConditionExpression,
                    FormatExpressionValues(expression.ConditionArgumentsList, _obj)).Count() > 0;
        }

        #endregion

    }
}