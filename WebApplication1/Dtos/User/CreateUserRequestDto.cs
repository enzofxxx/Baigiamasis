namespace WebApplication1.Dtos.User
{
    public class CreateUserRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[]? Salt { get; set; }
        public string Role { get; set; } = "User";
    }
}
