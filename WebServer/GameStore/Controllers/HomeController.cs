namespace WebServer.GameStore.Controllers
{
    using System;
    using System.Linq;
    using WebServer.GameStore.Common;
    using WebServer.GameStore.Services;
    using WebServer.GameStore.Services.Contracts;
    using WebServer.GameStore.ViewModels.Games;
    using WebServer.Server.HTTP.Contracts;

    public class HomeController : BaseController
    {
        private readonly IGameService games;

        public HomeController(IHttpRequest request) 
            : base(request)
        {
            this.games = new GameService();
        }

        public IHttpResponse Index()
        {
            
            var result = this.games.HomeListGames()
                .Select(g => new HtmlHelper().HomeListGamesPattern(g.Id, g.Title, g.Price, g.Size, g.Description, g.Image));

            if (this.Authentication.IsAdmin)
            {
                result = this.games.HomeListGames()
                    .Select(g =>
                        new HtmlHelper().AdminHomeListGamesPattern(g.Id, g.Title, g.Price, g.Size, g.Description,
                            g.Image));
            }
            var gamesAsHtml = string.Join(Environment.NewLine, result);

            this.ViewData["result"] = gamesAsHtml;

            return this.FileViewResponse(@"\home\index");
        }

        public IHttpResponse Details()
        {
            var id = int.Parse(this.Request.UrlParameters["id"]);
            var model = this.games.Details(id);
            this.ViewData["titleValue"] = model.Title;
            this.ViewData["descriptionValue"] = model.Description;
            this.ViewData["imageValue"] = model.Image;
            this.ViewData["videoIdValue"] = model.VideoId;
            this.ViewData["priceValue"] = model.Price.ToString();
            this.ViewData["sizeValue"] = model.Size.ToString();
            this.ViewData["releaseDateValue"] = model.RealeaseDate;
            this.ViewData["authDisplay"] = "none";
            this.ViewData["gameId"] = $"{id}";
            if (this.Authentication.IsAdmin)
            {
                this.ViewData["authDisplay"] = "inline";
            }
            
            return this.FileViewResponse($@"\home\game-details");
        }
    }
}