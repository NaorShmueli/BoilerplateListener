using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaManager.Serializers
{
    class CustomJsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> objectData, bool isNull, SerializationContext context)
        {
            if (isNull) return default;
            T result = default;
            string jsonDataString = Encoding.UTF8.GetString(objectData.ToArray());
            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)jsonDataString;
                }
                result = JsonConvert.DeserializeObject<T>(jsonDataString);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
