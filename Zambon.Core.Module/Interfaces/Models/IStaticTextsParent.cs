using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IStaticTextsParent<TStaticText> : IParent
        where TStaticText : IStaticText
    {
        ChildItemCollection<TStaticText> StaticTextsList { get; set; }
    }
}