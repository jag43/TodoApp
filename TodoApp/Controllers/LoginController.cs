using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        public IActionResult SignInSuccess()
        {
            return RedirectToAction("Index");
        }

        [Route("sign-out")]
        public async Task<IActionResult> SignOut(string signOutType)
        {
            if (signOutType == "app")
            {
                await HttpContext.SignOutAsync();
            }
            if (signOutType == "all")
            {
                return Redirect("https://login.microsoftonline.com/common/oauth2/v2.0/logout");
            }
            return RedirectToPage("/_Host");
        }

        [AllowAnonymous, Route("test")]
        public IActionResult Test()
        {
            return Ok("Test");
        }
    }
}
