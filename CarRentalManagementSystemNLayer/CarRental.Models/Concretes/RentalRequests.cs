using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.Concretes
{
    public partial class RentalRequests : IDisposable
    {
        public int RentalRequestId { get; set; }

        [Required(ErrorMessage = "You must enter requeste customer ID.")]
        public int RentalRequestCustomerId { get; set; }

        [Required(ErrorMessage = "You must enter supplier company ID.")]
        public int RequestedSupplierCompanyId { get; set; }

        [Required(ErrorMessage = "You must enter requested vehicle ID.")]
        public int RequestedVehicleId { get; set; }

        [Required(ErrorMessage = "You must enter pick up date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RequestedPickUpDate { get; set; }

        [Required(ErrorMessage = "You must enter drop off date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RequestedDropOffDate { get; set; }

        public Customers RentalRequestCustomer { get; set; }
        public Companies RequestedSupplierCompany { get; set; }
        public Vehicles RequestedVehicle { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
