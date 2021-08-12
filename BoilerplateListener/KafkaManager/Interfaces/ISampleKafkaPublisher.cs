using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaManager.Interfaces
{
    public interface ISampleKafkaPublisher
    {
        Task Publish(JObject json, string topic);
    }
}
