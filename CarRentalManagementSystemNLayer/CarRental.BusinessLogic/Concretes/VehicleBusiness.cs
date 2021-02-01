using CarRental.BusinessLogic.Abstracts;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRental.DataAccess.Concretes;
using CarRental.Models.Concretes;
using System;
using System.Collections.Generic;

namespace CarRental.BusinessLogic.Concretes
{
    public class VehicleBusiness : IDisposable
    {
        public bool Insert(Vehicles entity)
        {
            try
            {
                bool isSuccess;
                using (var vehicleRepo = new VehicleRepository())
                {
                    isSuccess = vehicleRepo.Insert(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes: " + entity.GetType().ToString() + "::Insert:Error occured.", ex);
            }
        }

        public bool Update(Vehicles entity)
        {
            try
            {
                bool isSuccess;
                using (var vehicleRepo = new VehicleRepository())
                {
                    isSuccess = vehicleRepo.Update(entity);
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
                using (var vehicleRepo = new VehicleRepository())
                {
                    isSuccess = vehicleRepo.DeleteByID(id);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes::Delete:Error occured.", ex);
            }
        }

        public Vehicles GetByID(int id)
        {
            try
            {
                Vehicles ResponseEntity;
                using (var vehicleRepo = new VehicleRepository())
                {
                    ResponseEntity = vehicleRepo.GetByID(id);
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

        public List<Vehicles> GetAll()
        {
            var ResponseEntity = new List<Vehicles>();
            try
            {
                using (var vehicleRepo = new VehicleRepository())
                {
                    foreach (var entity in vehicleRepo.GetAll())
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
