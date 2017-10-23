namespace WebServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using Server.Enums;

    public interface IServerRouteConfig
    {
        Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> Routes { get; }

        ICollection<string> AnonymousPaths { get; }
    }
}