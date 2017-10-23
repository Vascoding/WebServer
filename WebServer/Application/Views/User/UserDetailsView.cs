namespace WebServer.Application.Views.User
{
    using WebServer.Server;
    using WebServer.Server.Contracts;
    public class UserDetailsView : IView
    {
        private readonly Model model;

        public UserDetailsView(Model model)
        {
            this.model = model;
        }

        public string View()
        {
            return $"<h1>Hello, {this.model["name"]}!</h1>";
        }
    }
}