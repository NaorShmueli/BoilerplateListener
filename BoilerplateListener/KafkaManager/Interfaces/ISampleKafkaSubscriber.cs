using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaManager.Interfaces
{
    public interface ISampleKafkaSubscriber
    {
        void Subscribe(string topicName, Action<object> action);
    }
}
