﻿namespace WebMVC.Models
{
    public class BookDetailViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string AuthorName { get; set; }

        public string Publisher { get; set; }

        public string ImageUrl { get; set; }

        //public byte[] ImageContent { get; set; }

    }
}