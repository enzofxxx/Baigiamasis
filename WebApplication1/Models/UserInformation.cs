namespace WebApplication1.Models
{
    public class UserInformation
    {
        public int UserInformationId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public string Role { get; set; } = "User";
        public HumanInformation? HumanInformation { get; set; }
    }
}
 