using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface ILanguagesParent<TLanguage> : IParent
        where TLanguage : ILanguage
    {
        ChildItemCollection<TLanguage> LanguagesList { get; set; }
    }
}