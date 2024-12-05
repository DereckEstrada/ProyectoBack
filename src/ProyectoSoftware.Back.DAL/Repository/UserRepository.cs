using Microsoft.EntityFrameworkCore;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.DAL.Interfaces;


namespace ProyectoSoftware.Back.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProyectoSoftwareDbContext _context;

        public UserRepository(ProyectoSoftwareDbContext context)
        {
            this._context = context;
        }
        public async Task<User> GetUser(AuthenticationRequest request)
        {
            var response=new User();
            try
            {
                response=await _context.Users.FirstOrDefaultAsync(user=>user.Email.Equals(request.Email));
            }
            catch (Exception)
            {
                throw;
            }
            return response??new User();
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
