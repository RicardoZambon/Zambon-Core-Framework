using System;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Xml.EntityTypes;

namespace Zambon.Core.Module.Xml.Views
{
    /// <summary>
    /// Base view class used for all Views.
    /// </summary>
    public abstract class BaseView : XmlNode, IIcon
    {
        /// <summary>
        /// The ViewId, used to merge same elements across ApplicationModels.
        /// </summary>
        [XmlAttribute("ViewId"), MergeKey]
        public string ViewId { get; set; }

        /// <summary>
        /// The view title, if leave blank will use the DisplayName from the EntityType informed in Type property.
        /// </summary>
        [XmlAttribute("Title")]
        public string Title { get; set; }

        /// <summary>
        /// The view icon, if leave blank will use the Icon from the EntityType informed in Type property.
        /// </summary>
        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// The type type, must exist in EntityTypes list.
        /// </summary>
        [XmlAttribute("Type")]
        public string Type { get; set; }


        /// <summary>
        /// The Entity object from the Type property.
        /// </summary>
        [XmlIgnore]
        public Entity Entity { get; protected set; }

        /// <summary>
        /// The current active object being shown.
        /// </summary>
        [XmlIgnore]
        public object CurrentObject { get; protected set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            Entity = app.FindEntityById(Type);
            if (Entity != null)
            {
                if (string.IsNullOrWhiteSpace(Title))
                    Title = Entity.DisplayName;

                if (string.IsNullOrWhiteSpace(Icon))
                    Icon = Entity.Icon;
            }

            base.OnLoadingXml(app, ctx);
        }

        internal override void OnLoadingUserModel(Application app, CoreDbContext ctx)
        {
            Entity = Array.Find(app.EntityTypes, x => x.Id == Type);

            base.OnLoadingUserModel(app, ctx);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return the TypeCLR string from the entity type.
        /// </summary>
        /// <returns></returns>
        public string GetEntityTypeName()
        {
            return Entity?.TypeClr;
        }

        /// <summary>
        /// Return the Type object from the entity type.
        /// </summary>
        /// <returns></returns>
        public Type GetEntityType()
        {
            return Entity?.GetEntityType();
        }

        /// <summary>
        /// Set the current object.
        /// </summary>
        /// <param name="currentObject">Current object. being show in page.</param>
        public void SetCurrentObject(object currentObject)
        {
            CurrentObject = currentObject;
        }

        /// <summary>
        /// Clears the current object.
        /// </summary>
        public void ClearCurrentObject()
        {
            CurrentObject = null;
        }

        #endregion
    }
}