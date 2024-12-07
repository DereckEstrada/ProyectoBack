using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using ProyectoSoftware.Back.BE.Const;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;
using ProyectoSoftware.Back.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BL.Services
{
    public class RolServices : IRolServices
    {
        private readonly IRolRepository _repository;

        public RolServices(IRolRepository repository)
        {
            this._repository = repository;
        }
        public async Task<ResponseHttp<List<Rol>>> GetRol()
        {
            ResponseHttp<List<Rol>> response = new();
            try
            {
                response.Data= await _repository.GetRol();
                bool containsList = response.Data != null && response.Data.Count > 0;
                response.Code = containsList ? CodeResponse.Ok : CodeResponse.NoContent;
                response.Message=containsList? MessageResponse.Ok : MessageResponse.NoContent;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
