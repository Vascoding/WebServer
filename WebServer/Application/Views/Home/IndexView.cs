namespace WebServer.Application.Views.Home
{
    using WebServer.Server.Contracts;
    public class IndexView : IView
    {
        public string View() => "<h1>Welcome!</h1>";
    }
}