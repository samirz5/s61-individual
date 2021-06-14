using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweet_Service.Models;

namespace Tweet_Service.Interfaces
{
    public interface ITweetService
    {
        Task<Tweet> CreateTweet(Tweet tweet);
        Tweet GetById(Guid id);
        IEnumerable<Tweet> GetUserTweets(Guid userId);
        IEnumerable<Tweet> GetMostRecentTweets();
        IEnumerable<Tweet> GetTweetsByTrend(string trend);
        Task<IEnumerable<Mention>> GetTweetsByMentionAsync();
    }
}
