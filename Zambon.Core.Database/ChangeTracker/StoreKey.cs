using System;

namespace Zambon.Core.Database.ChangeTracker
{
    /// <summary>
    /// Represents the key for each object save in custom ChangeTracker.
    /// </summary>
    [Serializable]
    public class StoreKey
    {

        /// <summary>
        /// The ModelType of the entity as string.
        /// </summary>
        public string ModelType { get; private set; }

        /// <summary>
        /// The ID of the entity as integer.
        /// </summary>
        public int EntityId { get; private set; }


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="modelType">The ModelType of the entity as string.</param>
        /// <param name="entityId">The ID of the entity as integer.</param>
        public StoreKey(string modelType, int entityId)
        {
            ModelType = modelType;
            EntityId = entityId;
        }


        /// <summary>
        /// Returns the hash code for this string.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified System.Guid object represent the same value.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>true if g is equal to this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            var compare = (StoreKey)obj;
            return ModelType.Equals(compare.ModelType) && EntityId.Equals(compare.EntityId);
        }

        /// <summary>
        /// Returns a string representation of the value of this instance in registry format.
        /// </summary>
        /// <returns>The value of this System.Guid plus the current user ID, formatted by using the format specifier as follows: xxxx_####.</returns>
        public override string ToString()
        {
            return $"{ModelType}_{EntityId.ToString()}";
        }

    }
}