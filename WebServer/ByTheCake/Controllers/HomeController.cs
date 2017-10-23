namespace WebServer.ByTheCake.Controllers
{
    using Server.HTTP.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index() => this.FileViewResponse(@"home\Index");

        public IHttpResponse About() => this.FileViewResponse(@"home\about");
    }
}