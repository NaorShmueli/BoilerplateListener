using Confluent.Kafka;
using KafkaManager.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KafkaManager
{
    public class KafkaSubscriber : ISampleKafkaSubscriber
    {
        private readonly ILogger<KafkaSubscriber> _logger;
        private static readonly string AppName = Assembly.GetEntryAssembly()?.GetName().Name;
        private IConsumer<Ignore, object> _consumer;
        public KafkaSubscriber(IConfiguration configuration)
        {
            InitConsumer(configuration);
        }

        private void InitConsumer(IConfiguration configuration)
        {
            ConsumerConfig consumerConfig = new ConsumerConfig
            {
                Debug = configuration["CONSUMER_DEBUG"],
                GroupId = AppName,
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoOffsetStore = true,
                AutoCommitIntervalMs = 0,
                BootstrapServers = configuration["BOOTSTRAP_SERVERS"],
                ClientId = AppName
            };

            _consumer =  new ConsumerBuilder<Ignore, object>(consumerConfig)
                .SetLogHandler((consumer, confluentLogModel) => _logger.LogInformation(confluentLogModel.Message))
                .SetErrorHandler((consumer, confluentLogError) => _logger.LogError(confluentLogError.Reason))
                .Build();
        }

        public void Subscribe(string topicName, Action<object> action)
        {
            try
            {
                _consumer.Subscribe(topicName);
                while (true)
                {
                    var consumeResult = _consumer.Consume();
                    var message = consumeResult.Message.Value;
                    action.Invoke(message);
                    _consumer.Commit(consumeResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Consumer stopped. {ex}");
            }
           
        }
    }
}
