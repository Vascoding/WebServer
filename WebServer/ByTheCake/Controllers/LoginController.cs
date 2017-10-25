namespace WebServer.ByTheCake.Controllers
{
    using System;
    using System.Collections.Generic;
    using WebServer.ByTheCake.Services;
    using WebServer.ByTheCake.Services.Contracts;
    using WebServer.ByTheCake.ViewModels;
    using WebServer.ByTheCake.ViewModels.Account;
    using WebServer.Infrastructure;
    using WebServer.Server.HTTP;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;

    public class LoginController : BaseController
    {
        private const string userName = "username";
        private const string password = "password";
        private const string invalidInput = "Invalid username or password";
        private readonly IUserService users;

        public LoginController()
        {
            this.users = new UserService();
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(@"Login\login");
        }


        public IHttpResponse Login(IHttpRequest request, LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                this.AddError("You have empty fields");
                return this.FileViewResponse(@"Login\login");
            }

            var success = this.users.FindUser(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);
                return new RedirectResponse("/");
            }
            else
            {
                this.ViewData["authDisplay"] = "none";
                this.AddError("Invalid user details");

                return this.FileViewResponse(@"Login\login");
            }
        }

        public IHttpResponse Profile(IHttpRequest req)
        {
            if (!req.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is no logged in user.");
            }

            var username = req.Session.Get<string>(SessionStore.CurrentUserKey);

            var profile = this.users.Profile(username);

            if (profile == null)
            {
                throw new InvalidOperationException($"The user {username} could not be found in the database");
            }

            this.ViewData["username"] = profile.Username;
            this.ViewData["registrationDate"] = profile.RegistrationDate.ToShortDateString();
            this.ViewData["totalOrders"] = profile.TotalOrders.ToString();

            return this.FileViewResponse(@"login\profile");
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(@"Login\register");
        }

        public IHttpResponse Register(IHttpRequest request, RegisterUserVIewModel model)
        {
            this.SetDefaultViewData();
            if (model.Username.Length < 3 || model.Password.Length < 3 || model.ConfirmPassword != model.Password)
            {
                this.AddError("Invalid user details");

                return this.FileViewResponse(@"/login/register");
            }

            var success = this.users.Create(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.AddError("This username is taken!");

                return this.FileViewResponse(@"/login/register");
            }
        }

        private void SetDefaultViewData()
        {
            this.ViewData["showError"] = "none";
            this.ViewData["authDisplay"] = "none";
        }

        private void LoginUser(IHttpRequest request, string name)
        {
            request.Session.Add(SessionStore.CurrentUserKey, name);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}