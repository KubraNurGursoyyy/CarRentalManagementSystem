using CarRental.BusinessLogic.Abstracts;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRental.DataAccess.Concretes;
using CarRental.Models.Concretes;
using System;
using System.Collections.Generic;

namespace CarRental.BusinessLogic
{
    public class RentedVehicleBusiness : IDisposable
    {
        public bool Insert(RentedVehicles entity)
        {
            try
            {
                bool isSuccess;
                using (var rentedVehicleRepo = new RentedVehicleRepository())
                {
                    isSuccess = rentedVehicleRepo.Insert(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes: " + entity.GetType().ToString() + "::Insert:Error occured.", ex);
            }
        }

        public bool Update(RentedVehicles entity)
        {
            try
            {
                bool isSuccess;
                using (var rentedVehicleRepo = new RentedVehicleRepository())
                {
                    isSuccess = rentedVehicleRepo.Update(entity);
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
                using (var rentedVehicleRepo = new RentedVehicleRepository())
                {
                    isSuccess = rentedVehicleRepo.DeleteByID(id);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes::Delete:Error occured.", ex);
            }
        }

        public RentedVehicles GetByID(int id)
        {
            try
            {
                RentedVehicles ResponseEntity;
                using (var rentedVehicleRepo = new RentedVehicleRepository())
                {
                    ResponseEntity = rentedVehicleRepo.GetByID(id);
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

        public List<RentedVehicles> GetAll()
        {
            var ResponseEntity = new List<RentedVehicles>();
            try
            {
                using (var rentedVehicleRepo = new RentedVehicleRepository())
                {
                    foreach (var entity in rentedVehicleRepo.GetAll())
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
    }
}
