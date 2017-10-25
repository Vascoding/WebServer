namespace WebServer.GameStore.ViewModels.Orders
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public const string SessionKey = "^%Current_Shopping_Cart%^";

        public List<int> GameIds { get; private set; } = new List<int>();
    }

}