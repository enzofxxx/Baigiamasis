using System;

namespace Baigiamasis.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string? Salt { get; set; }
        public string Role { get; set; } = "User";
        public Person? Person { get; set; }
    }
}
