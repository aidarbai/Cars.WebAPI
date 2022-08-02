namespace Cars.DAL.Models
{
    public class PhotoUrl
    {
        public int Id { get; set; }
        public int SourceId { get; set; }
        public PhotoSourceType SourceType { get; set; }
        public string Path { get; set; }
    }

    public enum PhotoSourceType
    {
        Car = 1,
        SparePart
    }
}
