using CarRental.BusinessLogic.Abstracts;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRental.DataAccess.Concretes;
using CarRental.Models.Concretes;
using System;
using System.Collections.Generic;

namespace CarRental.BusinessLogic.Concretes
{
    public class CompanyBusiness : IUserBusinessLogic, IDisposable
    {
        public bool Insert(Companies entity)
        {
            try
            {
                bool isSuccess;
                using (var companyRepo = new CompanyRepository())
                {
                    isSuccess = companyRepo.Insert(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes: " + entity.GetType().ToString() + "::Insert:Error occured.", ex);
            }
        }

        public bool Update(Companies entity)
        {
            try
            {
                bool isSuccess;
                using (var companyRepo = new CompanyRepository())
                {
                    isSuccess = companyRepo.Update(entity);
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
                using (var companyRepo = new CompanyRepository())
                {
                    isSuccess = companyRepo.DeleteByID(id);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes::Delete:Error occured.", ex);
            }
        }

        public Companies GetByID(int id)
        {
            try
            {
                Companies ResponseEntity;
                using (var companyRepo = new CompanyRepository())
                {
                    ResponseEntity = companyRepo.GetByID(id);
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

        public List<Companies> GetAll()
        {
            var ResponseEntity = new List<Companies>();
            try
            {
                using (var companyRepo = new CompanyRepository())
                {
                    foreach (var entity in companyRepo.GetAll())
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
                using (var CompanyRepository = new CompanyRepository())
                {
                    foreach (var entity in CompanyRepository.GetAll())
                    {
                        if (entity.CompanyEmail == Email && entity.CompanyPassword == Password)
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
                throw new Exception("CarRental.BusinessLogic.Concretes:CompanyBusiness:LogIn::Error occured.", ex);
            }
        }
    }
}
