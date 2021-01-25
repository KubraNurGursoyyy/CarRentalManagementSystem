using CarRental.BusinessLogic.Concretes;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using Companies= CarRental.Models.Concretes.Companies;
using CarRentalManagementSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Employees = CarRental.Models.Concretes.Employees;
using RentedVehicles = CarRental.Models.Concretes.RentedVehicles;
using Vehicles = CarRental.Models.Concretes.Vehicles;
using CarRental.Commons.Concretes.Encryption;

namespace CarRentalManagementSystem.Web.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        string username;
        int companyid;
        public ActionResult LogInCompany(LoginModel loginModel)
        {
            string ViewName = "CompanyView";
            try
            {
                username = loginModel.Username;
                loginModel.UserPassword=Encryption(loginModel.UserPassword);
                
                if (LoginCompany(username, loginModel.UserPassword))
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUpAsCompany(Companies companies)
        {
            try
            {
                companies.CompanyPassword = Encryption(companies.CompanyPassword);
                if (CompanySignUp(companies))
                {
                    FormsAuthentication.SetAuthCookie(companies.CompanyEmail, true);
                    return RedirectToAction("CompanyView");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                return View();
            }
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
                employee.EmployessPassword = Encryption(employee.EmployessPassword);
                if (SaveEmployee(employee.EmployeesEmail, employee.EmployessPassword , companyid, employee.EmployeesPhoneNumber, employee.EmployeesAddress))
                    return RedirectToAction("CompanysEmployeesView");
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                return View();
            }
        }
        [Authorize]
        public ActionResult CompanysVehiclesView()
        {
            return View(ListCompanysAllVehicles(companyid));
        }
        [Authorize]
        public ActionResult CompanysRentedVehiclesView(int companyid)
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
        private string Encryption(string password)
        {
            return Sha1Encryption.SHA1(password);
        }
        private bool CompanySignUp(Companies companies)
        {
            try
            {
                using (var companyBusiness = new CompanyBusiness())
                {
                    return companyBusiness.Insert(companies);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CompanyController::InsertCoompany::Error occured.", ex);
            }
        }
        private int GetIDByUsername(string username)
        {
            try
            {
                using (var companyBusiness = new CompanyBusiness())
                {
                    int responseID = -1;
                    foreach (var item in companyBusiness.GetAll())
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
                using (var vehicleBusiness = new VehicleBusiness())
                {
                    return vehicleBusiness.Insert(new Vehicles()
                    {
                        VehiclesCompanyId = vehiclescompanyId,
                        VehicleName = vehicleName,
                        VehicleModel = vehicleModel,
                        VehiclesInstantKm = VehicleInstantKm,
                        HasAirbag = hasairbag,
                        TrunkVolume = trunkVolume,
                        SeatingCapacity = seatingcapacity,
                        DailyRentalPrice = dailyRentalPrice,
                        AgeLimitForDrivingThisCar = agelimitfordirivingthiscar,
                        KmLimitPerDay = kmlimitperday,
                        RequiredDrivingLicenseAge = requireddirivinglicenseage,
                    });
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
                using (var companyBusiness = new CompanyBusiness())
                {
                    List<Employees> employees = new List<Employees>();
                    foreach (var responsedEmployees in companyBusiness.GetByID(companyid).Employees)
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
                using (var companyBusiness = new CompanyBusiness())
                {
                    List<RentedVehicles> allrentedvehicles = new List<RentedVehicles>();
                    foreach (var responsedRentedVehicles in companyBusiness.GetByID(companyid).RentedVehicles)
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
                using (var employeeBusiness = new EmployeeBusiness())
                {
                    return employeeBusiness.Insert(new Employees()
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
                using (var companysBusiness = new CompanyBusiness())
                {
                    List<Vehicles> allvehicles = new List<Vehicles>();
                    foreach (var responsedVehicles in companysBusiness.GetByID(companyid).Vehicles)
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
                using (var vehicleBusiness = new VehicleBusiness())
                {
                    return vehicleBusiness.DeleteById(ID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Customer doesn't exists.");
            }
        }

        private bool LoginCompany(string username, string password)
        {
            try
            {
                using (var companyBusiness = new CompanyBusiness())
                {
                    return companyBusiness.LogIn(username, password);
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