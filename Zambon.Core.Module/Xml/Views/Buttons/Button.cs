using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Xml.Views.Buttons
{
    /// <summary>
    /// Represents buttons used in DetailViews or ListViews.
    /// </summary>
    public class Button : XmlNode, IComparable, ICondition, IIcon
    {

        /// <summary>
        /// The button Id, used to merge same elements across ApplicationModels.
        /// </summary>
        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        /// <summary>
        /// The icon should be shows along with the button.
        /// </summary>
        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// The display name should show with the button (Inline buttons does not use DisplayName). The display name is also used to set the Title html property.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Custom CSS class to use with the button.
        /// </summary>
        [XmlAttribute("CssClass")]
        public string CssClass { get; set; }

        /// <summary>
        /// The index value returned from XML file.
        /// </summary>
        [XmlAttribute("Index"), Browsable(false)]
        public string IntIndex
        {
            get { return Index.ToString(); }
            set { if (value != null) { int.TryParse(value, out int index); Index = index; } }
        }
        /// <summary>
        /// The button index order to display.
        /// </summary>
        [XmlIgnore]
        public int? Index { get; set; }

        /// <summary>
        /// The target container of the button, where the button should be shown (Toolbar, Inline, ModalFooter, Custom...). Can have more than one container separated by ",".
        /// </summary>
        [XmlAttribute("Target")]
        public string Target { get; set; }


        /// <summary>
        /// The controller name of the button, if no controller is informed will use the controller from the View where is inserted.
        /// </summary>
        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        /// <summary>
        /// The action name this button should execute when clicked.
        /// </summary>
        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        /// <summary>
        /// List of custom parameters to pass to the action, is possible to use object properties with [ ], ex: id=[ID].
        /// </summary>
        [XmlAttribute("ActionParameters")]
        public string ActionParameters { get; set; }

        /// <summary>
        /// The action method the button should execute, by default is POST.
        /// </summary>
        [XmlAttribute("ActionMethod")]
        public string ActionMethod { get; set; }

        /// <summary>
        /// If the button should use the form post from DetailView, passing the entire object to the server. Default false.
        /// </summary>
        [XmlAttribute("UseFormPost"), Browsable(false)]
        public string BoolUseFormPost
        {
            get { return UseFormPost?.ToString(); }
            set { if (value != null) { bool.TryParse(value, out bool useFormPost); UseFormPost = useFormPost; } }
        }
        /// <summary>
        /// If the button should use the form post from DetailView, passing the entire object to the server. Default false.
        /// </summary>
        [XmlIgnore]
        public bool? UseFormPost { get; set; }

        /// <summary>
        /// Name of the loading container this button should use, default blank and will use the same from the form.
        /// </summary>
        [XmlAttribute("LoadingContainer")]
        public string LoadingContainer { get; set; }


        /// <summary>
        /// If this button should show a confirmation message before submitting.
        /// </summary>
        [XmlAttribute("ConfirmMessage")]
        public string ConfirmMessage { get; set; }


        /// <summary>
        /// Condition to show this button.
        /// </summary>
        [XmlAttribute("Condition")]
        public string Condition { get; set; }

        /// <summary>
        /// Arguments pass to the condition separated by ",".
        /// </summary>
        [XmlAttribute("ConditionArguments")]
        public string ConditionArguments { get; set; }


        /// <summary>
        /// The modal id in SubViews > SubModal > Id this button should open when clicked.
        /// </summary>
        [XmlAttribute("OpenModal")]
        public string OpenModal { get; set; }

        /// <summary>
        /// Override the modal Title property set in the modal itself and in the SubDetailView.
        /// </summary>
        [XmlAttribute("ModalTitle")]
        public string ModalTitle { get; set; }


        /// <summary>
        /// Sub buttons list. Only used in Toolbar buttons.
        /// </summary>
        [XmlIgnore]
        public Button[] SubButtons { get { return _SubButtons?.Button; } }


        /// <summary>
        /// Sub buttons list. Only used in Toolbar buttons.
        /// </summary>
        [XmlElement("SubButtons"), Browsable(false)]
        public Buttons _SubButtons { get; set; }


        /// <summary>
        /// List of targets set in Target property separated by ",".
        /// </summary>
        [XmlIgnore]
        public string[] Targets { get { return Target?.Split(',') ?? new string[0]; } }

        /// <summary>
        /// The modal object this button should open. Null if is not opening any modal.
        /// </summary>
        [XmlIgnore]
        public BaseModal Modal { get; set; }


        #region Methods

        /// <summary>
        /// Compares the Index with other button, to sort the buttons array.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is Button objButton)
                return (Index ?? 0).CompareTo((objButton.Index ?? 0));
            throw new ArgumentException("Object is not a command button.");
        }

        /// <summary>
        /// Checks if the button is application to the target.
        /// </summary>
        /// <param name="target">The target container to check.</param>
        /// <returns>Returns true if the button is applicable.</returns>
        public bool IsApplicable(string target)
        {
            return Targets.Contains(target);
        }

        /// <summary>
        /// Checks if the button is application to the target and with the current object.
        /// </summary>
        /// <param name="service">Expressions service to compare the object with the condition property.</param>
        /// <param name="target">The target container to check.</param>
        /// <param name="obj">The object instance to compare the Criteria property.</param>
        /// <returns></returns>
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