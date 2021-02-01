using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.Concretes
{
    public partial class RentedVehicles : IDisposable
    {
        public int RentId { get; set; }

        [Required(ErrorMessage = "You must enter renter vehicle ID.")]
        public int RentedVehicleId { get; set; }

        [Required(ErrorMessage = "You must enter supplier company ID.")]
        public int SupplierCompanyId { get; set; }

        [Required(ErrorMessage = "You must enter drive's ID.")]
        public int DriverCustomerId { get; set; }

        [Required(ErrorMessage = "You must enter pick up date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PickUpDate { get; set; }

        [Required(ErrorMessage = "You must enter drop off date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DropOffDate { get; set; }

        [Required(ErrorMessage = "You must enter vehicle's pick up Km.")]
        public int VehiclesPickUpKm { get; set; }

        [Required(ErrorMessage = "You must enter vehicle's drop off Km.")]
        public int VehiclesDropOffKm { get; set; }

        [Required(ErrorMessage = "You must enter rental price.")]
        public decimal RentalPrice { get; set; }

        public Customers DriverCustomer { get; set; }
        public Vehicles RentedVehicle { get; set; }
        public Companies SupplierCompany { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
