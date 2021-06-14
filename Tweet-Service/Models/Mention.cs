using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweet_Service.Models
{
    public class Mention
    {
        public Guid Id { get; set; }
        public Guid TweetId { get; set; }
        public Tweet tweet { get; set; }
        public string UserName { get; set; }

    }
}
