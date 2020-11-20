using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{

    //Data from User
    public class TodoUser
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool IsComplete { get; set; }

        public string Secret { get; set; }
    }

    // Data from User without a secret
    public class TodoUserDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
}