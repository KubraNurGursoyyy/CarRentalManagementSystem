using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using RentedVehicles = CarRental.Models.Concretes.RentedVehicles;
using Vehicles = CarRental.Models.Concretes.Vehicles;
using RentalRequests = CarRental.Models.Concretes.RentalRequests;
using Employee = CarRental.Models.Concretes.Employees;
using CarRental.BusinessLogic.Concretes;
using CarRental.BusinessLogic;
using CarRentalManagementSystem.Web.Models;
using System.Web.Security;
using CarRental.Commons.Concretes.Encryption;

namespace CarRentalManagementSystem.Web.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        [HttpGet]
        public ViewResult EmployeesMainPageView()
        {
            return View();
        }
        public ActionResult EmployeeLogin(LoginModel loginModel)
        {
            string ViewName = "EmployeesMainPageView";
            try
            {
                loginModel.UserPassword = Encryption(loginModel.UserPassword);
                if (LoginEmployee(loginModel.Username, loginModel.UserPassword))
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
        public ActionResult ListAllRequestOfCompany(int companyid)
        {
            try
            {
                IList<RentalRequests> rentalrequests = ListAllRentalRequestOfEmployeesCompany(companyid).ToList();
                return View(rentalrequests);
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("There isn't any request to show.");
            }
        }

        public ActionResult AprroveRequest(int id)
        {
            try
            {
                if (ApproveAndAdd(id))
                    return RedirectToAction("ListAllRequestOfCompany");
                return RedirectToAction("ListAllRequestOfCompany");
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Operation failed!", ex);
            }
        }

        public ActionResult DenyRequest(int id)
        {
            try
            {
                if (DeleteRequest(id))
                    return RedirectToAction("ListAllRequestOfCompany");
                return RedirectToAction("ListAllRequestOfCompany");
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
        private bool LoginEmployee(string username, string password)
        {
            try
            {
                using (var EmployeeBusiness = new EmployeeBusiness())
                {
                    return EmployeeBusiness.LogIn(username, password);
                }
            }
            catch (Exception ex)
            {

                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("EmployeeController::LogInEmployee::Error occured.", ex);
            }
        }
        private bool DeleteRequest(int ID)
        {
            try
            {
                using (var rentalRequestBusiness = new RentalRequestBusiness())
                {
                    return rentalRequestBusiness.DeleteById(ID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Request doesn't exists.");
            }
        }

        private bool ApproveAndAdd(int ID)
        {
            try
            {
                using (var rentalRequesBusiness = new RentalRequestBusiness())
                {

                    RentalRequests rentalreq = rentalRequesBusiness.GetByID(ID);
                    var rentingtime = Convert.ToInt32(rentalreq.RequestedDropOffDate.Date - rentalreq.RequestedPickUpDate.Date);
                    using (var vehicleBusiness = new VehicleBusiness())
                    {
                       Vehicles reqvehicle = vehicleBusiness.GetByID(rentalreq.RequestedVehicleId);
                        using (var rentedvehicleBusiness = new RentedVehicleBusiness())
                        {
                            RentedVehicles rentvehicle = new RentedVehicles()
                            {
                                RentalPrice = reqvehicle.DailyRentalPrice * rentingtime,
                                DropOffDate = rentalreq.RequestedDropOffDate,
                                PickUpDate = rentalreq.RequestedPickUpDate,
                                VehiclesPickUpKm = reqvehicle.VehiclesInstantKm,
                                VehiclesDropOffKm = reqvehicle.VehiclesInstantKm + (reqvehicle.KmLimitPerDay * rentingtime),
                                SupplierCompanyId = rentalreq.RequestedSupplierCompanyId,
                                RentedVehicleId = rentalreq.RequestedVehicleId,
                                DriverCustomerId = rentalreq.RentalRequestCustomerId

                            };
                            DeleteRequest(ID);
                            return rentedvehicleBusiness.Insert(rentvehicle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Request doesn't exists.");
            }
        }

        private List<RentalRequests> ListAllRentalRequestOfEmployeesCompany(int companyid)
        {
            try
            {
                using (var companyBusiness = new CompanyBusiness())
                {
                    List<RentalRequests> rentalRequests = new List<RentalRequests>();
                    foreach (var responsedRequests in companyBusiness.GetByID(companyid).RentalRequests)
                    {
                        RentalRequests castedRequests = new RentalRequests()
                        {
                            RentalRequestId = responsedRequests.RentalRequestId,
                            RentalRequestCustomerId = responsedRequests.RentalRequestCustomerId,
                            RequestedSupplierCompanyId = responsedRequests.RequestedSupplierCompanyId,
                            RequestedVehicleId = responsedRequests.RequestedVehicleId,
                            RequestedPickUpDate = responsedRequests.RequestedPickUpDate,
                            RequestedDropOffDate = responsedRequests.RequestedDropOffDate

                        };
                        rentalRequests.Add(castedRequests);
                    }
                    return rentalRequests;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("There isn't any request to show.");
            }
        }

        private Employee GetEmployeeByID(int ID)
        {
            try
            {
                using (var employeesBusiness = new EmployeeBusiness())
                {
                    Employee castedEmployee = null;
                    Employee responsedEmployee = employeesBusiness.GetByID(ID);
                    if (responsedEmployee != null)
                    {
                        castedEmployee = new Employee()
                        {
                            EmployeesId = responsedEmployee.EmployeesId,
                            EmployeesCompanyId = responsedEmployee.EmployeesCompanyId,
                            EmployeesAddress = responsedEmployee.EmployeesAddress,
                            EmployeesEmail = responsedEmployee.EmployeesEmail,
                            EmployeesPhoneNumber = responsedEmployee.EmployeesPhoneNumber,
                            EmployessPassword = responsedEmployee.EmployessPassword
                        };
                    }
                    return castedEmployee;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("Employee doesn't exists.");
            }
        }
        #endregion
    }
   
}