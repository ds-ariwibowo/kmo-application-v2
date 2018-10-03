using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;

using Kmo.Modules._650_SYS.UserManagement;

namespace Kmo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (AuthenticationManager.User.Identity.IsAuthenticated)
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Web.Models.LoginViewModel model, string returnUrl)
        {

            //ModelState.AddModelError("testerror", "This is Test Error from Server");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe == 1 ? true : false, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", string.Format("{0} is Locked. Please Wait in a Few Minutes then Try to Login Again", model.Email));
                    return View(model);
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }

        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Session["Menu"] = null;
            HttpContext.Session["MainController"] = null;
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ChangePassword(Web.Models.ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = Services.GetUserDb(false, User.Identity.GetUserId());
            if (model.OldPassword.HashSha512().HexaString().AsHexaStringToBin() == data.PasswordHash)
            {
                data.PasswordHash = model.NewPassword.HashSha512().HexaString().AsHexaStringToBin();
                Services.UpdateUserDb(false, data);
            }
            else
            {
                ModelState.AddModelError("", "Incorrect current password.");
                return View(model);
            }
            return RedirectToAction("Index", "Home", new { Message = ManageMessageId.ChangePasswordSuccess });
        }
            
        public enum ManageMessageId
        {          
            ChangePasswordSuccess,         
            Error
        }


        [ChildActionOnly]
        public PartialViewResult UserMenu()
        {
            List<Kmo.Web.Models.MenuViewModels> models = new List<Kmo.Web.Models.MenuViewModels>();

            //Commented by Bowo
            //models.Add(new Kmo.Web.Models.MenuViewModels { Name = "CFS", Route = "Modules/800-BDM/ClientFactSheet/Index", MenuPath = "CBO:icon-folder/800-BDM:icon-folder/$Modules:icon-docs" });
            //models.Add(new Kmo.Web.Models.MenuViewModels { Name = "SAS", Route = "Modules/810-BDC/SuiteAvailabilityStatus/Index", MenuPath = "CBO:icon-folder/810-BDC:icon-folder/$Modules:icon-docs" });
            //models.Add(new Kmo.Web.Models.MenuViewModels { Name = "LOO", Route = "Modules/900-CEO/LetterOfOffer/Index", MenuPath = "CEO:icon-folder/900-CEO:icon-folder/$Modules:icon-docs" });
            //models.Add(new Kmo.Web.Models.MenuViewModels { Name = "CTR", Route = "Modules/900-CEO/LeaseAgreement/Index", MenuPath = "CEO:icon-folder/900-CEO:icon-folder/$Modules:icon-docs" });
            //models.Add(new Kmo.Web.Models.MenuViewModels { Name = "INV:SEC", Route = "Modules/900-CEO/LeaseAgreement/IndexPreInv", MenuPath = "CFO:icon-folder/710-BIL:icon-folder/$Modules:icon-docs" });
            //models.Add(new Kmo.Web.Models.MenuViewModels { Name = "APA:ECD", Route = "Modules/300-PMM/UtlEcd/IndexApa", MenuPath = "CFO:icon-folder/740-APA:icon-folder/$Modules:icon-docs" });
            //models.Add(new Kmo.Web.Models.MenuViewModels { Name = "UTL:ECD", Route = "Modules/300-PMM/UtlEcd/IndexUtl", MenuPath = "PMM:icon-folder/300-PMM:icon-folder/$Modules:icon-docs" });

            //Added by Bowo
            if (HttpContext.Session["Menu"] == null)
            {
                HttpContext.Session["Menu"] = Services.GetUserWebModules(User.Identity.GetUserId());
            }

            var data = HttpContext.Session["Menu"] as List<Modules._650_SYS.UserManagement.Models.vi_UsersWebModule>;
            foreach (var item in data)
            {
                models.Add(new Kmo.Web.Models.MenuViewModels { Name = item.ModuleName, Route = item.ModuleRoute, MenuPath = item.MenuPath });
            }                 
            //End added

            List<Web.Models.MenuNodes> menuRoot = new List<Web.Models.MenuNodes>();

            foreach (var item in models)
            {
                ProcessModule(menuRoot, item);
            }

            //menuRoot = menuRoot.OrderBy(r => r.Name).ToList();
            menuRoot = menuRoot.ToList();

            foreach (var menu in menuRoot)
            {
                SortItem(menu);
            }
            return PartialView("~/Views/Shared/SideBarMenu.cshtml", menuRoot);
        }

        private void SortItem(Web.Models.MenuNodes node)
        {

            //var sorted = node.ChildNodes.OrderBy(r => r.Name).OrderByDescending(r => r.ChildNodes.Count > 0).ToList();
            var sorted = node.ChildNodes.OrderByDescending(r => r.ChildNodes.Count > 0).ToList();

            if (sorted.Count > 0)
            {
                foreach (var item in sorted)
                {
                    SortItem(item);
                }
                node.ChildNodes.Clear();
                node.ChildNodes.AddRange(sorted.ToArray());
            }
        }

        private void ProcessModule(List<Web.Models.MenuNodes> roots, Kmo.Web.Models.MenuViewModels model)
        {
            if (!string.IsNullOrEmpty(model.MenuPath))
            {
                Web.Models.MenuNodes parent = null;
                foreach (var item in model.MenuPath.Split('/').Where(r => !string.IsNullOrEmpty(r)))
                {
                    if (parent == null)
                    {
                        if (item.StartsWith("$Modules"))
                        {
                            Web.Models.MenuNodes rootChild = new Web.Models.MenuNodes();
                            rootChild.Name = model.Name;
                            rootChild.IsFolder = false;
                            rootChild.Icon = item.Split(':')[1];
                            rootChild.Path = model.Route;
                            roots.Add(rootChild);
                            return;
                        }
                        else
                        {
                            parent = roots.SingleOrDefault(r => r.Name == item.Split(':')[0]);
                            if (parent == null)
                            {
                                parent = new Web.Models.MenuNodes();
                                parent.Name = item.Split(':')[0];
                                parent.IsFolder = true;
                                parent.Icon = item.Split(':')[1];
                                parent.Path = model.Route;
                                roots.Add(parent);
                            }
                        }
                    }
                    else
                    {
                        if (item.StartsWith("$Modules"))
                        {
                            parent.ChildNodes.Add(new Web.Models.MenuNodes
                            {
                                Name = model.Name,
                                IsFolder = false,
                                Icon = item.Split(':')[1],
                                Path = model.Route
                            });
                            return;
                        }
                        else
                        {

                            var nextParent = parent.ChildNodes.SingleOrDefault(r => r.Name == item.Split(':')[0]);
                            if (nextParent == null)
                            {
                                nextParent = new Web.Models.MenuNodes();
                                nextParent.Name = item.Split(':')[0];
                                nextParent.IsFolder = true;
                                nextParent.Icon = item.Split(':')[1];
                                nextParent.ParentNode = parent;
                                parent.ChildNodes.Add(nextParent);
                            }
                            parent = nextParent;

                        }
                    }                    
                }
            }
        }




    }
}