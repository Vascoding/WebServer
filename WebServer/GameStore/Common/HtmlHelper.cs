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
                <a class=""card-button btn btn-primary"" name=""buy"" href=""\user\cart\add\{id}"" >Buy</a>
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
                <a class=""card-button btn btn-primary"" name=""buy"" href=""\user\cart\add\{id}"" >Buy</a>
                </div>
                </div>
                ";
        }

        public string ShoppingCartGamesViewPattern(string title, string description, decimal price, string image)
        {
            return $@"<div class=""list-group-item"">
                <div class=""media"" >
                <a class=""btn btn-outline-danger btn-lg align-self-center mr-3"" href =""#"" >X</a>
                <img class=""d-flex mr-4 align-self-center img-thumbnail"" height=""127"" src= ""{image}""
            width = ""227"" alt =""Generic placeholder image"">
                <div class=""media-body align-self-center"" >
                <a href = ""#"">
                <h4 class=""mb-1 list-group-item-heading"" >{title}</h4>
                </a>
                <p>{description}</p>
                </div>
                <div class=""col-md-2 text-center align-self-center mr-auto"" >
                <h2> {price}&euro; </h2>
                </div>
                </div>
                </div>";
        }
    }
}