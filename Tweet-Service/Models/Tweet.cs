using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweet_Service.Models
{
    public class Tweet
    {

        public Tweet()
        {
            this.CreatedDate = new DateTime();
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int HeartCount { get; set; }

        public List<Mention> Mentions { get; set; }

    }
}
