namespace WebServer.ByTheCake.Controllers
{
    using System;
    using WebServer.ByTheCake.Services;
    using WebServer.ByTheCake.Services.Contracts;
    using WebServer.ByTheCake.ViewModels;
    using WebServer.ByTheCake.ViewModels.Account;
    using WebServer.Server.HTTP;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;

    public class LoginController : BaseController
    {
        private const string InvalidUserDetails = "Invalid user details";
        private const string TakenUsername = "This username is taken!";
        private const string EemptyFieldsError = "You have empty fields";
        private const string LoginPath = @"Login\login";
        private const string RegisterPath = @"Login\register";
        private const string ProfilePath = @"login\profile";
        private const string NotLoggedInUser = "There is no logged in user.";
        private const string RedirectUrl = "/";


        private readonly IUserService users;

        public LoginController()
        {
            this.users = new UserService();
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(LoginPath);
        }


        public IHttpResponse Login(IHttpRequest request, LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                this.AddError(EemptyFieldsError);
                return this.FileViewResponse(LoginPath);
            }

            var success = this.users.FindUser(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);
                return new RedirectResponse(RedirectUrl);
            }
            else
            {
                this.ViewData["authDisplay"] = "none";
                this.AddError(InvalidUserDetails);

                return this.FileViewResponse(LoginPath);
            }
        }

        public IHttpResponse Profile(IHttpRequest req)
        {
            if (!req.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException(NotLoggedInUser);
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

            return this.FileViewResponse(ProfilePath);
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(RegisterPath);
        }

        public IHttpResponse Register(IHttpRequest request, RegisterUserVIewModel model)
        {
            this.SetDefaultViewData();
            if (model.Username.Length < 3 || model.Password.Length < 3 || model.ConfirmPassword != model.Password)
            {
                this.AddError(InvalidUserDetails);

                return this.FileViewResponse(RegisterPath);
            }

            var success = this.users.Create(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse(RedirectUrl);
            }
            else
            {
                this.AddError(TakenUsername);

                return this.FileViewResponse(RegisterPath);
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