using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Xml.Views.Buttons
{
    public class Button : XmlNode, IComparable, ICondition
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("CssClass")]
        public string CssClass { get; set; }

        [XmlAttribute("Index"), Browsable(false)]
        public string IntIndex
        {
            get { return Index.ToString(); }
            set { int.TryParse(value, out int index); Index = index; }
        }
        [XmlIgnore]
        public int? Index { get; set; }

        [XmlAttribute("Target")]
        public string Target { get; set; }


        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        [XmlAttribute("ActionParameters")]
        public string ActionParameters { get; set; }

        [XmlAttribute("ActionMethod")]
        public string ActionMethod { get; set; }

        [XmlAttribute("UseFormPost"), Browsable(false)]
        public string BoolUseFormPost
        {
            get { return UseFormPost?.ToString(); }
            set { bool.TryParse(value, out bool useFormPost); UseFormPost = useFormPost; }
        }
        [XmlIgnore]
        public bool? UseFormPost { get; set; }

        [XmlAttribute("LoadingContainer")]
        public string LoadingContainer { get; set; }


        [XmlAttribute("ConfirmMessage")]
        public string ConfirmMessage { get; set; }


        [XmlAttribute("Condition")]
        public string Condition { get; set; }

        [XmlAttribute("ConditionArguments")]
        public string ConditionArguments { get; set; }


        [XmlAttribute("OpenModal")]
        public string OpenModal { get; set; }

        [XmlAttribute("ModalTitle")]
        public string ModalTitle { get; set; }


        [XmlIgnore]
        public Button[] SubButtons { get { return _SubButtons?.Button; } }


        [XmlElement("SubButtons"), Browsable(false)]
        public Buttons _SubButtons { get; set; }


        [XmlIgnore]
        public string[] Targets { get { return Target?.Split(',') ?? new string[0]; } }

        [XmlIgnore]
        public BaseModal Modal { get; set; }


        #region Methods

        public int CompareTo(object obj)
        {
            if (obj is Button objButton)
                return (Index ?? 0).CompareTo((objButton.Index ?? 0));
            throw new ArgumentException("Object is not a command button.");
        }

        public bool IsApplicable(string target)
        {
            return Targets.Contains(target);
        }

        public bool IsApplicable(ExpressionsService service, string target, object obj)
        {
            if (IsApplicable(target))
            {
                if (!string.IsNullOrWhiteSpace(Condition))
                    return service.IsConditionApplicable(this, obj);
                return true;
            }
            return false;
        }

        #endregion

    }
}