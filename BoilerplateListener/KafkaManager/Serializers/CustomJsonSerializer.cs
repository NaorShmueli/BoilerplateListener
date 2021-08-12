using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaManager.Serializers
{

    internal class CustomJsonSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T objectData, SerializationContext context)
        {
            if (!(objectData is string jsonDataString))
            {
                jsonDataString = JsonConvert.SerializeObject(objectData);
            }
            return Encoding.UTF8.GetBytes(jsonDataString);
        }
    }

}
