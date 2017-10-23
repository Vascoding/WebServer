namespace WebServer.Server.HTTP.Response
{
    using Server.Enums;
    using Common;
    
    public class NotFoundResponse : ViewResponse
    {
        public NotFoundResponse()
            : base(HttpStatusCode.NotFound, new NotFoundView())
        {
            this.StatusCode = HttpStatusCode.NotFound;
        }
    }
}