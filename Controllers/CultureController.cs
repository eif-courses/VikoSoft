namespace VikoSoft.Controllers;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]/[action]")]
public class CultureController : Controller
{
    public IActionResult Set(string? culture, string redirectUri)
    {
        if (culture != null)
        {
            HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture, culture)));
        }

        return LocalRedirect(redirectUri);
    }
    
    // TRACK MALICIOUS REDIRECT ATTACKS
    // private IActionResult RedirectToLocal(string returnUrl)
    // {
    //     if (Url.IsLocalUrl(returnUrl))
    //     {
    //         return Redirect(returnUrl);
    //     }
    //     else
    //     {
    //         return RedirectToAction(nameof(HomeController.Index), "Home");
    //     }
    // }
}