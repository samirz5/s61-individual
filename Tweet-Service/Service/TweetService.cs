using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Tweet_Service.Context;
using Tweet_Service.Interfaces;
using Tweet_Service.Models;

namespace Tweet_Service.Service
{
    public class TweetService : ITweetService
    {
        private readonly TweetServiceContext _context;
        private static readonly HttpClient client = new HttpClient();
        private readonly string topic = "trend_topic";
        private readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };
        public TweetService(TweetServiceContext context)
        {          
            _context = context;
        }

        public async Task<Tweet> CreateTweet(Tweet tweet)
        {
            tweet.Id = Guid.NewGuid();
            tweet.CreatedDate = DateTime.Now;
            tweet.UpdatedDate = DateTime.Now;

            await _context.AddAsync(tweet);
            await _context.SaveChangesAsync();

            if (tweet.Message.Contains("@"))
            {
                var userNamesInTweet = GetSubStringAfterTag(tweet.Message, "@");
                foreach (var userName in userNamesInTweet)
                {
                    var user = await GetUserByUserName(userName);
                    if(user != null)
                    {
                        // TODO: Check wheter to save userId or userName.
                        await AddMention(userName, tweet.Id);
                    }                   
                    
                }
            }
            
            if (tweet.Message.Contains("#"))
            {
                var trends = GetSubStringAfterTag(tweet.Message, "#");
                foreach (var trend in trends)
                {
                    SendToKafka(topic, trend);
                }
            }

            /*var kafkaContext = new KafkaContext(_config);
            kafkaContext.SendRideToKafkaTopic(ride);*/

            return tweet;
        }

        public Tweet GetById(Guid id)
        {
            return _context.Tweet.Find(id);
        }

        public IEnumerable<Tweet> GetMostRecentTweets()
        {
            return _context.Tweet.OrderByDescending(e => e.CreatedDate);
        }

        public IEnumerable<Tweet> GetTweetsByTrend(string trend)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tweet> GetUserTweets(Guid userId)
        {
            return _context.Tweet.Where(e => e.UserId == userId);
        }

        private List<string> GetSubStringAfterTag(string tweet, string tag)
        {
            string[] words = tweet.Split(" ");
            List<string> subStrings = new List<string>();
            foreach (var word in words)
            {
                if (word.StartsWith(tag))
                {                    
                     subStrings.Add(word.Substring(1));
                }
            }
            return subStrings;
        }

        private async Task AddMention(string userName, Guid tweetId)
        {
            Mention mention = new Mention();
            mention.Id = new Guid();
            mention.TweetId = tweetId;
            mention.UserName = userName;

           await _context.AddAsync(mention);
           await _context.SaveChangesAsync();
        }
        private void AddTrend()
        {
            // send with kafka to trend service.
        }

        public async Task<IEnumerable<Mention>> GetTweetsByMentionAsync()
        {
            //TODO: remove hardcoded and implement getting userName from token or from frontend.
            var tweets = await _context.Mention.Include(x => x.tweet).Where(x => x.UserName == "samir").ToListAsync();
            return tweets.OrderByDescending(x => x.tweet.CreatedDate);
        }

        private async Task<UserDTO> GetUserByUserName(string userName)
        {
            //TODO: remove hardcoded url.
            var content = await client.GetStringAsync("http://localhost:18924/getByUserName/" + userName);
            if (content == "")
            {
                return null;
            }
            return JsonSerializer.Deserialize<UserDTO>(content);

        }

        private void SendToKafka(string topic, string message)
        {
            using (var producer =
                new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var test = producer.ProduceAsync(topic, new Message<Null, string> { Value = message })
                        .GetAwaiter()
                        .GetResult();
                    Console.WriteLine(test);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }
            }
        }
    }
}
