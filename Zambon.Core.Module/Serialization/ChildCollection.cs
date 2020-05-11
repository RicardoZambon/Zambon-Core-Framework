using System.Collections.Generic;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Serialization
{
    public abstract class BaseChildItemCollection
    {
        public static bool AllowEditParent { get; protected set; }
    }

    /// <summary>
    /// Collection of child items. This collection automatically set the
    /// Parent property of the child items when they are added or removed
    /// </summary>
    /// <typeparam name="TChild">Type of the child items</typeparam>
    public class ChildItemCollection<TChild> : BaseChildItemCollection, IList<TChild> where TChild : class, ISerializationNode
    {
        private object _parent;
        private IList<TChild> _collection;

        #region Constructors

        public ChildItemCollection(object parent)
        {
            this._parent = parent;
            this._collection = new List<TChild>();
        }

        public ChildItemCollection(object parent, IList<TChild> collection)
        {
            this._parent = parent;
            this._collection = collection;
        }

        #endregion

        #region IList<TChild> Members

        public int IndexOf(TChild item)
        {
            return _collection.IndexOf(item);
        }

        public void Insert(int index, TChild item)
        {
            if (item != null)
            {
                _allowEditParent = true;
                item.Parent = _parent;
                _allowEditParent = false;
            }
            _collection.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            TChild oldItem = _collection[index];
            _collection.RemoveAt(index);
            if (oldItem != null)
            {
                _allowEditParent = true;
                oldItem.Parent = null;
                _allowEditParent = false;
            }
        }

        public TChild this[int index]
        {
            get
            {
                return _collection[index];
            }
            set
            {
                TChild oldItem = _collection[index];
                if (value != null)
                {
                    _allowEditParent = true;
                    value.Parent = _parent;
                    _allowEditParent = false;
                }
                _collection[index] = value;
                if (oldItem != null)
                {
                    _allowEditParent = true;
                    oldItem.Parent = null;
                    _allowEditParent = false;
                }
            }
        }

        #endregion

        #region ICollection<TChild> Members

        public void Add(TChild item)
        {
            if (item != null)
            {
                _allowEditParent = true;
                item.Parent = _parent;
                _allowEditParent = false;
            }
            _collection.Add(item);
        }

        public void Clear()
        {
            foreach (TChild item in _collection)
            {
                if (item != null)
                {
                    _allowEditParent = true;
                    item.Parent = null;
                    _allowEditParent = false;
                }
            }
            _collection.Clear();
        }

        public bool Contains(TChild item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(TChild[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return _collection.IsReadOnly; }
        }

        public bool Remove(TChild item)
        {
            bool b = _collection.Remove(item);
            if (item != null)
            {
                _allowEditParent = true;
                item.Parent = null;
                _allowEditParent = false;
            }
            return b;
        }

        #endregion

        #region IEnumerable<TChild> Members

        public IEnumerator<TChild> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (_collection as System.Collections.IEnumerable).GetEnumerator();
        }

        #endregion
    }
}