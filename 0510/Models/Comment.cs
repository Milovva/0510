namespace _0510.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }


        public Guid BookId { get; set; }


        public Book Book { get; set; } 
    }
}
