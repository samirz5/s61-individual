using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweet_Service.Models
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Location { get; set; }
        public string Picture { get; set; }
    }
}
