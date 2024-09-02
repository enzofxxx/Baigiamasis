namespace WebApplication1.Dtos.Human
{
    public class HumanDto
    {
        public int HumanInformationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long PersonalId { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public byte[]? FileData { get; set; }
    }
}
