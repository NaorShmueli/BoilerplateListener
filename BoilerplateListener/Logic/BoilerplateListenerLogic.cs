using KafkaManager.Interfaces;
using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class BoilerplateListenerLogic : IBoilerplateListenerLogic
    {
        private readonly ILogger<BoilerplateListenerLogic> _logger;
        private readonly int JOB_INTERVAL_SECONDS;
        private readonly IObservable<long> _subscription;
        private bool isSubscribed;
        private readonly ISampleKafkaPublisher _sampleKafkaPublisher;
        private readonly ISampleKafkaSubscriber _sampleKafkaSubscriber;

        private readonly string producerTopic;
        public BoilerplateListenerLogic(ISampleKafkaSubscriber sampleKafkaSubscriber,ILogger<BoilerplateListenerLogic> logger, IConfiguration configuration, ISampleKafkaPublisher sampleKafkaPublisher)
        {
            JOB_INTERVAL_SECONDS = configuration.GetValue<int>("JobIntervalSeconds");
            _logger = logger;
            _subscription = Observable.Interval(TimeSpan.FromSeconds(JOB_INTERVAL_SECONDS)).StartWith(0);
            isSubscribed = false;
            _sampleKafkaPublisher = sampleKafkaPublisher;
            producerTopic = configuration["Kafka:Topic"];
            _sampleKafkaSubscriber = sampleKafkaSubscriber;
        }

        public void Job()
        {
            if (isSubscribed)
            {
                return;
            }
            isSubscribed = true;
            _sampleKafkaSubscriber.Subscribe(producerTopic, (x) => Console.WriteLine($"Message recevied {JsonConvert.SerializeObject(x)}"));
            _subscription
               .Subscribe(x =>
               {
                   Console.WriteLine($"Job is working {DateTime.Now}");
                   var sampleMessage = new { Name = "Naor", Age = 30 };
                   var jobject = new JObject(sampleMessage);
                   _sampleKafkaPublisher.Publish(jobject, producerTopic);
               });
        }
    }
}
