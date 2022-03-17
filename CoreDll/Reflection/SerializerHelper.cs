using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDll.Reflection
{
    public static class SerializerHelper
    {
        public static byte[] SerializeBytes<T>(T data) where T : class, new()
        {
            byte[] ret = null;

            if (data != null)
            {
                System.IO.MemoryStream streamMemory = new System.IO.MemoryStream();
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(streamMemory, data);
                ret = streamMemory.GetBuffer();
            }

            return ret;
        }

        public static T DeserializeBytes<T>(byte[] binData) where T : class, new()
        {
            T retorno = default(T);

            if (binData != null && binData.Length != 0)
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.MemoryStream streamMemory = new System.IO.MemoryStream(binData);
                retorno = formatter.Deserialize(streamMemory) as T;
            }

            return retorno;
        }
    }
}