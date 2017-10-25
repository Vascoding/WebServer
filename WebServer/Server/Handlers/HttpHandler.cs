namespace WebServer.Server.Handlers
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::WebServer.Server.HTTP;
    using Server.Handlers.Contracts;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Server.Routing.Contracts;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig routeConfig)
        {
            this.serverRouteConfig = routeConfig;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            try
            {
                var anonymousPaths = this.serverRouteConfig.AnonymousPaths;

                if (!anonymousPaths.Contains(context.Request.Path) &&
                    (context.Request.Session == null || !context.Request.Session.Contains(SessionStore.CurrentUserKey)))
                {
                    return new RedirectResponse(anonymousPaths.First());
                }

                var requestMethod = context.Request.RequestMethod;
                var requestPath = context.Request.Path;
                var registeredRoutes = this.serverRouteConfig.Routes[requestMethod];
                

                foreach (var registeredRoute in registeredRoutes)
                {
                    var routePattern = registeredRoute.Key;
                    var routingContext = registeredRoute.Value;

                    var routeRegex = new Regex(routePattern);
                    var match = routeRegex.Match(requestPath);

                    if (!match.Success)
                    {
                        continue;
                    }

                    var parameters = routingContext.Parameters;

                    foreach (var parameter in parameters)
                    {
                        var parameterValue = match.Groups[parameter].Value;
                        context.Request.AddUrlParameter(parameter, parameterValue);
                    }

                    return routingContext.Handler.Handle(context);
                }
            }
            catch (Exception e)
            {
                return new InternelServerErrorResponse(e);
            }

            return new NotFoundResponse();
        }
    }
}