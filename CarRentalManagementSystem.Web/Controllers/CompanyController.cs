using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRentalManagementSystem.Web.CompanyWebService;
using CarRentalManagementSystem.Web.EmployeeWebService;
using CarRentalManagementSystem.Web.Models;
using CarRentalManagementSystem.Web.RentedVehicleService;
using CarRentalManagementSystem.Web.VehicleWebService;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Employees = CarRental.Models.Concretes.Employees;
using RentedVehicles = CarRental.Models.Concretes.RentedVehicles;
using Vehicles = CarRental.Models.Concretes.Vehicles;

namespace CarRentalManagementSystem.Web.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        string username;
        int companyid;
        string password;
        public ActionResult LogInCompany(LoginModel loginModel)
        {
            string ViewName = "CompanyView";
            try
            {
                username = loginModel.Username;
                password = loginModel.UserPassword;
                if (LoginCompany(username, password))
                {
                    companyid = GetIDByUsername(username);
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
        public ActionResult CompanysEmployeesView()
        {
            return View(ListAllEmployeesOfCompany(companyid));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEmployeeToCompany(Employees employee)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                if (SaveEmployee(employee.EmployeesEmail, employee.EmployessPassword, companyid, employee.EmployeesPhoneNumber, employee.EmployeesAddress))
                    return RedirectToAction("CompanysEmployeesView");
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                return View();
            }
        }
        [HttpPost]

        public ActionResult CompanysVehiclesView()
        {
            return View(ListCompanysAllVehicles(companyid));
        }

        public ActionResult CompanysRentedVehiclesView()
        {
            return View(ListCompanysRentedVehicles(companyid));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVehicleToCompany(Vehicles vehicles)
        {
            string viewName = "CompanysVehicleView";
            try
            {
                if (AddVehicle(companyid, vehicles.VehicleName, vehicles.VehicleModel, vehicles.VehiclesInstantKm, vehicles.HasAirbag, vehicles.TrunkVolume, vehicles.SeatingCapacity, vehicles.DailyRentalPrice,vehicles.AgeLimitForDrivingThisCar, vehicles.KmLimitPerDay, vehicles.RequiredDrivingLicenseAge))
                {
                    return RedirectToAction(viewName);   
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
            
        }

        [HttpPost]
        public ActionResult DeleteVehicleFromCompany(int vehicleid)
        {
            try
            {
                if (DeleteVehicle(vehicleid))
                    return RedirectToAction("CompanysVehiclesView");
                return RedirectToAction("CompanysVehiclesView");
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Operation failed!", ex);
            }
        }

        #region PRIVATE METHODS
        private int GetIDByUsername(string username)
        {
            try
            {
                using (var companysoapclient = new CompanyWebServiceSoapClient())
                {
                    int responseID = -1;
                    foreach (var item in companysoapclient.GetAllCompanies())
                    {
                        if (item.CompanyEmail == username)
                        {
                            responseID = item.CompanyId;
                            break;
                        }
                    }
                    return responseID;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
            
            
        }
        private bool AddVehicle(int vehiclescompanyId, string vehicleName, string vehicleModel, int VehicleInstantKm, bool hasairbag, int trunkVolume, int seatingcapacity, decimal dailyRentalPrice, int agelimitfordirivingthiscar, int kmlimitperday, int requireddirivinglicenseage)
        {
            try
            {
                using (var vehiclesoapClient = new VehiclesWebServiceSoapClient())
                {
                   return vehiclesoapClient.AddVehicle(new VehicleWebService.Vehicles()
                    {
                        VehiclesCompanyId = vehiclescompanyId,
                        VehicleName = vehicleName,
                        VehicleModel = vehicleModel,
                        VehiclesInstantKm =VehicleInstantKm,
                        HasAirbag =hasairbag,
                        TrunkVolume=trunkVolume,
                        SeatingCapacity = seatingcapacity,
                        DailyRentalPrice = dailyRentalPrice,
                        AgeLimitForDrivingThisCar =agelimitfordirivingthiscar,
                        KmLimitPerDay=kmlimitperday,
                        RequiredDrivingLicenseAge=requireddirivinglicenseage,
                    }) ; 
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
            
        }
        private List<Employees> ListAllEmployeesOfCompany(int companyid)
        {
            try
            {
                using (var employeeSoapClient = new EmployeesWebServiceSoapClient())
                {
                    List<Employees> employees = new List<Employees>();
                    foreach (var responsedEmployees in employeeSoapClient.GetCompanysAllEmployees(companyid))
                    {
                        Employees castedEmployee = new Employees()
                        {
                            EmployeesAddress = responsedEmployees.EmployeesAddress,
                            EmployeesCompanyId = responsedEmployees.EmployeesCompanyId,
                            EmployeesEmail = responsedEmployees.EmployeesEmail,
                            EmployeesPhoneNumber = responsedEmployees.EmployeesPhoneNumber,
                            EmployessPassword = responsedEmployees.EmployessPassword
                        };
                        employees.Add(castedEmployee);
                    }
                    return employees;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private List<RentedVehicles> ListCompanysRentedVehicles(int companyid)
        {
            try
            {
                using (var rentedvehiclesSoapClient = new RentedVehiclesWebServiceSoapClient())
                {
                    List<RentedVehicles> allrentedvehicles = new List<RentedVehicles>();
                    foreach (var responsedRentedVehicles in rentedvehiclesSoapClient.GetCompanysRentedVehicles(companyid))
                    {
                        RentedVehicles castedRentedVehicles = new RentedVehicles()
                        {
                            RentalPrice = responsedRentedVehicles.RentalPrice,
                            DriverCustomerId = responsedRentedVehicles.DriverCustomerId,
                            DropOffDate = responsedRentedVehicles.DropOffDate,
                            PickUpDate = responsedRentedVehicles.PickUpDate,
                            VehiclesPickUpKm = responsedRentedVehicles.VehiclesPickUpKm,
                            VehiclesDropOffKm = responsedRentedVehicles.VehiclesDropOffKm,
                            RentedVehicleId = responsedRentedVehicles.RentedVehicleId,
                            SupplierCompanyId = responsedRentedVehicles.SupplierCompanyId
                        };
                        allrentedvehicles.Add(castedRentedVehicles);
                    }
                    return allrentedvehicles;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private bool SaveEmployee(string email, string password, int companyid, string phonenumber, string address)
        {
            try
            {
                using (var employeeSoapClient = new EmployeesWebServiceSoapClient())
                {
                    return employeeSoapClient.SignUpAsEmployee(new EmployeeWebService.Employees()
                    {
                        EmployeesEmail = email,
                        EmployessPassword = password,
                        EmployeesCompanyId = companyid,
                        EmployeesPhoneNumber = phonenumber,
                        EmployeesAddress = address,
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private List<Vehicles> ListCompanysAllVehicles(int companyid)
        {
            try
            {
                using (var vehiclesSoapClient = new VehiclesWebServiceSoapClient())
                {
                    List<Vehicles> allvehicles = new List<Vehicles>();
                    foreach (var responsedVehicles in vehiclesSoapClient.GetCompanysAllVehicles(companyid))
                    {
                        Vehicles castedVehicles = new Vehicles()
                        {
                            RequiredDrivingLicenseAge = responsedVehicles.RequiredDrivingLicenseAge,
                            DailyRentalPrice = responsedVehicles.DailyRentalPrice,
                            AgeLimitForDrivingThisCar = responsedVehicles.AgeLimitForDrivingThisCar,
                            HasAirbag = responsedVehicles.HasAirbag,
                            KmLimitPerDay = responsedVehicles.KmLimitPerDay,
                            SeatingCapacity = responsedVehicles.SeatingCapacity,
                            TrunkVolume = responsedVehicles.TrunkVolume,
                            VehicleModel = responsedVehicles.VehicleModel,
                            VehiclesInstantKm = responsedVehicles.VehiclesInstantKm,
                            VehiclesCompanyId = responsedVehicles.VehiclesCompanyId,
                            VehicleName = responsedVehicles.VehicleName
                        };
                        allvehicles.Add(castedVehicles);
                    }
                    return allvehicles;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private bool DeleteVehicle(int ID)
        {
            try
            {
                using (var vehicleSoapClient = new VehiclesWebServiceSoapClient())
                {
                    return vehicleSoapClient.DeleteVehicle(ID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }


        #endregion 
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
    }
}