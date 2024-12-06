using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;

namespace ProyectoSoftware.Back.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailServices _services;

        public EmailController(IEmailServices services)
        {
            this._services = services;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<bool>> SendEmail(EmailRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                response = await _services.SendEmail(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
