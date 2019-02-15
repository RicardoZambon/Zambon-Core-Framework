using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.WebModule.CustomProviders
{
    public class CoreMetaDataProvider : Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.IDisplayMetadataProvider// IMetadataDetailsProvider, IDisplayMetadataProvider
    {

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            context.DisplayMetadata.ConvertEmptyStringToNull = false;
        }

    }
}