using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRentalManagementSystem.Web.CustomerWebService;
using CarRentalManagementSystem.Web.Models;
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
      
        public ActionResult LogInCustomer(LoginModel loginModel)
        {
            string ViewName = "CustomerView";
            try
            {
                if (LoginCustomer(loginModel.Username, loginModel.UserPassword))
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
                throw new Exception("CustomerController::LogInCustomer::Error occured.", ex);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerSignUp(FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("SignUpAsCustomerView");
            }
            string ViewName = "CustomerView";

            try
            {
                if (InsertCustomer(collection["CustomerName"], collection["CustomerSurname"], collection["CustomerEmail"], collection["CustomerPassword"], 
                    collection["CustomerAddress"], collection["CustomerPhoneNumber"], collection["SecondPhoneNumber"], collection["NationalIdcard"]))
                { 
                    FormsAuthentication.SetAuthCookie(collection["CustomerEmail"], true);
                    return RedirectToAction(ViewName);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CustomerController::CustomerSignUp::Error occured.", ex);
            }
        }
        public ViewResult SignUpAsCustomer()
        {
            return View("SignUpAsCustomerView");
        }
        public ViewResult CustomerView()
        {
            return View();
        }
        #region PRIVATE METHODS
        private bool InsertCustomer(string name, string surname, string username, string password, string adress, string phonenumber, string secondphonenumber, string nationalid)
        {
            try
            {
                using (var customerSoapClient = new CustomerWebServiceSoapClient())
                {
                    return customerSoapClient.SignUpAsCustomer(new CustomerWebService.Customers()
                    {
                        CustomerName = name,
                        CustomerSurname = surname,
                        CustomerEmail = username,
                        CustomerPassword = password,
                        CustomerAddress = adress,
                        CustomerPhoneNumber = phonenumber,
                        SecondPhoneNumber = secondphonenumber,
                        NationalIdcard = nationalid
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CustomerController::InsertCustomer::Error occured.", ex);
            }

        }
        private bool LoginCustomer(string username,string password)
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