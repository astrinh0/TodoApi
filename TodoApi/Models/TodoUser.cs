namespace TodoApi.Models
{
    public class TodoUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public string Secret { get; set; }
    }

    public class TodoUserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}