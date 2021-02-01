
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRental.DataAccess.Concretes;
using CarRental.Models.Concretes;
using System;
using System.Collections.Generic;

namespace CarRental.BusinessLogic.Concretes
{
    public class RentalRequestBusiness : IDisposable
    {
        public bool Insert(RentalRequests entity)
        {
            try
            {
                bool isSuccess;
                using (var rentalRequestRepository = new RentalRequestRepository())
                {
                    isSuccess = rentalRequestRepository.Insert(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes: " + entity.GetType().ToString() + "::Insert:Error occured.", ex);
            }
        }

        public bool Update(RentalRequests entity)
        {
            try
            {
                bool isSuccess;
                using (var rentalRequestRepository = new RentalRequestRepository())
                {
                    isSuccess = rentalRequestRepository.Update(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes: " + entity.GetType().ToString() + "::Update:Error occured.", ex);
            }
        }

        public bool DeleteById(int id)
        {
            try
            {
                bool isSuccess;
                using (var rentalRequestRepository = new RentalRequestRepository())
                {
                    isSuccess = rentalRequestRepository.DeleteByID(id);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes::Delete:Error occured.", ex);
            }
        }

        public RentalRequests GetByID(int id)
        {
            try
            {
                RentalRequests ResponseEntity;
                using (var rentalRequestRepository = new RentalRequestRepository())
                {
                    ResponseEntity = rentalRequestRepository.GetByID(id);
                    if (ResponseEntity == null)
                    {
                        throw new NullReferenceException("Entity doesnt exists!");
                    }
                    return ResponseEntity;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes:GenericBusinessLogic::GetByID::Error occured.", ex);
            }
        }

        public List<RentalRequests> GetAll()
        {
            var ResponseEntity = new List<RentalRequests>();
            try
            {
                using (var rentalRequestRepository = new RentalRequestRepository())
                {
                    foreach (var entity in rentalRequestRepository.GetAll())
                    {
                        ResponseEntity.Add(entity);
                    }
                }
                return ResponseEntity;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes:GenericBusinessLogic::GetAll::Error occured.", ex);
            }

        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
        public bool ApproveOrDenyTheRequest(RentalRequests rentalRequests, bool isApprove, decimal rentalprice, int pickupkm, int dropoffkm)
        {
            try
            {
                bool isDeleteSuccess = false, isInsertSuccess = false;
                if (isApprove)
                {
                    using (var rentedvehiclebusiness = new RentedVehicleBusiness())
                    {
                        var rentedvehicle = new RentedVehicles();
                        rentedvehicle.DriverCustomerId = rentalRequests.RentalRequestCustomerId;
                        rentedvehicle.DropOffDate = rentalRequests.RequestedDropOffDate;
                        rentedvehicle.PickUpDate = rentalRequests.RequestedPickUpDate;
                        rentedvehicle.RentalPrice = rentalprice;
                        rentedvehicle.RentedVehicleId = rentalRequests.RequestedVehicleId;
                        rentedvehicle.SupplierCompanyId = rentalRequests.RequestedSupplierCompanyId;
                        rentedvehicle.VehiclesPickUpKm = pickupkm;
                        rentedvehicle.VehiclesDropOffKm = dropoffkm;
                        isInsertSuccess = rentedvehiclebusiness.Insert(rentedvehicle);
                        if (isInsertSuccess)
                        {
                            isDeleteSuccess = DeleteById(rentalRequests.RentalRequestId);

                        }
                    }
                }
                else
                {
                    isDeleteSuccess = DeleteById(rentalRequests.RentalRequestId);
                }
                return isDeleteSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes:RentalRequestBusiness::ApproveOrDenyTheRequest:Error occured.", ex);
            }
        }
    }
}
