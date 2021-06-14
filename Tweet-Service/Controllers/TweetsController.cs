using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweet_Service.Context;
using Tweet_Service.Interfaces;
using Tweet_Service.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tweet_Service.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("tweets")]
    [ApiController]
    public class TweetsController : ControllerBase
    {

        private readonly ITweetService _tweetService;
        private readonly TweetServiceContext _context;
        

        public TweetsController(ITweetService tweetService, TweetServiceContext context)
        {
            _tweetService = tweetService;
            _context = context;
        }

        // GET: api/<TweetsController>
        [HttpGet("/userTweets/{id}")]
        public ActionResult<IEnumerable<Tweet>> GetUserTweets(Guid id)
        {
            var tweets = _tweetService.GetUserTweets(id);
            return Ok(tweets);
        }

        // GET: api/<TweetsController>
        [HttpGet("/recentTweets")]
        public ActionResult<IEnumerable<Tweet>> GetRecentTweets()
        {
            var tweets = _tweetService.GetMostRecentTweets();
            return Ok(tweets);
        }

        // GET: api/<TweetsController>
        [HttpGet("/mentionedTweets")]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetMentionedTweetsAsync()
        {
            var tweets = await _tweetService.GetTweetsByMentionAsync();
            return Ok(tweets);
        }

        // GET api/<TweetsController>/5
        [HttpGet("{id}")]
        public ActionResult<Tweet> Get(Guid id)
        {
            var tweet = _tweetService.GetById(id);

            if (tweet == null)
            {
                return NotFound();
            }

            return Ok(tweet);
        }

        // POST api/<TweetsController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Tweet tweet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTweet = await _tweetService.CreateTweet(tweet);
                return CreatedAtAction(nameof(Get), new { id = createdTweet.Id }, createdTweet);
            }
            catch (UnauthorizedAccessException)
            {
                return new UnauthorizedResult();
            }
        }

        

        // PUT api/<TweetsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(Guid id, [FromBody] Tweet tweet)
        {
            if (id != tweet.Id)
            {
                return BadRequest();
            }
            tweet.Id = id;
            _context.Entry(tweet).State = EntityState.Modified;
            try
            {
                 _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TweetExists(id))
                {
                    return NotFound();
                }
                else throw;
            }
            return Ok("Updated");
        }

        // DELETE api/<TweetsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var todoItem = await _context.Tweet.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Tweet.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TweetExists(Guid id) =>
            _context.Tweet.Any(e => e.Id == id);
    }
}
