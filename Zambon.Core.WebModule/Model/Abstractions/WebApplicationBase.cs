using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public class WebApplicationBase<TEntityTypesParent, TEntity, TPropertiesParent, TProperty, TStaticTextsParent, TStaticText, TNavigationParent, TMenu> : ApplicationBase<TEntityTypesParent, TEntity, TPropertiesParent, TProperty, TStaticTextsParent, TStaticText, TNavigationParent, TMenu>
        where TEntityTypesParent : IEntityTypesParent<TEntity, TPropertiesParent, TProperty>
            where TEntity : IEntity<TPropertiesParent, TProperty>, IWebEntity
                where TPropertiesParent : IPropertiesParent<TProperty>
                    where TProperty : IProperty
        where TStaticTextsParent : IStaticTextsParent<TStaticText>
            where TStaticText : IStaticText
        where TNavigationParent : INavigationParent<TMenu>
            where TMenu : IMenu<TMenu>
    {
    }
}