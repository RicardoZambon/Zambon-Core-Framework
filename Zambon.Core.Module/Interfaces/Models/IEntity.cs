using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEntity<TPropertiesParent, TProperty> : IParent
        where TPropertiesParent : IPropertiesParent<TProperty>
            where TProperty : IProperty
    {
        string Id { get; set; }

        string DisplayName { get; set; }

        string SingularName { get; set; }

        string Icon { get; set; }

        string FromSql { get; set; }

        string FromSqlParameters { get; set; }

        string TypeClr { get; set; }

        TPropertiesParent _Properties { get; set; }

        ChildItemCollection<TProperty> Properties { get; }
    }
}