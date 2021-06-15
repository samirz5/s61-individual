using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class Follower
    {
        public Guid Id { get; set; }
        public string MyUserName { get; set; }
        public string FollowerUserName { get; set; }
    }
}
