namespace WebServer.ByTheCake.Controllers
{
    using WebServer.Infrastructure;
    public abstract class BaseController : Controller
    {
        protected override string ApplicationDirectory => "ByTheCake";
    }
}