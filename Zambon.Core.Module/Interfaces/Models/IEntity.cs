using Zambon.Core.Module.Interfaces.Models.Validations;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEntity : IParent, IDbValidation
    {
        string Id { get; set; }

        string DisplayName { get; set; }

        string SingularName { get; set; }

        string Icon { get; set; }

        string FromSql { get; set; }

        string FromSqlParameters { get; set; }

        string TypeClr { get; set; }
    }

    public interface IEntity<TProperty> : IEntity where TProperty : class, IProperty
    {
        ChildItemCollection<TProperty> Properties { get; set; }
    }
}