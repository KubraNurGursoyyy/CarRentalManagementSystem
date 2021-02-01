using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CarRental.Models.Concretes
{
    public partial class Vehicles : IDisposable
    {
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "You must enter a vehicle name.")]
        [StringLength(50, MinimumLength = 3)]
        public string VehicleName { get; set; }

        [Required(ErrorMessage = "You must enter a vehicle model.")]
        [StringLength(50, MinimumLength = 3)]
        public string VehicleModel { get; set; }

        [Required(ErrorMessage = "You must enter vehicle instant Km")]
        public int VehiclesInstantKm { get; set; }

        public bool HasAirbag { get; set; }

        [Required(ErrorMessage = "You must enter trunk volume.")]
        public int TrunkVolume { get; set; }

        [Required(ErrorMessage = "You must enter seating capacity.")]
        public int SeatingCapacity { get; set; }

        [Required(ErrorMessage = "You must enter daily rental price")]
        public decimal DailyRentalPrice { get; set; }

        [Required(ErrorMessage = "You must enter age limit this car")]
        public int AgeLimitForDrivingThisCar { get; set; }

        [Required(ErrorMessage = "You must enter km limit per day.")]
        public int KmLimitPerDay { get; set; }

        [Required(ErrorMessage = "You must enter required driving license age")]
        public int RequiredDrivingLicenseAge { get; set; }

        [Required(ErrorMessage = "You must enter vehicle's company ID.")]
        public int VehiclesCompanyId { get; set; }

        public Companies VehiclesCompany { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
