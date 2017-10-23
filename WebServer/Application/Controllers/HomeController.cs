namespace WebServer.Application.Controllers
{
    using System;
    using WebServer.Application.Views.Home;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;
    using Server.Enums;
    using WebServer.Server.HTTP;

    public class HomeController 
    {
        public IHttpResponse Index()
        {
            var result = new ViewResponse(HttpStatusCode.Ok, new IndexView());

            result.Cookies.Add(new HttpCookie("lang", "en"));

            return result;
        }

        public IHttpResponse SessionTest(IHttpRequest request)
        {
            var session = request.Session;

            var sessionDateKey = "Saved_Date";

            if (session.Get(sessionDateKey) == null)
            {
                session.Add(sessionDateKey, DateTime.UtcNow);
            }

            return new ViewResponse(HttpStatusCode.Ok, new SessionTestView(session.Get<DateTime>(sessionDateKey)));
        }
    }
}