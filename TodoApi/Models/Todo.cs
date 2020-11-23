using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TodoApi.Models
{
    public class Todo
    {
        public long Id { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
