using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using RentalRequests = CarRental.Models.Concretes.RentalRequests;
using Employee = CarRental.Models.Concretes.Employees;
using CarRentalManagementSystem.Web.EmployeeWebService;
using CarRentalManagementSystem.Web.RentalRequestWebService;
using CarRentalManagementSystem.Web.VehicleWebService;
using CarRentalManagementSystem.Web.RentedVehicleService;

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
        private bool LoginEmployee(string username, string password)
        {
            try
            {
                using (var EmployeeSoapClient = new EmployeesWebServiceSoapClient())
                {
                    return EmployeeSoapClient.LogInAsEmployee(username, password);
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

        private bool ApproveAndAdd(int ID)
        {
            try
            {
                using (var rentalRequestWebSoapClient = new RentalRequestWebServiceSoapClient())
                {
                    
                    RentalRequestWebService.RentalRequests rentalreq = rentalRequestWebSoapClient.GetRentalRequestByID(ID);
                    var rentingtime = Convert.ToInt32(rentalreq.RequestedDropOffDate.Date - rentalreq.RequestedPickUpDate.Date);
                    using (var vehicleWebSoapClient = new VehiclesWebServiceSoapClient())
                    {
                        VehicleWebService.Vehicles reqvehicle = vehicleWebSoapClient.GetVehicleByID(rentalreq.RequestedVehicleId);
                        using (var rentedvehicleWebSoapClient = new RentedVehiclesWebServiceSoapClient())
                        {
                            RentedVehicleService.RentedVehicles rentvehicle = new RentedVehicleService.RentedVehicles()
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
                            return rentedvehicleWebSoapClient.RentVehicle(rentvehicle);
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
                using (var rentalRequestWebSoapClient = new RentalRequestWebServiceSoapClient())
                {
                    List<RentalRequests> rentalRequests = new List<RentalRequests>();
                    foreach (var responsedRequests in rentalRequestWebSoapClient.GetCompanysRentalRequests(companyid).OrderBy(x => x.RentalRequestId).ToList())
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
                using (var employeesSoapClient = new EmployeesWebServiceSoapClient())
                {
                    Employee castedEmployee = null;
                    EmployeeWebService.Employees responsedEmployee = employeesSoapClient.GetEmployeeInformationByID(ID);
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