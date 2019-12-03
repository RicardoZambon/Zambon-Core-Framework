using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Zambon.Core.Database.ChangeTracker.Keys;

namespace Zambon.Core.Database.ChangeTracker.Extensions
{
    /// <summary>
    /// Extension methods to serialize and deserialize objects from array of bytes.
    /// </summary>
    internal static class SerializationExtension
    {
        /// <summary>
        /// Deserialize an array of stored entity instance keys.
        /// </summary>
        /// <param name="bytes">The bytes array to deserialize.</param>
        /// <returns>Returns an array of EntityInstanceKey.</returns>
        internal static EntityInstanceKey[] DeserializeEntityInstanceKeys(this byte[] bytes)
        {
            if (bytes == null)
            {
                return new EntityInstanceKey[0];
            }
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return formatter.Deserialize(ms) as EntityInstanceKey[];
            }
        }

        /// <summary>
        /// Serialize an array of entity instance keys.
        /// </summary>
        /// <param name="keys">The EntityInstanceKey array to serialize.</param>
        /// <returns>Returns an array of bytes.</returns>
        internal static byte[] SerializeEntityInstanceKeys(this EntityInstanceKey[] keys)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, keys);
                return ms.ToArray();
            }
        }


        /// <summary>
        /// Deserialize a dictionary of the object changed properties.
        /// </summary>
        /// <param name="bytes">The bytes array to deserialize.</param>
        /// <returns>Returns a dictionary of string (property name) and object (value).</returns>
        internal static Dictionary<string, object> DeserializeEntityInstanceObject(this byte[] bytes)
        {
            if (bytes == null)
            {
                return new Dictionary<string, object>();
            }
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return formatter.Deserialize(ms) as Dictionary<string, object>;
            }
        }

        /// <summary>
        /// Serialize a dictionary of the object changed properties.
        /// </summary>
        /// <param name="dictionary">The dictionary to serialize.</param>
        /// <returns>Returns an array of bytes.</returns>
        internal static byte[] SerializeEntityInstanceObject(this Dictionary<string, object> dictionary)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, dictionary);
                return ms.ToArray();
            }
        }
    }
}