using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweet_Service.Models
{
    public class TrendDTO
    {
        public Guid TweetId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
