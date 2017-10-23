namespace WebServer.Server.Routing
{
    using System.Collections.Generic;
    using Handlers;
    using Contracts;
    
    public class RoutingContext : IRoutingContext
    {
        public RoutingContext(RequestHandler handler, IEnumerable<string> parameters)
        {
            this.Handler = handler;
            this.Parameters = parameters;
        }

        public IEnumerable<string> Parameters { get; private set; }
        public RequestHandler Handler { get; private set; }
    }
}