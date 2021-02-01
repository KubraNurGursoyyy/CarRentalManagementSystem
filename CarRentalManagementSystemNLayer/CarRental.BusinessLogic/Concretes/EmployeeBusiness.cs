using CarRental.BusinessLogic.Abstracts;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRental.DataAccess.Concretes;
using CarRental.Models.Concretes;
using System;
using System.Collections.Generic;

namespace CarRental.BusinessLogic.Concretes
{
    public class EmployeeBusiness : IUserBusinessLogic, IDisposable
    {
        public bool LogIn(string Email, string Password)
        {
            bool login = false;
            try
            {
                using (var employee = new EmployeeRepository())
                {
                    foreach (var entity in employee.GetAll())
                    {
                        if (entity.EmployeesEmail == Email && entity.EmployessPassword == Password)
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
                throw new Exception("CarRental.BusinessLogic.Concretes:EmployeeBusiness::LogIn::Error occured.", ex);
            }
        }
        public bool Insert(Employees entity)
        {
            try
            {
                bool isSuccess;
                using (var employeeRepo = new EmployeeRepository())
                {
                    isSuccess = employeeRepo.Insert(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes: " + entity.GetType().ToString() + "::Insert:Error occured.", ex);
            }
        }

        public bool Update(Employees entity)
        {
            try
            {
                bool isSuccess;
                using (var employeeRepo = new EmployeeRepository())
                {
                    isSuccess = employeeRepo.Update(entity);
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
                using (var employeeRepo = new EmployeeRepository())
                {
                    isSuccess = employeeRepo.DeleteByID(id);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.BusinessLogic.Concretes::Delete:Error occured.", ex);
            }
        }

        public Employees GetByID(int id)
        {
            try
            {
                Employees ResponseEntity;
                using (var employeeRepo = new EmployeeRepository())
                {
                    ResponseEntity = employeeRepo.GetByID(id);
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

        public List<Employees> GetAll()
        {
            var ResponseEntity = new List<Employees>();
            try
            {
                using (var employeeRepo = new EmployeeRepository())
                {
                    foreach (var entity in employeeRepo.GetAll())
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
