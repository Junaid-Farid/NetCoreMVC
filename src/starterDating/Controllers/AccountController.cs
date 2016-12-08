using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using starterDating.Models.Identity;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace starterDating.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        //Dependency Injection
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            // we don't need to create the object UserManager dependency handle it for Dependency Injection
        }

        //Now handle the register HttpGet request
        [HttpGet]
        public IActionResult Register()
        {
            return View(); //Return the Register view
        }
        //if user fill the form and submit the form from Register View the handle the post request
        [HttpPost]  //it is always the async method
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //Check the model is valid or not
            if (ModelState.IsValid)
            {
                //if model state is valid then
                ApplicationUser user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false); //signin and return to new page
                    return RedirectToAction("Index", "Home");
                }
                //in case of sign in error
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //if model state is not valid then return this page i.e., Register View Page
            return View();
        }

        //Now Handle the login request

        //Get login page request page
        public IActionResult Login()
        {
            return View();
        }
        //after filling the field from login page and presssing the button 
        //handl the post request
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //check the model state otherwise return to the login page
            if (ModelState.IsValid)
            {
                //attempt to signing
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt!");
                }
            }
            return View();
        }

        //check the login 
        //This method can be access by anonymous user
        [Authorize]
        public string Check()
        {
            return "Yes, you are logged in!";
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
