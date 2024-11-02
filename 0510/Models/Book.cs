﻿namespace _0510.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }


        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
