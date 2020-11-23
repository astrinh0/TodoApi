using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Task
    {
        public long UserId { get; set; }
        public long TodoId { get; set; }

        public User User { get; set; }
        public Todo Todo { get; set; }
    }
}
