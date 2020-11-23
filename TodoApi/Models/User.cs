using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{

    //Data from User
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DefaultValue(false)]
        public string Job { get; set; }

        public string Secret { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }

    // Data from User without a secret
    public class UserDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Job { get; set; }
        public ICollection<Task> Tasks { get; set; }


    }

 

  
}