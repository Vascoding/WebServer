namespace WebServer.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Server.Enums;
    using Server.Exeptions;
    using Server.HTTP.Contracts;
    
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.HeaderCollection = new HttpHeaderCollection();
            this.UrlParameters = new Dictionary<string, string>();
            this.QueryParameters = new Dictionary<string, string>();
            this.FormData = new Dictionary<string, string>();
            this.CookieCollection = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }
        
        public Dictionary<string, string> FormData { get; set; }

        public IHttpHeaderCollection HeaderCollection { get; }

        public IHttpCookieCollection CookieCollection { get; private set; }

        public string Path { get; private set; }

        public Dictionary<string, string> QueryParameters { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, string> UrlParameters { get; }

        public IHttpSession Session { get; set; }

        public void AddUrlParameter(string key, string value)
        {
            this.UrlParameters[key] = value;
        }
        
        private void ParseRequest(string requestString)
        {
            var requestLines = requestString.Split(Environment.NewLine);

            var requestLine = requestLines[0].Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid request line");
            }

            this.RequestMethod = this.ParseRequestMethod(requestLine[0].ToUpper());

            this.Url = requestLine[1];
            this.Path = this.Url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];

            this.ParseHeaders(requestLines);

            this.ParseCookies();
            
            this.ParseParameters();

            if (this.RequestMethod == HttpRequestMethod.Post)
            {
                this.ParseQuery(requestLines[requestLines.Length - 1], this.FormData);
            }

            this.SetSession();
        }

        private void ParseCookies()
        {
            if (this.HeaderCollection.ContainsKey(HttpHeader.Cookie))
            {
                var allCookies = this.HeaderCollection.GetHeader(HttpHeader.Cookie);

                foreach (var cookie in allCookies)
                {
                    if (!cookie.Value.Contains("="))
                    {
                        return;
                    }

                    var cookieParts = cookie
                        .Value
                        .Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    if (!cookieParts.Any())
                    {
                        continue;
                    }

                    foreach (var cookiePart in cookieParts)
                    {
                        var cookieKeyValuePair = cookiePart
                            .Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                        if (cookieKeyValuePair.Length == 2)
                        {
                            var key = cookieKeyValuePair[0].Trim();
                            var value = cookieKeyValuePair[1].Trim();

                            this.CookieCollection.Add(new HttpCookie(key, value, false));
                        }
                    }
                }
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }
            var query = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[1];
            this.ParseQuery(query, this.QueryParameters);
        }

        private void ParseQuery(string query, Dictionary<string, string> dict)
        {
            if (!query.Contains("="))
            {
                return;
            }

            var queryFairs = query.Split('&');

            foreach (var queryFair in queryFairs)
            {
                var queryArgs = queryFair.Split('=');
                if (queryArgs.Length != 2)
                {
                    continue;
                }

                dict.Add(WebUtility.UrlDecode(queryArgs[0]), WebUtility.UrlDecode(queryArgs[1]));
            }
        }

        private void ParseHeaders(string[] requestLines)
        {
            int endIndex = Array.IndexOf(requestLines, string.Empty);
            for (int i = 1; i < endIndex; i++)
            {
                var headersArgs = requestLines[i].Split(new[] { ": " }, StringSplitOptions.None);

                this.HeaderCollection.Add(new HttpHeader(headersArgs[0], headersArgs[1].Trim()));
            }

            if (this.HeaderCollection.GetHeader("Host") == null)
            {
                throw new BadRequestException();
            }
        }

        private HttpRequestMethod ParseRequestMethod(string requestLine)
        {
            if (requestLine != "POST" && requestLine != "GET")
            {
                throw new BadRequestException();
            }
            return (HttpRequestMethod)Enum.Parse(typeof(HttpRequestMethod), requestLine, true);
        }

        private void SetSession()
        {

            if (this.CookieCollection.ContainsKey(SessionStore.sessionCookieKey))
            {
                var cookie = this.CookieCollection.GetCookie(SessionStore.sessionCookieKey);

                var sessionId = cookie.Value;

                this.Session = SessionStore.Get(sessionId);
            }
        }
    }
}