using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BE.Utilitarian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BL.Interfaces
{
    public interface IUserServices
    {
        Task<ResponseHttp<TokenJWT>> GetUser(AuthenticationRequest request);
        Task<ResponseHttp<bool>> UpdateUser(UserRequest request);
        Task<ResponseHttp<bool>> ChangesPassword(AuthenticationRequest request);
        Task<ResponseHttp<bool>> RestedUser(RecoveredRequest request);
        Task<ResponseHttp<bool>> GeneratedToken(EmailRequest request);
        Task<ResponseHttp<bool>> ValidToken(RecoveredRequest request);

    }
}
