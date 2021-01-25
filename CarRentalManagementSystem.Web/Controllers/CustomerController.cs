using CarRental.BusinessLogic;
using CarRental.BusinessLogic.Concretes;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRentalManagementSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Vehicles = CarRental.Models.Concretes.Vehicles;
using Customers = CarRental.Models.Concretes.Customers;
using CarRental.Commons.Concretes.Encryption;

namespace CarRentalManagementSystem.Web.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
      
        public ActionResult LogInCustomer(LoginModel loginModel)
        {
            string ViewName = "CustomerMainPageView";
            try
            {
                loginModel.UserPassword = Encryption(loginModel.UserPassword);
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
        public ActionResult CustomerSignUp(Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("SignUpAsCustomerView");
            }
            string ViewName = "CustomerMainPageView";

            try
            {
                customer.CustomerPassword = Encryption(customer.CustomerPassword);
                if (InsertCustomer(customer))
                { 
                    FormsAuthentication.SetAuthCookie(customer.CustomerEmail, true);
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
     
        public ViewResult CustomerMainPageView()
        {
            return View();
        }
        [Authorize]
        public ViewResult CustomerVehiclePageView()
        {
            return View();
        }
        [Authorize]
        public ActionResult CustomerRentalPageView()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListAllAvailable(FormCollection collection)
        {
            TempData["requestedDateTime"] = collection["requestedDateTime"];
            TempData["CompanyID"] = collection["CompanyID"]; 
            try
            {
                switch (collection["Submit"])
                {
                    case "Müsait Olan Tüm Araçlara Bakabilirsiniz":
                        return RedirectToAction("ListAllAvaliableVehiclesView", "Customer");
                    case "Şirkete Ait Müsait Araçları Listele":
                        return RedirectToAction("ListAllAvaliableVehiclesOfCompanyView", "Customer");
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Request doesn't exists.");
            }
          
        }
        public ActionResult ListAllAvaliableVehiclesOfCompanyView()
        {
            return View(GetAllAvaliableVehiclesOfCompany(Convert.ToDateTime(TempData["requestedDateTime"]),Convert.ToInt32(TempData["CompanyID"])));
        }
        public ActionResult CancelRequest(int id)
        {
            try
            {
                if (DeleteRequest(id))
                    return RedirectToAction("CustomerRentalPageView");
                return RedirectToAction("CustomerRentalPageView");

            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Request doesn't exists.");
            }
        }
        public ActionResult ListAllAvaliableVehiclesView()
        {

            return View(GetAllAvaliableVehicles(Convert.ToDateTime(TempData["requestedDateTime"])));
            
        }
        #region PRIVATE METHODS
        private string Encryption(string password)
        {
           return Sha1Encryption.SHA1(password);
        }
        private List<Vehicles> GetAllAvaliableVehiclesOfCompany(DateTime date, int companyid)
        {
            try
            {
                using (var companybusiness = new CompanyBusiness())
                {
                    using (var vehiclebusiness = new VehicleBusiness())
                    {
                        List<Vehicles> vehicles = companybusiness.GetByID(companyid).Vehicles.ToList();
                        List<Vehicles> AllAvaliableVehicles = GetAllAvaliableVehicles(date);
                        List<Vehicles> AvaliableVehiclesOfCompany = new List<Vehicles>();
                        foreach (var availablevehicle in AllAvaliableVehicles)
                        {
                            foreach (var vehicleofcompany in vehicles)
                            {
                                if (vehicleofcompany.VehiclesCompanyId == availablevehicle.VehiclesCompanyId)
                                {
                                    AllAvaliableVehicles.Add(vehicleofcompany);
                                }
                            }
                        }
                        return AllAvaliableVehicles;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Request doesn't exists.");
            }
        }
        private List<Vehicles> GetAllAvaliableVehicles(DateTime date)
        {
            try
            {
                using (var vehicleBusiness = new VehicleBusiness())
                {
                   var vehicles = vehicleBusiness.GetAll();
                    using (var rentedbusiness = new RentedVehicleBusiness())
                    {
                        var rentedVehicles = rentedbusiness.GetAll();             
                        foreach (var rentedvehicle in rentedVehicles )
                        {       
                            List<DateTime> isntavaliabletimes = new List<DateTime>();
                            for (DateTime time = rentedvehicle.PickUpDate.Date; time <= rentedvehicle.DropOffDate.Date; time = time.AddDays(1))
                            {
                                isntavaliabletimes.Add(time);
                            }
                            if (isntavaliabletimes.Contains(date))
                            {
                                var removethisvehicle = vehicleBusiness.GetByID(rentedvehicle.RentId);
                                vehicles.Remove(removethisvehicle);
                            }
                        }
                        return vehicles;
                    }
                }
            }
            catch (Exception ex)
            {

                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Request doesn't exists.");
            }
        }
        private bool DeleteRequest(int ID)
        {
            try
            {
                using (var rentalRequestBussiness = new RentalRequestBusiness())
                {
                    return rentalRequestBussiness.DeleteById(ID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Request doesn't exists.");
            }
        }
        private bool InsertCustomer(Customers customers)
        {
            try
            {
                using (var customerBusiness = new CustomerBusiness())
                {
                    return customerBusiness.Insert(customers);
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
                using (var customerBusiness= new CustomerBusiness())
                {
                    return customerBusiness.LogIn(username, password);
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