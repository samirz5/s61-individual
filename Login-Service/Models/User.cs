using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login_Service.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
