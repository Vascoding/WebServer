﻿namespace WebServer.Server.Common
{
    using Contracts;
    public class NotFoundView : IView
    {
        public string View()
        {
            return "<h1>404 This page does not exists :/</h1>";
        }
    }
}