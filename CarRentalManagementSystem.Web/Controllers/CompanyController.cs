using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRentalManagementSystem.Web.CompanyWebService;
using CarRentalManagementSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CarRentalManagementSystem.Web.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult LogInCompany(LoginModel loginModel)
        {
            string ViewName = "CompanyView";
            try
            {
                if (LoginCompany(loginModel.Username, loginModel.UserPassword))
                {
                    FormsAuthentication.SetAuthCookie(loginModel.Username, true);
                    return RedirectToAction(ViewName);
                }
                else
                {
                    return RedirectToAction("LogInView", "LogInAndSignUp");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CompanyController::LogInCompany::Error occured.", ex);
            }
        }
        public ViewResult SignUpAsCompany()
        {
            return View();
        }
        public ViewResult CompanyView()
        {
            return View();
        }
        #region PRIVATE METHODS
        private bool LoginCompany(string username, string password)
        {
            try
            {
                using (var companySoapClient = new CompanyWebServiceSoapClient())
                {
                    return companySoapClient.LogInAsCompany(username, password);
                }
            }
            catch (Exception ex)
            {

                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CompanyController::LogInCompany::Error occured.", ex);
            }
        }
        #endregion
    }
}