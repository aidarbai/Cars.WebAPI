using System.ComponentModel.DataAnnotations;

namespace Cars.COMMON.DTOs
{
    public class CarUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        public int Price { get; set; }
        public int Mileage { get; set; }
    }
}
