using ProyectoSoftware.Back.BE.Const;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;
using ProyectoSoftware.Back.DAL.Interfaces;


namespace ProyectoSoftware.Back.BL.Services
{
    public class SexServices : ISexServices
    {
        private readonly ISexRepository _repository;

        public SexServices(ISexRepository repository)
        {
            this._repository = repository;
        }
        public async Task<ResponseHttp<List<Sex>>> GetSex()
        {
            ResponseHttp<List<Sex>> response = new();
            try
            {
                response.Data = await _repository.GetSex();                
                bool containsList = response.Data != null && response.Data.Count > 0;
                response.Code = containsList ? CodeResponse.Ok : CodeResponse.NoContent;
                response.Message = containsList ? MessageResponse.Ok : MessageResponse.NoContent;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
