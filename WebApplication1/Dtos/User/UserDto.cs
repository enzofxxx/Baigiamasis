using WebApplication1.Models;

namespace WebApplication1.Dtos.User
{
    public class UserDto
    {
        public int UserInformationId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; } = "User";
    }
}
