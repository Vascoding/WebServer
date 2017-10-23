namespace WebServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using Enums;
    using Handlers;
    
    public interface IAppRouteConfig
    {
        Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> Routes { get; }

        void AddRoute(string route, RequestHandler httpHandler);

        ICollection<string> AnonymousPaths { get; }
    }
}