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
        IEnumerable<Tweet> GetUserTweets(string userName);
        IEnumerable<Tweet> GetMostRecentTweets();
        Task<IEnumerable<Tweet>> GetTweetsByTrend(string trend);
        Task<IEnumerable<Mention>> GetTweetsByMentionAsync(string userName);

        Task<IEnumerable<Tweet>> GetTweetsOfFollowers(string userName);


    }
}
