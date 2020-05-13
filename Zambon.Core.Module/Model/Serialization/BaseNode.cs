using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Xml.Serialization;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Model.Serialization
{
    /// <summary>
    /// Base class that serialization will use when loading models.
    /// </summary>
    public abstract class BaseNode : ISerializationNode, IParent
    {
        protected static bool NodeEditParent { get; private set; }

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
                if (!NodeEditParent && !BaseChildItemCollection.CollectionEditParent)
                {
                    throw new InvalidOperationException();
                }
                _parent = value;
            }
        }

        #endregion

        #region Methods

        protected void SetParent<T>(T value, ref T prop) where T : class, IParent
        {
            if (prop != null)
            {
                NodeEditParent = true;
                prop.Parent = null;
                NodeEditParent = false;
            }

            if (value != null)
            {
                NodeEditParent = true;
                value.Parent = this;
                NodeEditParent = false;
            }

            prop = value;
        }


        /// <summary>
        /// Merges two arrays of objects that implements the interface IMergeable.
        /// </summary>
        /// <typeparam name="TObject">The element type of the objects.</typeparam>
        /// <param name="readObj">The source array, containing the original items values.</param>
        public void Merge<TObject>(TObject readObj) where TObject : class, ISerializationNode
        {
            if (readObj != null)
            {
                var properties = typeof(TObject).GetProperties();
                for (var i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];

                    if (property.SetMethod != null && property.Name != nameof(Parent)
                        && property.GetValue(readObj) is object readValue)
                    {
                        var writeValue = property.GetValue(this);

                        if (property.PropertyType.ImplementsInterface<ISerializationNode>())
                        {
                            if (writeValue == null)
                            {
                                writeValue = Activator.CreateInstance(property.PropertyType);
                                property.SetValue(this, writeValue);

                                if (property.PropertyType.ImplementsInterface<IParent>())
                                {
                                    NodeEditParent = true;
                                    ((IParent)writeValue).Parent = this;
                                    NodeEditParent = false;
                                }
                            }

                            GetType().GetMethod(nameof(Merge)).MakeGenericMethod(property.PropertyType).Invoke(writeValue, new object[] { readValue });
                        }
                        else if (properties[i].PropertyType.ImplementsInterface<IEnumerable>() && properties[i].PropertyType.GenericTypeArguments.Count() > 0
                            && properties[i].PropertyType.GenericTypeArguments[0] is Type elementType && elementType.ImplementsInterface<ISerializationNode>())
                        {
                            GetType().GetMethod(nameof(MergeElements)).MakeGenericMethod(elementType).Invoke(this, new object[] { writeValue, readValue });
                        }
                        else if (writeValue == null && readValue != null)
                        {
                            property.SetValue(this, readValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Merges two arrays of objects that implements the interface IMergeable.
        /// </summary>
        /// <typeparam name="TObject">The element type of the array.</typeparam>
        /// <param name="write">The target array, that will have the items with null values set.</param>
        /// <param name="read">The source array, containing the original items values.</param>
        /// <returns>Returns the target array plus all any new element from the source array.</returns>
        public void MergeElements<TObject>(IList<TObject> write, IList<TObject> read) where TObject : class, ISerializationNode, IParent
        {
            if ((read?.Count() ?? 0) > 0)
            {
                if (write == null)
                {
                    write = new ChildItemCollection<TObject>(this);
                }

                var properties = typeof(TObject).GetProperties();

                var keyProperties = properties.Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(MergeAttribute)));
                if (keyProperties.Count() == 0)
                {
                    keyProperties = properties;
                }

                foreach (var readValue in read)
                {
                    TObject writeValue = write.AsQueryable().FirstOrDefault(w => keyProperties.Count(k => k.GetValue(w) == k.GetValue(readValue)) == keyProperties.Count());
                    if (writeValue == null)
                    {
                        writeValue = Activator.CreateInstance<TObject>();
                        write.Add(writeValue);
                    }
                    writeValue.Merge(readValue);
                }
            }
        }

        #endregion
    }
}