using Zambon.Core.Module.Interfaces.Models.Common;
using Zambon.Core.Module.Interfaces.Models.Validations;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IColumn : IParent, IIndex, IModelValidation
    {
        string Id { get; set; }

        string PropertyName { get; set; }

        string DisplayName { get; set; }

        string Size { get; set; }

        string Alignment { get; set; }

        string FormatType { get; set; }

        string FormatRegex { get; set; }

        string FormatRegexReplacement { get; set; }

        string NullValue { get; set; }
    }
}