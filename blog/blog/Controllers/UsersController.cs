using AutoMapper;
using blog.ApiResponse.Interface;
using blog.Code;
using blog.Models;
using DTO;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utility.AnotherFunction;
using Utility.String;

namespace blog.Controllers
{
    public class UsersController : Controller
    {

        IApiResponse _iapiResponse;
        private readonly IMapper _mapper;

        public UsersController(IApiResponse apiResponse, IMapper mapper)
        {
            _iapiResponse = apiResponse;
            _mapper = mapper;
        }
    

        public ActionResult Index()
        {          
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if(string.IsNullOrEmpty(User.Identity.GetUserName()))
            return View();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")]  AccountVM.LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDTO = new UserDTO();
                userDTO = _mapper.Map<AccountVM.LoginViewModel, UserDTO>(user);
                userDTO.Password = HashFunction.CreateHash(userDTO.Password);

                bool login = _iapiResponse.Authentication(userDTO);
                if (login==false)
                {
                    ModelState.AddModelError("", "Error in login ");
                    return View();
                } 
                else
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Error in login ");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (string.IsNullOrEmpty(User.Identity.GetUserName()))
                return View();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountVM.RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDTO = new UserDTO();
                userDTO = _mapper.Map<AccountVM.RegisterViewModel, UserDTO>(user);
                userDTO.Password=HashFunction.CreateHash(userDTO.Password);
                int? loggedIn = ConvertString.ConvertToInt(_iapiResponse.Post<UserDTO>("users", userDTO));
                if (loggedIn == (int)StatusCode.ISEXISTSUSER)
                {
                    ModelState.AddModelError("Email", "Email is alredy exists");
                    return View(user);
                }
                
                else
                    if (loggedIn != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    bool login = _iapiResponse.Authentication(userDTO);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Can't register your account");
            return View(user);
        }

        [Authorize]
        public ActionResult Edit()
        {

            UserDTO userDTO = _iapiResponse.Get<UserDTO>("users?value=" + User.Identity.GetUserName());
            AccountVM.EditUserViewModel userVM = new AccountVM.EditUserViewModel();
            userVM.DisplayName = userDTO.DisplayName;
            userVM.ImageUser = ImageFunction.ConvertImage(userDTO.ImageUser);
            if (userDTO == null)
            {
                return HttpNotFound();
            }
            return View(userVM);
        }
        

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountVM.EditUserViewModel user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDTO = new UserDTO();
                userDTO.Email = User.Identity.GetUserName();
                userDTO.DisplayName = user.DisplayName;               
                if (file == null)
                {
                    if (ConvertString.ConvertToBool(_iapiResponse.Put<UserDTO>("users", userDTO)))
                    return RedirectToAction("Index", "Home");
                }
                else if (file != null && ImageFunction.IsImage(file.InputStream))
                {
                    Image resizeImage = ImageFunction.ScaleImage(Image.FromStream(file.InputStream), 200, 200);
                    userDTO.ImageUser= ImageFunction.imageToByteArray(resizeImage);
                    if(ConvertString.ConvertToBool(_iapiResponse.Put<UserDTO>("users", userDTO)))
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Image is not correct file format");
                    return View(user);
                }
            }
            return View(user);
        }
        
      
    }
}
