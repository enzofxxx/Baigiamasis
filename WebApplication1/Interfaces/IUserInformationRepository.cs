using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IUserInformationRepository
    {
        Task<List<UserInformation>> GetAllAsync();
       /* Task<UserInformation?> GetByIdAsync(int id);*/
        /*Task<UserInformation> CreateAsync(UserInformation user);
        Task<UserInformation> UpdateAsync(int id, UserInformation user);
        Task<UserInformation> DeleteAsync(int id);*/
    }
}
