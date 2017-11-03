namespace WebServer.ByTheCake.Controllers
{
    using Server.HTTP.Contracts;

    public class HomeController : BaseController
    {
        private const string HomeIndex = @"home\Index";

        public IHttpResponse Index() => this.FileViewResponse(HomeIndex);

        public IHttpResponse About() => this.FileViewResponse(HomeIndex);
    }
}