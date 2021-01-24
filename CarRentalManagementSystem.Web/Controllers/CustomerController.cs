using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRentalManagementSystem.Web.CustomerWebService;
using CarRentalManagementSystem.Web.Models;
using CarRentalManagementSystem.Web.RentalRequestWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Vehicles = CarRental.Models.Concretes.Vehicles;

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
            string ViewName = "CustomerMainPageView";

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
     
        public ViewResult CustomerMainPageView()
        {
            return View();
        }
 
        public ViewResult CustomerVehiclePageView()
        {
            return View();
        }
        public ActionResult CustomerRentalPageView()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListAllAvailable(FormCollection collection)
        {
            TempData["requestedDateTime"] = collection["requestedDateTime"];
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
            return View(GetAllAvaliableVehiclesOfCompany(Convert.ToDateTime(TempData["requestedDateTime"])));
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
        private List<Vehicles> GetAllAvaliableVehiclesOfCompany(DateTime date)
        {
            try
            {
                List<Vehicles> vehicles = new List<Vehicles>();
                using (var vehicleSoapclient = new VehicleWebService.VehiclesWebServiceSoapClient())
                {
                    foreach (var vehicle in GetAllAvaliableVehicles(date))
                    {
                        using (var companySoapClient= new CompanyWebService.CompanyWebServiceSoapClient()) 
                        { 
                            foreach (var company in companySoapClient.GetAllCompanies())
                            {
                                if (vehicle.VehiclesCompanyId==company.CompanyId)
                                {
                                    vehicles.Add(vehicle);
                                }
                            }
                        }
                    }
                    return vehicles;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private List<Vehicles> GetAllAvaliableVehicles(DateTime date)
        {
            try
            {
                using (var vehiclesoapclient = new VehicleWebService.VehiclesWebServiceSoapClient())
                {
                    List<Vehicles> vehicles = new List<Vehicles>();
                    foreach (var responsevehicle in vehiclesoapclient.GetAllAvaliableVehicles(date).ToList())
                    {
                        Vehicles vehicle = new Vehicles()
                        {
                            VehiclesCompanyId = responsevehicle.VehiclesCompanyId,
                            VehicleName = responsevehicle.VehicleName,
                            VehicleModel = responsevehicle.VehicleModel,
                            VehiclesInstantKm = responsevehicle.VehiclesInstantKm,
                            HasAirbag = responsevehicle.HasAirbag,
                            TrunkVolume = responsevehicle.TrunkVolume,
                            SeatingCapacity = responsevehicle.SeatingCapacity,
                            DailyRentalPrice = responsevehicle.DailyRentalPrice,
                            AgeLimitForDrivingThisCar = responsevehicle.AgeLimitForDrivingThisCar,
                            KmLimitPerDay = responsevehicle.KmLimitPerDay,
                            RequiredDrivingLicenseAge = responsevehicle.RequiredDrivingLicenseAge,
                        };
                        vehicles.Add(vehicle);
                    }
                    return vehicles;
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
                using (var rentalRequestWebSoapClient = new RentalRequestWebServiceSoapClient())
                {
                    return rentalRequestWebSoapClient.CancelRentalRequest(ID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Request doesn't exists.");
            }
        }
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