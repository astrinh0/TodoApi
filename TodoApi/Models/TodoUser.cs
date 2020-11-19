namespace TodoApi.Models
{

    //Data from User
    public class TodoUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public string Secret { get; set; }
    }

    // Data from User without a secret
    public class TodoUserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}