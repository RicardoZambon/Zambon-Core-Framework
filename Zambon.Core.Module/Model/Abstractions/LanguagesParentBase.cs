using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class LanguagesParentBase<TLanguage> : SerializeNodeBase, ILanguagesParent<TLanguage>
        where TLanguage : LanguageBase
    {
        #region Constants

        private const string LANGUAGES_NODE = "Language";

        #endregion

        #region XML Elements

        [XmlElement(LANGUAGES_NODE)]
        public ChildItemCollection<TLanguage> LanguagesList { get; set; }

        #endregion

        #region Constructors

        public LanguagesParentBase()
        {
            LanguagesList = new ChildItemCollection<TLanguage>(this);
        }

        #endregion
    }
}