namespace WebServer.Server.HTTP
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using global::WebServer.Server.HTTP.Contracts;
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            this.cookies[cookie.Key] = cookie;
        }

        public bool ContainsKey(string key)
        {
            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            if (!this.cookies.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key {key} was not present in the cookie collection.");
            }
            return this.cookies[key];
        }

        public IEnumerator<HttpCookie> GetEnumerator()
            => this.cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}