using System.Net;

namespace Baigiamasis.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public Address? Address { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
