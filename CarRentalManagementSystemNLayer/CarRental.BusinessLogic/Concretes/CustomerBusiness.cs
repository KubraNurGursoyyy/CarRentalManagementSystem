using CarRental.BusinessLogic.Abstracts;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRental.DataAccess.Concretes;
using CarRental.Models.Concretes;
using System;
using System.Collections.Generic;

namespace CarRental.BusinessLogic.Concretes
{
    public class CustomerBusiness : IUserBusinessLogic, IDisposable
    {
        public bool Insert(Customers entity)
        {
            try
            {
                bool isSuccess;
                using (var customerRepo = new CustomerRepository())
                {
                    isSuccess = customerRepo.Insert(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes: " + entity.GetType().ToString() + "::Insert:Error occured.", ex);
            }
        }

        public bool Update(Customers entity)
        {
            try
            {
                bool isSuccess;
                using (var customerRepo = new CustomerRepository())
                {
                    isSuccess = customerRepo.Update(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes: " + entity.GetType().ToString() + "::Update:Error occured.", ex);
            }
        }

        public bool DeleteByID(int id)
        {
            try
            {
                bool isSuccess;
                using (var customerRepo = new CustomerRepository())
                {
                    isSuccess = customerRepo.DeleteByID(id);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes::Delete:Error occured.", ex);
            }
        }

        public Customers GetByID(int id)
        {
            try
            {
                Customers ResponseEntity;
                using (var customerRepo = new CustomerRepository())
                {
                    ResponseEntity = customerRepo.GetByID(id);
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

        public List<Customers> GetAll()
        {
            var ResponseEntity = new List<Customers>();
            try
            {
                using (var customerRepo = new CustomerRepository())
                {
                    foreach (var entity in customerRepo.GetAll())
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
        public bool LogIn(string Email, string Password)
        {
            bool login = false;
            try
            {
                using (var customerrepo = new CustomerRepository())
                {
                    foreach (var entity in customerrepo.GetAll())
                    {
                        if (entity.CustomerEmail == Email && entity.CustomerPassword == Password)
                        {
                            login = true;
                        }
                    }
                }
                return login;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes:CustomerBusiness::LogIn::Error occured.", ex);
            }
        }
    }
}
