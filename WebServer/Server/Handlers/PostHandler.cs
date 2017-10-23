namespace WebServer.Server.Handlers
{
    using System;
    using HTTP.Contracts;
    public class PostHandler : RequestHandler
    {
        public PostHandler(Func<IHttpRequest, IHttpResponse> handlingFunc) 
            : base(handlingFunc)
        {
        }
    }
}