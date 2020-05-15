﻿using Microsoft.Extensions.Options;
using Zambon.Core.Module.Configurations;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.Model.Abstractions;

namespace Zambon.Core.WebModule.Services
{
    public abstract class WebModelProviderBase<TApplication, TEntityTypesParent, TEntity, TPropertiesParent, TProperty, TStaticTextsParent, TStaticText, TNavigationParent, TMenu> : BaseModelProvider<TApplication, TEntityTypesParent, TEntity, TPropertiesParent, TProperty, TStaticTextsParent, TStaticText, TNavigationParent, TMenu>
        where TApplication : WebApplicationBase<TEntityTypesParent, TEntity, TPropertiesParent, TProperty, TStaticTextsParent, TStaticText, TNavigationParent, TMenu>, new()
            where TEntityTypesParent : WebEntityTypesParentBase<TEntity, TPropertiesParent, TProperty>
                where TEntity : WebEntityBase<TPropertiesParent, TProperty>
                    where TPropertiesParent : PropertiesParentBase<TProperty>
                        where TProperty : PropertyBase
            where TStaticTextsParent : StaticTextsParentBase<TStaticText>
                where TStaticText : StaticTextBase
            where TNavigationParent : NavigationParentBase<TMenu>
                where TMenu : MenuBase<TMenu>
    {
        public WebModelProviderBase(IOptions<AppSettings> appSettings, IModule mainModule) : base(appSettings, mainModule)
        {
        }
    }
}
