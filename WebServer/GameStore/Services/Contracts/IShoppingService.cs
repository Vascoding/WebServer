namespace WebServer.GameStore.Services.Contracts
{
    using System.Collections.Generic;

    public interface IShoppingService
    {
        void CreateOrder(string userEmail, IEnumerable<int> gameIds);
    }
}