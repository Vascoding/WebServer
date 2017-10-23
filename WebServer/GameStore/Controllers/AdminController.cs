namespace WebServer.GameStore.Controllers
{
    using System;
    using System.Linq;
    using WebServer.GameStore.Services;
    using WebServer.GameStore.Services.Contracts;
    using WebServer.GameStore.ViewModels.Games;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;

    public class AdminController : BaseController
    {
        private readonly IGameService games;

        public AdminController(IHttpRequest request) 
            : base(request)
        {
            this.games = new GameService();
        }

        public IHttpResponse AddGame()
        {
            if (this.Authentication.IsAdmin)
            {
                return this.FileViewResponse(@"/admin/add-game");
            }
            return new RedirectResponse(@"\");
        }

        public IHttpResponse AddGame(AddGameViewModel model)
        {
            if (!this.Authentication.IsAdmin)
            {
                return new RedirectResponse(@"\");
            }

            if (string.IsNullOrWhiteSpace(model.RealeaseDate))
            {
                this.AddError("Release date is required!!!");
                return this.AddGame();
            }

            this.games.Create(model.Title, model.Description, model.Image, model.Price, model.Size, model.VideoId, DateTime.Parse(model.RealeaseDate));

            return new RedirectResponse(@"\admin\games\list");
        }

        public IHttpResponse AllGames()
        {
            if (!this.Authentication.IsAdmin)
            {
                return new RedirectResponse(@"\");
            }

            var result = this.games
                .All()
                .Select(g => $@"<tr>
                                    <td>{g.Id}</td>
                                    <td>{g.Title}</td>
                                    <td>{g.Size:F2} GB</td>
                                    <td>{g.Price:F2} &euro;</td>
                                    <td>
                                        <a class=""btn btn-warning"" href=""/admin/games/edit/{g.Id}"">Edit</a>
                                        <a class=""btn btn-danger"" href=""/admin/games/delete/{g.Id}"">Delete</a>
                                    </td>
                                </tr>");

            var gamesAsHtml = string.Join(Environment.NewLine, result);

            this.ViewData["games"] = gamesAsHtml;

            return this.FileViewResponse(@"/admin/admin-games");
        }

        public IHttpResponse Edit()
        {
            var id = int.Parse(this.Request.UrlParameters["id"]);
            var gameViewModel = this.games.Get(id);
            this.ViewData["titleValue"] = gameViewModel.Title;
            this.ViewData["descriptionValue"] = gameViewModel.Description;
            this.ViewData["imageValue"] = gameViewModel.Image;
            this.ViewData["videoIdValue"] = gameViewModel.VideoId;
            this.ViewData["priceValue"] = gameViewModel.Price.ToString();
            this.ViewData["sizeValue"] = gameViewModel.Size.ToString();
            this.ViewData["releaseDateValue"] = gameViewModel.RealeaseDate;
            return this.FileViewResponse(@"/admin/edit-game");
        }

        public IHttpResponse Edit(AdminEditGameViewModel model)
        {
            if (!this.Authentication.IsAdmin)
            {
                return new RedirectResponse(@"\");
            }
            var id = int.Parse(this.Request.UrlParameters["id"]);
            if (string.IsNullOrWhiteSpace(model.RealeaseDate)) 
            {
                this.AddError("Release date is required!!!");
                return this.Edit();
            }
            this.games.Edit(id, model);
            return new RedirectResponse(@"/admin/games/list");
        }

        public IHttpResponse Delete()
        {
            var id = int.Parse(this.Request.UrlParameters["id"]);
            var gameViewModel = this.games.Get(id);
            this.ViewData["titleValue"] = gameViewModel.Title;
            this.ViewData["descriptionValue"] = gameViewModel.Description;
            this.ViewData["imageValue"] = gameViewModel.Image;
            this.ViewData["videoIdValue"] = gameViewModel.VideoId;
            this.ViewData["priceValue"] = gameViewModel.Price.ToString();
            this.ViewData["sizeValue"] = gameViewModel.Size.ToString();
            this.ViewData["releaseDateValue"] = gameViewModel.RealeaseDate;
            return this.FileViewResponse(@"/admin/delete-game");
        }

        public IHttpResponse Delete(int id)
        {
            this.games.Delete(id);

            return new RedirectResponse(@"/admin/games/list");
        }
    }
}