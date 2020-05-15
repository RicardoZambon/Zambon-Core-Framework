using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEntity<TProperties, TProperty> : IParent
        where TProperties : IProperties<TProperty>
            where TProperty : IProperty
    {
        string Id { get; set; }

        string DisplayName { get; set; }

        string SingularName { get; set; }

        string Icon { get; set; }

        string FromSql { get; set; }

        string FromSqlParameters { get; set; }

        string TypeClr { get; set; }

        TProperties _Properties { get; set; }

        ChildItemCollection<TProperty> Properties { get; }
    }
}