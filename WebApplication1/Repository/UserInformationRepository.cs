using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class UserInformationRepository : IUserInformationRepository
    {
        private readonly AppDBContext _context;
        public UserInformationRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<UserInformation> CreateAsync(UserInformation user)
        {
            await _context.UserInformation.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserInformation> DeleteAsync(int id)
        {
            var user = await _context.UserInformation.FirstOrDefaultAsync(x => x.UserInformationId == id);
            if (user == null) 
            {
                return null;
            }
            _context.UserInformation.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<List<UserInformation>> GetAllAsync()
        {
            return _context.UserInformation.ToListAsync();
        }

       /* public Task<UserInformation?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserInformation> UpdateAsync(int id, UserInformation user)
        {
            throw new NotImplementedException();
        }*/
    }
}
