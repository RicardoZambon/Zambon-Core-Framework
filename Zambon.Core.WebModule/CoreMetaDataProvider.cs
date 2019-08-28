using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Zambon.Core.WebModule
{
    public class CoreMetaDataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            context.DisplayMetadata.ConvertEmptyStringToNull = false;
        }
    }
}