using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.Concretes
{
    public class Employees : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public int EmployeesId { get; set; }

        [Required(ErrorMessage = "You must enter employee's company ID.")]
        public int EmployeesCompanyId { get; set; }

        [Required(ErrorMessage = "You must enter an employee's email.")]
        [StringLength(50, MinimumLength = 3)]
        public string EmployeesEmail { get; set; }

        [Required(ErrorMessage = "You must enter an employee's password.")]
        [StringLength(50, MinimumLength = 3)]
        public string EmployessPassword { get; set; }

        [Required(ErrorMessage = "You must enter an employee's password.")]
        [StringLength(10)]
        public string EmployeesPhoneNumber { get; set; }

        [Required(ErrorMessage = "You must enter an employee's address.")]
        [StringLength(250, MinimumLength = 3)]
        public string EmployeesAddress { get; set; }

        public virtual Companies EmployeesCompany { get; set; }
    }
}
