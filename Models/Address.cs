namespace Baigiamasis.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? ApartmentNumber { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }
    }
}
