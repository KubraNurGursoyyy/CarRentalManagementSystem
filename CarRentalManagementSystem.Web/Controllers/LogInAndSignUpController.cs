using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRentalManagementSystem.Web.Models;
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
        public ActionResult LogInView(FormCollection collection,LoginModel loginModel)
        {
            try
            {
                if (collection["Login"]== "Kaydol")
                {
                    return RedirectToAction("SignUpAs");
                }
                
                else if (!(loginModel.Username == null) && !(loginModel.UserPassword == null))
                {
                    switch (collection["Login"])
                    { 
                            
                        case "Kullanıcı Girişi":
                            return RedirectToAction("LogInCustomer", "Customer",loginModel);
                        case "Çalışan Girişi":
                            return RedirectToAction("LogInEmployee", "Employee",loginModel);
                        case "Şirket Girişi":
                            return RedirectToAction("LogInCompany", "Company", loginModel);

                    }
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