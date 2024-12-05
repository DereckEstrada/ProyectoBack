using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;

namespace ProyectoSoftware.Back.API.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolServices _services;

        public RolController(IRolServices services)
        {
            this._services = services;
        }
        [HttpGet("[action]")]
        public async  Task<ResponseHttp<List<Rol>>> GetRol()
        {
            ResponseHttp<List<Rol>> response = new();
            try
            {
                response= await _services.GetRol();
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

    }
}
