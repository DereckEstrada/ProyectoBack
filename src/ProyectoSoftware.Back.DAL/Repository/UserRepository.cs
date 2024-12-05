using System.Linq.Expressions;
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
        public IQueryable<User> GetUser(Expression<Func<User, bool>> expression)
        {
            IQueryable<User> response;
            try
            {
                response= _context.Users.Where(expression);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
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
