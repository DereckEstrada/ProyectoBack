using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Request;
namespace ProyectoSoftware.Back.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(AuthenticationRequest request);        
        Task UpdateUser(User user);
    }
}
