using Login_Service.Models;
using Login_Service.Service;
using Login_Service.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = userService.Authenticate(user.UserName, user.Password);
            return new JsonResult(token);            
        }

        [HttpPost]
        [Route("/Register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userService.Register(user);
            return Ok();
            
        }
    }
}
