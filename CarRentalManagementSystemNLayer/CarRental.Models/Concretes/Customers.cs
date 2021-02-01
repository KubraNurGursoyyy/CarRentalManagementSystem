using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.Concretes
{
    public class Customers : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Customers()
        {
            RentalRequests = new List<RentalRequests>();
            RentedVehicles = new List<RentedVehicles>();
        }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "You must enter an customer name.")]
        [StringLength(50, MinimumLength = 3)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "You must enter an customer surname.")]
        [StringLength(50, MinimumLength = 3)]
        public string CustomerSurname { get; set; }

        [Required(ErrorMessage = "You must enter an customer email.")]
        [StringLength(50, MinimumLength = 3)]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "You must enter an customer password.")]
        [StringLength(50, MinimumLength = 3)]
        public string CustomerPassword { get; set; }

        [Required(ErrorMessage = "You must enter an customer phone number.")]
        [StringLength(10)]
        public string CustomerPhoneNumber { get; set; }

        [Required(ErrorMessage = "You must enter an customer address.")]
        [StringLength(250, MinimumLength = 3)]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "You must enter an second phone number for customer.")]
        [StringLength(10)]
        public string SecondPhoneNumber { get; set; }

        [Required(ErrorMessage = "You must enter an customer national ID card.")]
        [StringLength(50, MinimumLength = 3)]
        public string NationalIdcard { get; set; }

        public virtual List<RentalRequests> RentalRequests { get; set; }
        public virtual List<RentedVehicles> RentedVehicles { get; set; }
    }
}