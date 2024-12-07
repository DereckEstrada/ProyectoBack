using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;

namespace ProyectoSoftware.Back.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _services;

        public UserController(IUserServices services)
        {
            this._services = services;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<TokenJWT>> GetUser(AuthenticationRequest request)
        {
            ResponseHttp<TokenJWT> response = new();
            try
            {
                response= await _services.GetUser(request); 
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }      
        [HttpPost("[action]")]
        public async Task<ResponseHttp<bool>> UpdateUser(UserRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                response = await _services.UpdateUser(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<bool>> ValidToken(RecoveredRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {   
                response = await _services.ValidToken(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<bool>> RestedUser(RecoveredRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                response = await _services.RestedUser(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<bool>> GeneratedToken(EmailRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                response = await _services.GeneratedToken(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
