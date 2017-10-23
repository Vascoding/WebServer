namespace WebServer.Server.HTTP.Contracts
{
    using System.Collections.Generic;

    public interface IHttpHeaderCollection : IEnumerable<ICollection<HttpHeader>>
    {
        void Add(HttpHeader header);

        bool ContainsKey(string key);

        ICollection<HttpHeader> GetHeader(string key);
    }
}