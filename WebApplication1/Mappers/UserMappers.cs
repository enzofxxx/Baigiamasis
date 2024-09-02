using WebApplication1.Dtos.User;
using WebApplication1.Models;

namespace WebApplication1.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this UserInformation userDto)
        {
            return new UserDto
            {
                UserInformationId = userDto.UserInformationId,
                UserName = userDto.UserName,
                Role = userDto.Role
            };
        }

        public static UserInformation ToUserInformationFromCreateDto(this CreateUserRequestDto createUser)
        {
            return new UserInformation
            {
                UserName = createUser.UserName,
                Password = createUser.Password,
                Role = createUser.Role,
                Salt = createUser.Salt,
            };
        }
    }
}
