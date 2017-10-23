namespace WebServer.GameStore.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string VideoId { get; set; }

        public string Image { get; set; }

        public double Size { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime RealeaseDate { get; set; }

        public List<UserGame> Users { get; set; } = new List<UserGame>();
    }
}