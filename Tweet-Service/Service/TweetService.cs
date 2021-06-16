using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
        private readonly UserService _userService;
        private readonly TrendService _trendService;
        private readonly KafkaProducer _kafkaProducer;
        private readonly string trendTopic = "trend_topic";

        public TweetService(TweetServiceContext context, IConfiguration config)
        {
            _context = context;
            _userService = new UserService(config);
            _trendService = new TrendService(config);
            _kafkaProducer = new KafkaProducer();
        }

        #region Getting Tweets
        public IEnumerable<Tweet> GetMostRecentTweets()
        {
            return _context.Tweet.OrderByDescending(e => e.CreatedDate);
        }
        public async Task<IEnumerable<Tweet>> GetTweetsOfFollowers(string userName)
        {
            List<string> userNamesListFollowing = await _userService.GetUserNamesFollowing(userName);
            userNamesListFollowing.Add(userName);

            /*string endPoint = "getFollowingUserNames/";
            var content = await client.GetStringAsync(_urlUserService + endPoint + userName);
            var userNamesList = JsonSerializer.Deserialize<List<string>>(content);
            userNamesList.Add(userName);*/

            /*var tweets = (from p in _context.Tweet
                          where userNamesListFollowing.Any(w => p.UserName.Contains(w))
                          select p).ToList();*/



            var tweets = new List<Tweet>();
            foreach (var item in userNamesListFollowing)
            {
                var tweetsOfUser = await _context.Tweet.Where(x => x.UserName == item).ToListAsync();
                tweets.AddRange(tweetsOfUser);
            }

            return tweets.OrderByDescending(x => x.CreatedDate);
        }
        public IEnumerable<Tweet> GetUserTweets(string userName)
        {
            return _context.Tweet.Where(e => e.UserName == userName);
        }
        public async Task<IEnumerable<Mention>> GetTweetsByMentionAsync(string userName)
        {
            var tweets = await _context.Mention.Include(x => x.Tweet).Where(x => x.UserName == userName).ToListAsync();
            return tweets.OrderByDescending(x => x.Tweet.CreatedDate);
        }
        public async Task<IEnumerable<Tweet>> GetTweetsByTrend(string trend)
        {
            List<Guid> tweetIds = await _trendService.GetTweetIdsOfTrend(trend);

            var tweets = _context.Tweet
                               .Where(t => tweetIds.Contains(t.Id));
            return tweets;
        }
        #endregion

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

        #region Private Functions
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
        private async Task CheckTweetForTagsAsync(Tweet tweet)
        {
            if (tweet.Message.Contains("@"))
            {
                var userNamesInTweet = GetSubStringAfterTag(tweet.Message, "@", false);
                foreach (var userName in userNamesInTweet)
                {
                    var user = await _userService.GetUserByUserName(userName);
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
                   _kafkaProducer.SendTrendToKafka(trendTopic, trendDto);
                }
            }
        }
        #endregion








    }

}
