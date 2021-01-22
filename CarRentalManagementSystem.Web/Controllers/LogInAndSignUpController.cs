using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using System;
using System.Web.Mvc;

namespace CarRentalManagementSystem.Web.Controllers
{
    public class LogInAndSignUpController : Controller
    {
        // GET: LogInAndSignUp

        [HttpGet]
        public ViewResult SignUpAs()
        {
            return View();
        }
        [HttpGet]
        public ViewResult LogInView()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogInView(FormCollection collection)
        {
            try
            {
                TempData["username"] = collection["username"];
                TempData["password"] = collection["userpassword"];
                switch (collection["Login"])
                {
                    case "Kaydol":
                        return RedirectToAction("SignUpAs");
                    case "Kullanıcı Girişi":
                        return RedirectToAction("LogInCustomer", "Customer");
                    case "Çalışan Girişi":
                        ViewBag.Message = "The operation was cancelled!";
                        return View();
                    case "Şirket Girişi":
                        ViewBag.Message = "The operation was cancelled!";
                        return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("LogInAndSignUpController::LogInView::Error occured.", ex);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult SignUpAs(FormCollection collection)
        {
            try
            {
                switch (collection["SignUpAs"])
                {
                    case "Kullanıcı":
                        return RedirectToAction("SignUpAsCustomer", "Customer");
                    case "Şirket":
                        return RedirectToAction("SignUpAsCompany", "Company");
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("LogInAndSignUpController::SignUpAs::Error occured.", ex);
            }   
        }
    }
}