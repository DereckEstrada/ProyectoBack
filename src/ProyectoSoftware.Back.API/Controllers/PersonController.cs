using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;

namespace ProyectoSoftware.Back.API.Controllers
{
    [Authorize(Roles ="ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonServices _services;
        
        public PersonController(IPersonServices services)
        {
            this._services = services;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<List<PersonDto>>> GetPerson(SearchRequest request)
        {
            ResponseHttp<List<PersonDto>> response = new();
            try
            {
                response = await _services.GetPerson(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<bool>> PostPerson(PersonRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                response = await _services.PostPerson(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<bool>> UpdatePerson(PersonRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                response = await _services.UpdatePerson(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        [HttpPost("[action]")]
        public async Task<ResponseHttp<bool>> DeletePerson(PersonRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                response = await _services.DeletePerson(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
