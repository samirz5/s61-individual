using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trend_Service.Models
{
    public class Trend
    {
        public Guid Id { get; set; }
        public Guid TweetId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
