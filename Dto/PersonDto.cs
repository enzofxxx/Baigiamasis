namespace Baigiamasis.Dto
{
    public class PersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureBase64 { get; set; } // Profile picture as base64 string
        public AddressDto Address { get; set; }
    }
}
