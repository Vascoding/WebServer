namespace WebServer.Application.Controllers
{
    using Application.Views.Home;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using WebServer.Application.Views.User;
    using WebServer.Server;

    public class UserController
    {
        public IHttpResponse RegisterGet()
        {
            return new ViewResponse(HttpStatusCode.Ok, new RegisterView());
        }

        public IHttpResponse RegisterPost(string name)
        {
            return new RedirectResponse($"user/{name}");
        }

        public IHttpResponse Details(string name)
        {
            Model model = new Model
            {
                ["name"] = name
            };
            return new ViewResponse(HttpStatusCode.Ok, new UserDetailsView(model));
        }
    }
}