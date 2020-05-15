using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IApplication<TEntityTypesParent, TEntity, TPropertiesParent, TProperty, TStaticTextsParent, TStaticText, TNavigationParent, TMenu>
        where TEntityTypesParent : IEntityTypesParent<TEntity, TPropertiesParent, TProperty>
            where TEntity : IEntity<TPropertiesParent, TProperty>
                where TPropertiesParent : IPropertiesParent<TProperty>
                    where TProperty : IProperty
        where TStaticTextsParent : IStaticTextsParent<TStaticText>
            where TStaticText : IStaticText
        where TNavigationParent : INavigationParent<TMenu>
            where TMenu : IMenu<TMenu>
    {
        #region XML Elements

        TEntityTypesParent _EntityTypes { get; set; }

        TStaticTextsParent _StaticTexts { get; set; }

        TNavigationParent _Navigation { get; set; }

        #endregion

        #region Properties

        ChildItemCollection<TEntity> Entities { get; }

        ChildItemCollection<TStaticText> StaticTexts { get; }
        
        ChildItemCollection<TMenu> Menus { get; }

        #endregion
    }
}