using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Tasks
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long TodoId { get; set; }

        public User User { get; set; }
        public Todo Todo { get; set; }
    }
}
