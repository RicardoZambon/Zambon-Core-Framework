using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

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
        public EntityTypes.Entity Entity { get; private set; }

        #region Methods

        internal override void OnLoading(Application app, CoreDbContext ctx) //protected virtual void LoadView(Application app)
        {
            Entity = app.EntityTypes.Entities.FirstOrDefault(x => x.Id == Type);

            if (string.IsNullOrWhiteSpace(Title))
                Title = Entity?.DisplayName;

            if (string.IsNullOrWhiteSpace(Icon))
                Icon = Entity?.Icon;
        }

        public string GetEntityTypeName()
        {
            return Entity?.TypeClr;
        }

        public Type GetEntityType()
        {
            return Entity?.GetEntityType();
        }
        
        #endregion

    }
}