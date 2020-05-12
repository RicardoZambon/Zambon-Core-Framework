using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Serialization;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Zambon.Core.Module.Serialization
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

                    if (property.SetMethod != null && property.Name != nameof(Parent))
                    {
                        var writeValue = property.GetValue(this);
                        var readValue = property.GetValue(readObj);

                        if (property.PropertyType.ImplementsInterface<ISerializationNode>())
                        {
                            if (writeValue == null)
                            {
                                writeValue = Activator.CreateInstance(property.PropertyType);

                                if (property.PropertyType.ImplementsInterface<IParent>())
                                {
                                    NodeEditParent = true;
                                    ((IParent)writeValue).Parent = this;
                                    NodeEditParent = false;
                                }
                            }
                            ((ISerializationNode)writeValue).Merge(readValue as ISerializationNode);
                        }
                        else if (properties[i].PropertyType.IsArray && properties[i].PropertyType.GetElementType() is Type elementType && elementType.ImplementsInterface<ISerializationNode>())
                        {
                            GetType().GetMethod(nameof(MergeElements)).MakeGenericMethod(properties[i].PropertyType.GetElementType()).Invoke(this, new object[] { writeValue, readValue });
                            //properties[i].SetValue(this, arrayValue);
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
        public void MergeElements<TObject>(ChildItemCollection<TObject> write, ChildItemCollection<TObject> read) where TObject : class, ISerializationNode, IParent
        {
            if ((read?.Count ?? 0) > 0)
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

                foreach(var readValue in read)
                {
                    object writeValue = write.AsQueryable().FirstOrDefault(w => keyProperties.Count(k => k.GetValue(w) == k.GetValue(readValue)) == keyProperties.Count());
                    if (writeValue == null)
                    {
                        writeValue = Activator.CreateInstance(typeof(TObject));

                        if (typeof(TObject).ImplementsInterface<IParent>())
                        {
                            NodeEditParent = true;
                            ((IParent)writeValue).Parent = this;
                            NodeEditParent = false;
                        }
                    }

                    ((ISerializationNode)writeValue).Merge(readValue as ISerializationNode);
                }
            }
        }

        #endregion



    }
}