using System.Text;
using Newtonsoft.Json;

namespace Lab3RIAT
{
        public class JsonSerializer:ISerializer
        {
            public bool CanSerialize(string serializeFormat)
            {
                return serializeFormat == "Json" ? true : false;
            }

            public byte[] Serialize<T>(T obj)
            {
                return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            }

            public T Deserialize<T>(byte[] serializedObj)
            {
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(serializedObj));
            }
        }
   }
