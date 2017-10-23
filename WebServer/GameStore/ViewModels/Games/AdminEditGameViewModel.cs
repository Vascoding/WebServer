namespace WebServer.GameStore.ViewModels.Games
{
    using System;

    public class AdminEditGameViewModel
    {
        public string Title { get; set; }

        public string VideoId { get; set; }

        public string Image { get; set; }

        public double Size { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string RealeaseDate { get; set; }
    }
}