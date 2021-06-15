using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
        private readonly string _urlUserService;
        private readonly TweetServiceContext _context;
        private static readonly HttpClient client = new();
        private readonly string topic = "trend_topic";
        private readonly ProducerConfig config = new()
        {
            BootstrapServers = "localhost:9092"
        };
        public TweetService(TweetServiceContext context, IConfiguration config)
        {          
            _context = context;
            _config = config;
            _urlUserService = config.GetValue<string>("Url:UserService");

        }

        public async Task<Tweet> CreateTweet(Tweet tweet)
        {
            tweet.Id = Guid.NewGuid();
            tweet.CreatedDate = DateTime.Now;
            tweet.UpdatedDate = DateTime.Now;

            await _context.AddAsync(tweet);
            await _context.SaveChangesAsync();

            await CheckTweetForTagsAsync(tweet);

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

        private List<string> GetSubStringAfterTag(string tweet, string tag, bool withTag)
        {
            string[] words = tweet.Split(" ");
            List<string> subStrings = new List<string>();
            foreach (var word in words)
            {
                if (word.StartsWith(tag) && withTag)
                {
                    subStrings.Add(word);
                }
                if (word.StartsWith(tag) && !withTag)
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

        public async Task<IEnumerable<Mention>> GetTweetsByMentionAsync(string userName)
        {
            //TODO: remove hardcoded and implement getting userName from token or from frontend.
            var tweets = await _context.Mention.Include(x => x.tweet).Where(x => x.UserName == userName).ToListAsync();
            return tweets.OrderByDescending(x => x.tweet.CreatedDate);
        }

        private async Task CheckTweetForTagsAsync(Tweet tweet)
        {
            if (tweet.Message.Contains("@"))
            {
                var userNamesInTweet = GetSubStringAfterTag(tweet.Message, "@", false);
                foreach (var userName in userNamesInTweet)
                {
                    var user = await GetUserByUserName(userName);
                    if (user != null)
                    {
                        // TODO: Check wheter to save userId or userName.
                        await AddMention(userName, tweet.Id);
                    }

                }
            }

            if (tweet.Message.Contains("#"))
            {
                var trends = GetSubStringAfterTag(tweet.Message, "#", true);
                foreach (var trend in trends)
                {
                    TrendDTO trendDto = new()
                    {
                        TweetId = tweet.Id,
                        Name = trend,
                        CreatedDate = tweet.CreatedDate
                    };
                    SendTrendToKafka(topic, trendDto);
                }
            }
        }

        private async Task<UserDTO> GetUserByUserName(string userName)
        {
            string endPoint = "getByUserName/";
            //TODO: remove hardcoded url.
            var content = await client.GetStringAsync(_urlUserService + endPoint + userName);
            if (content == "")
            {
                return null;
            }
            return JsonSerializer.Deserialize<UserDTO>(content);

        }

        private void SendTrendToKafka(string topic, TrendDTO message)
        {
            using var producer =
                new ProducerBuilder<Null, string>(config).Build();
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

        public Task<IEnumerable<Tweet>> GetTweetsOfFollowers()
        {


            return null;
        }
    }
}
