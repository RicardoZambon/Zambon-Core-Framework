using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Xml.Serialization;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Extensions;
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
        public void Merge(object readObj) //where TObject : class, ISerializationNode
        {
            if (readObj != null)
            {
                var readProperties = readObj.GetType().GetProperties();
                for (var i = 0; i < readProperties.Length; i++)
                {
                    var readProperty = readProperties[i];

                    if (readProperty.SetMethod != null && readProperty.Name != nameof(Parent)
                        && readProperty.GetValue(readObj) is object readValue)
                    {
                        var writeProperty = GetType().GetPropertyFromParents(readProperty.Name);
                        var writeValue = writeProperty.GetValue(this);

                        if (readProperty.PropertyType.ImplementsInterface<ISerializationNode>())
                        {
                            if (writeValue == null)
                            {
                                writeValue = Activator.CreateInstance(writeProperty.PropertyType);
                                writeProperty.SetValue(this, writeValue);

                                if (readProperty.PropertyType.ImplementsInterface<IParent>())
                                {
                                    NodeEditParent = true;
                                    ((IParent)writeValue).Parent = this;
                                    NodeEditParent = false;
                                }
                            }

                            GetType().GetMethod(nameof(Merge))/*.MakeGenericMethod(property.PropertyType)*/.Invoke(writeValue, new object[] { readValue });
                        }
                        else if (readProperties[i].PropertyType.ImplementsInterface<IEnumerable>() && readProperties[i].PropertyType.GenericTypeArguments.Count() > 0
                            && readProperties[i].PropertyType.GenericTypeArguments[0] is Type elementType && elementType.ImplementsInterface<ISerializationNode>())
                        {
                            GetType().GetMethod(nameof(MergeElements)).MakeGenericMethod(new Type[] { writeProperty.PropertyType.GenericTypeArguments[0], elementType }).Invoke(this, new object[] { writeValue, readValue });
                        }
                        else if (writeValue == null && readValue != null)
                        {
                            writeProperty.SetValue(this, readValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Merges two arrays of objects that implements the interface IMergeable.
        /// </summary>
        /// <typeparam name="TWObject">The element type of the array.</typeparam>
        /// <param name="write">The target array, that will have the items with null values set.</param>
        /// <param name="read">The source array, containing the original items values.</param>
        /// <returns>Returns the target array plus all any new element from the source array.</returns>
        public void MergeElements<TWObject, TRObject>(IList<TWObject> write, IList<TRObject> read)
            where TWObject : class, ISerializationNode, IParent
            where TRObject : class, ISerializationNode, IParent
        {
            if ((read?.Count() ?? 0) > 0)
            {
                if (write == null)
                {
                    write = new ChildItemCollection<TWObject>(this);
                }

                var keyRProperties = typeof(TRObject).GetProperties()
                    .Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(MergeKeyAttribute)))
                    .ToDictionary(k => k.Name, v => v);
                
                if (keyRProperties.Count() == 0)
                {
                    keyRProperties = typeof(TRObject).GetProperties()
                        .ToDictionary(k => k.Name, v => v);
                }

                var keyWProperties = keyRProperties.Select(x => typeof(TWObject).GetProperty(x.Key))
                    .ToDictionary(k => k.Name, v => v);

                foreach (var readValue in read)
                {
                    TWObject writeValue = write.FirstOrDefault(w => keyWProperties.Select(x => x.Value.GetValue(w) == keyRProperties[x.Key].GetValue(readValue)).Count() == keyWProperties.Count());
                    if (writeValue == null)
                    {
                        writeValue = Activator.CreateInstance<TWObject>();
                        write.Add(writeValue);
                    }
                    writeValue.Merge(readValue);
                }
            }
        }

        #endregion
    }
}