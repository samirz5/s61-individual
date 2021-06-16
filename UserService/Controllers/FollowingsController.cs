using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Context;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowingsController : ControllerBase
    {
        private readonly UserContext _context;

        public FollowingsController(UserContext context)
        {
            _context = context;
        }


        [HttpGet("/followingUserNames/{userName}")]
        public async Task<ActionResult<IEnumerable<Following>>> GetUserNamesFollowing(string userName)
        {
            return await _context.Following.Where(x => x.MyUserName == userName).ToListAsync();
        }

        [HttpGet("/followersUserNames/{userName}")]
        public async Task<ActionResult<IEnumerable<Following>>> GetUserNamesFollowers(string userName)
        {
            return await _context.Following.Where(x => x.FollowingUserName == userName).ToListAsync();
        }

        // GET: api/Followings
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Following>>> GetFollowing()
        {
            return await _context.Following.ToListAsync();
        }*/

        // GET: api/Followings/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<Following>> GetFollowing(Guid id)
        {
            var following = await _context.Following.FindAsync(id);

            if (following == null)
            {
                return NotFound();
            }

            return following;
        }*/

        // PUT: api/Followings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutFollowing(Guid id, Following following)
        {
            if (id != following.Id)
            {
                return BadRequest();
            }

            _context.Entry(following).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Followings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Following>> Follow(Following following)
        {
            _context.Following.Add(following);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFollowing", new { id = following.Id }, following);
        }

        // DELETE: api/Followings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> UnFollow(Guid id)
        {
            var following = await _context.Following.FindAsync(id);
            if (following == null)
            {
                return NotFound();
            }

            _context.Following.Remove(following);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FollowingExists(Guid id)
        {
            return _context.Following.Any(e => e.Id == id);
        }
    }
}
