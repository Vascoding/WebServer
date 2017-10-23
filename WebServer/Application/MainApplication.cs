namespace WebServer.Application
{
    using WebServer.Application.Controllers;
    using WebServer.Server.Contracts;
    using WebServer.Server.Handlers;
    using WebServer.Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .AddRoute("/", new GetHandler(request => new HomeController().Index()));

            appRouteConfig
                .AddRoute("/testsession", new GetHandler(req => new HomeController().SessionTest(req)));

            appRouteConfig
                .AddRoute("/register", new GetHandler(httpContext => new UserController().
                    RegisterGet()));

            appRouteConfig
                .AddRoute("/register", new PostHandler(httpContext => new UserController()
                .RegisterPost(httpContext.FormData["name"])));

            appRouteConfig
                .AddRoute("/user/{(?<name>[a-z]+)}", new GetHandler(httpContext => 
                new UserController()
                .Details(httpContext.UrlParameters["name"])));
        }
    }
}