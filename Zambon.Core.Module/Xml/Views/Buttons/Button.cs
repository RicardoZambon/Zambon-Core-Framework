using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Zambon.Core.Database;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.Buttons
{
    public class Button : XmlNode, IExpression, IComparable
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("IconName")]
        public string IconName { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("ClassName")]
        public string ClassName { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlAttribute("Target")]
        public string Target { get; set; }

        [XmlIgnore]
        public string[] Targets { get { return Target?.Split(',') ?? new string[0]; } }


        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        [XmlAttribute("ActionParameters")]
        public string ActionParameters { get; set; }

        [XmlAttribute("ActionMethod")]
        public string ActionMethod { get; set; }

        [XmlAttribute("UseFormPost")]
        public string BoolUseFormPost { get; set; }
        [XmlIgnore]
        public bool UseFormPost { get { return bool.Parse(BoolUseFormPost?.ToLower() ?? "false"); } }


        [XmlAttribute("ConfirmMessage")]
        public string ConfirmMessage { get; set; }


        [XmlAttribute("ConditionExpression")]
        public string ConditionExpression { get; set; }

        [XmlAttribute("ConditionArguments")]
        public string ConditionArguments { get; set; }


        [XmlAttribute("LoadingContainer")]
        public string LoadingContainer { get; set; }


        [XmlAttribute("OpenModal")]
        public string OpenModal { get; set; }

        [XmlAttribute("ModalTitle")]
        public string ModalTitle { get; set; }


        [XmlIgnore]
        public string[] ConditionArgumentsList
        {
            get { return (ConditionArguments != null ? ConditionArguments.Split(',') : new string[] { }); }
        }


        [XmlElement("SubButtons")]
        public Buttons SubButtons { get; set; }

        #region Overrides

        internal override void OnLoading(Application app, CoreContext ctx)
        {
            if (SubButtons != null)
                SubButtons.OnLoading(app, ctx);

            base.OnLoading(app, ctx);
        }

        #endregion

        #region Methods

        public int CompareTo(object obj)
        {
            if (obj is Button)
                return Index.CompareTo(((Button)obj).Index);
            throw new ArgumentException("Object is not a command button.");
        }

        public bool IsApplicable(string _target)
        {
            return Targets.Contains(_target);
        }

        public bool IsApplicable(string _target, object _obj, GlobalExpressionsService _service)
        {
            if (Targets.Contains(_target))
            {
                if (!string.IsNullOrWhiteSpace(ConditionExpression))
                    return _service.IsApplicableItem(this, _obj);
                return true;
            }
            return false;
        }

        #endregion

    }
}