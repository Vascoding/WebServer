namespace WebServer.GameStore.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using WebServer.GameStore.ViewModels.Games;

    public interface IGameService
    {
        void Create(
            string title,
            string description,
            string image,
            decimal price,
            double size,
            string videoId,
            DateTime releaseDate);

        IEnumerable<AdminListGameViewModel> All();

        IEnumerable<HomePageListGamesViewMoodel> HomeListGames();

        AdminEditGameViewModel Get(int id);

        void Edit(int id, AdminEditGameViewModel model);

        void Delete(int id);

        AddGameViewModel Details(int id);
    }
}