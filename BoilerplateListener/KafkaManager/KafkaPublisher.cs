using Confluent.Kafka;
using KafkaManager.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KafkaManager
{
    public class KafkaPublisher : ISampleKafkaPublisher
    {
        private IProducer<Ignore,object> _producer;
        private readonly ILogger<KafkaPublisher> _logger;
        private static readonly string AppName = Assembly.GetEntryAssembly()?.GetName().Name;
        public KafkaPublisher(IConfiguration configuration, ILogger<KafkaPublisher> logger)
        {
            _logger = logger;
            InitProducer(configuration);
        }

        private void InitProducer(IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                Debug = configuration["PRODUCER_DEBUG"],
                Acks = Acks.All,
                MessageTimeoutMs = 10000,
                SocketNagleDisable = false,
                MessageSendMaxRetries = 2,
                RetryBackoffMs = 1000,
                BootstrapServers = configuration["BOOTSTRAP_SERVERS"],
                ClientId = AppName
            };
            _producer =  new ProducerBuilder<Ignore, object>(producerConfig)
               .SetLogHandler((producer, confluentLogModel) => _logger.LogInformation(confluentLogModel.Message))
               .SetErrorHandler((producer, confluentLogError) => _logger.LogError(confluentLogError.Reason))
               .Build();
        }

        public async Task Publish(JObject json, string topic)
        {
            Message<Ignore, object> message = new Message<Ignore, object>
            {
                Value = json
            };
            DeliveryResult<Ignore, object> result = await _producer.ProduceAsync(topic, message);
            _logger.LogInformation($"Message status: {result.Status}");
        }
    }
}
