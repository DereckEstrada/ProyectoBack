using System.Linq.Expressions;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Request;
namespace ProyectoSoftware.Back.DAL.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetUser(Expression<Func<User, bool>> expression);        
        Task UpdateUser(User user);
    }
}
