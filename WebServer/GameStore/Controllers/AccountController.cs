namespace WebServer.GameStore.Controllers
{
    using WebServer.GameStore.Services;
    using WebServer.GameStore.Services.Contracts;
    using WebServer.GameStore.ViewModels.Account;
    using WebServer.GameStore.ViewModels.Orders;
    using WebServer.Infrastructure;
    using WebServer.Server.HTTP;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;

    public class AccountController : BaseController
    {
        private readonly IUserService users;

        public AccountController(IHttpRequest request) : base(request)
        {
            this.users = new UserService();
        }

        public IHttpResponse Register()
        {
            return this.FileViewResponse(@"\account\register");
        }

        public IHttpResponse Register(RegisterViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Register();
            }

            this.users.Create(model.Email, model.FullName, model.ConfirmPassword);

            this.LoginUser(model.Email);
            return new RedirectResponse("/");
        }

        public IHttpResponse Login()
        {
            return this.FileViewResponse(@"\account\login");
        }

        public IHttpResponse Login( LoginUserViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Password))
            {
                var success = this.users.FindUser(model.Email, model.Password);

                if (success)
                {
                    this.LoginUser(model.Email);
                    return new RedirectResponse("/");
                }
            }


            return this.FileViewResponse(@"\account\login-error");
        }

        public IHttpResponse Logout()
        {
            this.Request.Session.Clear();

            return new RedirectResponse("/");
        }

        private void LoginUser(string email)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, email);
            this.Request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}