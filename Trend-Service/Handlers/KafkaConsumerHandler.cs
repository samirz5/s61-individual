using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Trend_Service.Interfaces;
using Trend_Service.Models;

namespace Trend_Service.Handlers
{
    public class KafkaConsumerHandler : BackgroundService
    {
        private readonly string topic = "trend_topic";
        private readonly IServiceScopeFactory scopeFactory;

        public KafkaConsumerHandler(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
/*
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();
            var trendService = scope.ServiceProvider.GetRequiredService<ITrendService>();

            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            

            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        var consumer = builder.Consume(cancelToken.Token);
                        var trend = JsonSerializer.Deserialize<Trend>(consumer.Message.Value);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                        
                        trendService.CreateTrend(trend);
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }*/

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();
            var trendService = scope.ServiceProvider.GetRequiredService<ITrendService>();

            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            await Task.Run(() =>
            {
                using (var builder = new ConsumerBuilder<Ignore, string>(conf).Build())
                {
                    builder.Subscribe(topic);

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumer = builder.Consume(stoppingToken);
                        var trend = JsonSerializer.Deserialize<Trend>(consumer.Message.Value);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");

                        trendService.CreateTrend(trend);
                    }
                    builder.Close();
                }
            });
        }
    }
}
