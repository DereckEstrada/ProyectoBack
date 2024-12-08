using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using ProyectoSoftware.Back.BE.Const;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;
using System.Security.Claims;

namespace ProyectoSoftware.Back.API.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class SexController : ControllerBase
    {
        private readonly ISexServices _services;

        public SexController(ISexServices services)
        {
            this._services = services;
        }
        [HttpGet("[action]")]
        public async Task<ResponseHttp<List<Sex>>> GetSex()
        {
            ResponseHttp<List<Sex>> response = new();
            try
            {
                response = await _services.GetSex();
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
