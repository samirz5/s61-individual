using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class Following
    {
        public Guid Id { get; set; }
        public string MyUserName { get; set; }
        public string FollowingUserName { get; set; }
    }
}
