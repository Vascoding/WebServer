namespace WebServer.GameStore.Common
{
    public class HtmlHelper
    {
        public string HomeListGamesPattern(int id, string title, decimal price, double size, string description, string image)
        {
            return $@"
                <div class=""card col-4 thumbnail"">
                <img
                class=""card-image-top img-fluid img-thumbnail""
            onerror=""this.src='https://i.ytimg.com/vi/BqJyluskTfM/maxresdefault.jpg';""
            src=""{image}"">
                <div class=""card-body"">
                <h4 class=""card-title"">{title}</h4>
                <p class=""card-text""><strong>Price</strong> - {price:f2}&euro;</p>
                <p class=""card-text""><strong>Size</strong> - {price:f2} GB</p>
                <p class=""card-text"" >{description}</p>
                </div>
                
                <div class=""card-footer"">
                <a class=""card-button btn btn-outline-primary"" name= ""info"" href= ""\games\details\{id}"">Info</a>
                <a class=""card-button btn btn-primary"" name=""buy"" href=""#"" >Buy</a>
                </div>
                </div>
                ";
        }

        public string AdminHomeListGamesPattern(int id, string title, decimal price, double size, string description, string image)
        {
            return $@"
                <div class=""card col-4 thumbnail"">
                <img
                class=""card-image-top img-fluid img-thumbnail""
            onerror=""this.src='https://i.ytimg.com/vi/BqJyluskTfM/maxresdefault.jpg';""
            src=""{image}"">
                <div class=""card-body"">
                <h4 class=""card-title"">{title}</h4>
                <p class=""card-text""><strong>Price</strong> - {price:f2}&euro;</p>
                <p class=""card-text""><strong>Size</strong> - {price:f2} GB</p>
                <p class=""card-text"" >{description}</p>
                </div>
                <div class=""card-footer"">
                <a class=""btn btn-warning"" href=""/admin/games/edit/{id}"">Edit</a>
                <a class=""btn btn-danger"" href=""/admin/games/delete/{id}"">Delete</a>
                <a class=""card-button btn btn-outline-primary"" name= ""info"" href= ""\games\details\{id}"">Info</a>
                <a class=""card-button btn btn-primary"" name=""buy"" href=""#"" >Buy</a>
                </div>
                </div>
                ";
        }
    }
}