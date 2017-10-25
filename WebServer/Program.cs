namespace WebServer
{
    using System;
    using WebServer.ByTheCake;
    using WebServer.GameStore;
    using WebServer.Server;
    using WebServer.Server.Contracts;
    using WebServer.Server.Routing;

    public class Program : IRunnable
    {
        public static void Main()
        {
            new Program().Run();
        }

        public void Run()
        {
            var mainApplication = new GameStoreApplication();
            mainApplication.InitializeDatabase();
            var appRouteConfig = new AppRouteConfig();
            mainApplication.Configure(appRouteConfig);
            var webServer = new WebServer(1337, appRouteConfig);
            webServer.Run();
        }
    }
}
