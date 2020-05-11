using System;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Serialization
{
    /// <summary>
    /// Base class that serialization will use when loading models.
    /// </summary>
    public abstract class BaseNode : ISerializationNode
    {
        #region Properties

        private object _parent;
        /// <summary>
        /// The parent node instance.
        /// </summary>
        [XmlIgnore]
        public object Parent
        {
            get => _parent;
            set
            {
                if (!BaseChildItemCollection.AllowEditParent)
                {
                    throw new InvalidOperationException();
                }
                _parent = value;
            }
        }

        #endregion
    }
}