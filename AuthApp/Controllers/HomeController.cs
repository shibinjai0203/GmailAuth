using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index(){
            return View();
        }

        // public IActionResult Login(){
        //     Login1("https://www.c-sharpcorner.com/article/implement-gmail-and-facebook-based-authentication-in-asp-net-core-2-2/");
        //     return View();
        // }

        public IActionResult Login(string returnUrl) =>
            new ChallengeResult(
                GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(LoginCallback), new { returnUrl })
                });

        public async Task<IActionResult> LoginCallback(string returnUrl)
        {
            if(string.IsNullOrEmpty(returnUrl) && !Url.IsLocalUrl(returnUrl))                       
                return RedirectToAction("List", "Home");  
                    
            var authenticateResult = await HttpContext.AuthenticateAsync("External");

            if (!authenticateResult.Succeeded)
                return BadRequest();

            var claimsIdentity = new ClaimsIdentity("Application");

            claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.Name));
            claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.Email));

            await HttpContext.SignInAsync(
                "Application",
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true }); // IsPersistent will set a cookie that lasts for two weeks (by default).

            return LocalRedirect(returnUrl);
        }
    }
}