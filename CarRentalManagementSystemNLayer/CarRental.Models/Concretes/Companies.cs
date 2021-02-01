using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.Concretes
{
    public class Companies : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public Companies()
        {
            Employees = new List<Employees>();
            RentalRequests = new List<RentalRequests>();
            RentedVehicles = new List<RentedVehicles>();
            Vehicles = new List<Vehicles>();
        }

        public int CompanyId { get; set; }

        [Required(ErrorMessage = "You must enter a company name.")]
        [StringLength(50, MinimumLength = 3)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "You must enter an company email.")]
        [StringLength(50, MinimumLength = 3)]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessage = "You must enter an company password.")]
        [StringLength(50, MinimumLength = 3)]
        public string CompanyPassword { get; set; }

        [Required(ErrorMessage = "You must enter an company city.")]
        [StringLength(50, MinimumLength = 3)]
        public string CompanyCity { get; set; }

        [Required(ErrorMessage = "You must enter an company address.")]
        [StringLength(500, MinimumLength = 3)]
        public string CompanyAdress { get; set; }

        public int? CompanyPoints { get; set; }

        public virtual List<Vehicles> Vehicles { get; set; }
        public virtual List<Employees> Employees { get; set; }
        public virtual List<RentalRequests> RentalRequests { get; set; }
        public virtual List<RentedVehicles> RentedVehicles { get; set; }
    }
}