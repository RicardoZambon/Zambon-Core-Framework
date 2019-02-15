using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Zambon.Core.Database.Cache.ChangeTracker;

namespace Zambon.Core.Database.Cache.Helper
{
    public static class SerializationExtensionMethods
    {

        public static StoreKey[] DeserializeStoreKeys(this byte[] bytes)
        {
            if (bytes == null)
                return new StoreKey[0];

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;

                return formatter.Deserialize(ms) as StoreKey[];
            }
        }

        public static byte[] SerializeStoreKeys(this StoreKey[] keys)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, keys);

                return ms.ToArray();
            }
        }


        public static Dictionary<string, object> DeserializeStoredObject(this byte[] bytes)
        {
            if (bytes == null)
                return new Dictionary<string,object>();

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;

                return formatter.Deserialize(ms) as Dictionary<string, object>;
            }
        }

        public static byte[] SerializeStoredObject(this Dictionary<string, object> dictionary)
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
