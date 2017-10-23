namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Server.Enums;
    using Server.Routing.Contracts;
    
    public class ServerRouteConfig : IServerRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> routes;

        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            this.routes = new Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>>();
            this.AnonymousPaths = new List<string>(appRouteConfig.AnonymousPaths);
            var availableMethods = Enum.GetValues(typeof(HttpRequestMethod)).Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                this.routes[method] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeServerConfig(appRouteConfig);
        }

        private void InitializeServerConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var registeredRoute in appRouteConfig.Routes)
            {
                var requestMethod = registeredRoute.Key;
                var routesHandlers = registeredRoute.Value;

                foreach (var routeWithHandler in routesHandlers)
                {
                    var route = routeWithHandler.Key;
                    var handler = routeWithHandler.Value;

                    var paratmeters = new List<string>();

                    var parsedRouteRegex = this.ParseRoute(route, paratmeters);

                    var routeContext = new RoutingContext(handler, paratmeters);

                    this.routes[requestMethod].Add(parsedRouteRegex, routeContext);
                }
            }
        }

        private string ParseRoute(string route, List<string> parameters)
        {
            if (route == "/")
            {
                return "^/$";
            }

            var result = new StringBuilder();

            result.Append("^/");

            if (route == "/")
            {
                result.Append("/$");
                return result.ToString();
            }

            var tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            this.ParseTokens(tokens, parameters, result);

            return result.ToString();


            //if (route == "/")
            //{
            //    return "^/$";
            //}

            //var result = new StringBuilder();

            //result.Append("^/");

            //var tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //this.ParseTokens(tokens, parameters, result);

            //return result.ToString();
        }

        private void ParseTokens(string[] tokens, List<string> paratmeters, StringBuilder result)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                var end = i == tokens.Length - 1 ? "$" : "/";

                var currentToken = tokens[i];

                if (!currentToken.StartsWith('{') && !currentToken.EndsWith('}'))
                {
                    result.Append($"{currentToken}{end}");
                    continue;
                }

                var parameterRegex = new Regex("<\\w+>");
                var parameterMatch = parameterRegex.Match(currentToken);

                if (!parameterMatch.Success)
                {
                    throw new InvalidOperationException($"Route parameter in '{currentToken}' is not valid.");
                }

                var match = parameterMatch.Value;
                var parameter = match.Substring(1, match.Length - 2);

                paratmeters.Add(parameter);

                var currentTokenWithoutBrackets = currentToken.Substring(1, currentToken.Length - 2);

                result.Append($"{currentTokenWithoutBrackets}{end}");
            }
        }

        public Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> Routes => this.routes;
        public ICollection<string> AnonymousPaths { get; private set; }
    }
}