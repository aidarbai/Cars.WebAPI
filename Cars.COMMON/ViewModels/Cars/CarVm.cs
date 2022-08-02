using System.ComponentModel.DataAnnotations;

namespace Cars.COMMON.ViewModels.Cars
{
    public class CarVm
    {
        public int Id { get; set; }
        [Required]
        public string Vin { get; set; }
        public int Year { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Mileage { get; set; }
        public string City { get; set; }
        public string PrimaryPhotoUrl { get; set; }
        public string Condition { get; set; }
        public string Color { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public string BodyType { get; set; }
        public string[] PhotoUrls { get; set; }
    }
}
