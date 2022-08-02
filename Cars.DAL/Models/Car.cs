using System.Collections.Generic;

namespace Cars.DAL.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Vin { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public int Mileage { get; set; }
        public string City { get; set; }
        public string PrimaryPhotoUrl { get; set; }
        public string Condition { get; set; }
        public bool IsDeleted { get; set; }
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public virtual Model Model { get; set; }
        public virtual BodyType BodyType { get; set; }
        public virtual List<PhotoUrl> PhotoUrls { get; set; } = new List<PhotoUrl>();
        public virtual ApplicationUser User { get; set; }
    }
}
