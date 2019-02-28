using System;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Xml.EntityTypes;

namespace Zambon.Core.Module.Xml.Views
{
    public class BaseView : XmlNode
    {

        [XmlAttribute("ViewId"), MergeKey]
        public string ViewId { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }


        [XmlIgnore]
        protected Entity Entity { get; set; }

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

        public string GetEntityTypeName()
        {
            return Entity?.TypeClr;
        }

        public Type GetEntityType()
        {
            return Entity?.GetEntityType();
        }

        public void SetCurrentObject(object currentObject)
        {
            CurrentObject = currentObject;
        }

        public void ClearCurrentObject()
        {
            CurrentObject = null;
        }

        #endregion

    }
}