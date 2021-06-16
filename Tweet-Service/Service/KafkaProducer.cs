using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tweet_Service.Models;

namespace Tweet_Service.Service
{
    public class KafkaProducer
    {
        private readonly ProducerConfig _producerConfig;
        private static IConfiguration _config;

        public KafkaProducer(IConfiguration config)
        {
            _config = config;
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = _config.GetValue<string>("UrlDocker:Kafka")
            };
        }


        public void SendTrendToKafka(string topic, TrendDTO message)
        {
            

            using var producer =
                new ProducerBuilder<Null, string>(_producerConfig).Build();
            try
            {
                var response = producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonSerializer.Serialize(message) })
                    .GetAwaiter()
                    .GetResult();
                Console.WriteLine(response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Oops, something went wrong: {e}");
            }
        }
    }
}
