namespace Cars.COMMON.DTOs
{
    public class CarImportDTO
    {
        public int Id { get; set; }
        public string Vin { get; set; }
        public int Year { get; set; }
        public int PriceUnformatted { get; set; }
        public int MileageUnformatted { get; set; }
        public string City { get; set; }
        public string PrimaryPhotoUrl { get; set; }
        public string Condition { get; set; }
        public string DisplayColor { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string BodyType { get; set; }
        public string[] PhotoUrls { get; set; }
        
    }
}
