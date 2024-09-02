namespace WebApplication1.Dtos.User
{
    public class UpdateUserRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[]? Salt { get; set; }
        public string Role { get; set; } = "User";
    }
}
