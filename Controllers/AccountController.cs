using Microsoft.AspNetCore.Mvc;
using SignUpProject.Models;
using System.Security.Claims;

namespace SignUpProject.Controllers
{
    public class AccountController : Controller
    {
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();
            
            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        public IActionResult UserProfile()
        {
            return View(new UserProfile()
            {
                Name = User.Identity.Name,
                Email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                Avatar = User.FindFirst(c => c.Type == "picture")?.Value
            });
        }
    }
}
