using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.WebModule.Interfaces.Models
{
    public interface IWebGridTemplate : IGridTemplate
    {
        string RowCssClass { get; set; }

        string CellCssClass { get; set; }
    }
}