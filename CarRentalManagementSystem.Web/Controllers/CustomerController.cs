using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRentalManagementSystem.Web.CustomerWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CarRentalManagementSystem.Web.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer

        public ActionResult LogInCustomer()
        {
            string ViewName = "CustomerView";
            try
            {
                string username = TempData["username"].ToString();
                string userpassword = TempData["password"].ToString();
                if (LoginCustomer(username, userpassword))
                {
                    FormsAuthentication.SetAuthCookie(username, true);
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
                throw new Exception("CustomerController::LogInCustomer::Error occured.", ex);
            }
        }
        public ViewResult SignUpAsCustomerView()
        {
            return View();
        }
        public ViewResult CustomerView()
        {
            return View();
        }
        #region PRIVATE METHODS
        private bool LoginCustomer(string username, string password)
        {
            try
            {
                using (var customerSoapClient= new CustomerWebServiceSoapClient())
                {
                    return customerSoapClient.LogInAsCustomer(username, password);
                }
            }
            catch (Exception ex)
            {

                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CustomerController::LogInCustomer::Error occured.", ex); 
            }
        }
        #endregion
    }
}