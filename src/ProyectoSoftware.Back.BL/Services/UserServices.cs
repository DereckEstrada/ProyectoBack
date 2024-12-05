using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProyectoSoftware.Back.BE.Const;
using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Request;
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
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly string _keyToken;

        public UserServices(IUserRepository repository, IMapper mapper, IConfiguration configuration)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._keyToken= configuration["keyJWT"]!;
        }
        public async Task<ResponseHttp<TokenJWT>> GetUser(AuthenticationRequest request)
        {
            ResponseHttp<TokenJWT> response = new();
            try
            {
                User user = await _repository.GetUser(request);
                if (user == null)
                {
                    throw new Exception("Login Incorrecto");
                }
                if (!user.Password.Equals(request.Password))
                {
                    await UpdateUserFailed(user);
                    throw new Exception("Login Incorrecto");
                }
                if (ValidLoginAttempts(user))
                {
                    throw new Exception("La cuenta ha sido bloqueada, vuelva a intentarlo más tarde");
                }
                var userDto=_mapper.Map<UserDto>(user);                
                await UpdateUserCorrect(user);
                response.Data=_keyToken.CreatedToken(userDto);
                response.Code = CodeResponse.Ok;
                response.Message=MessageResponse.Ok;

            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        private async Task UpdateUserFailed(User user)
        {
                try
                {
                    user.Attempts += 1;
                    if (user.Attempts == 5)
                    {
                        user.Blocked = true;
                        user.DateLastAttempts = DateTime.Now.AddHours(1);
                    }
                    await _repository.UpdateUser(user);
                }
                catch (Exception)
                {
                    throw;
                }
        }
        private bool ValidLoginAttempts(User user)
        {
            bool valid = false; 
            try
            {
                if (!user.Blocked && user.DateLastAttempts<DateTime.Now)
                {
                    valid = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return valid;
        }
        private async Task UpdateUserCorrect(User user)
        {
            try
            {
                user.Active = true;
                user.Blocked = false;
                user.DateLastAttempts = null;
                user.Attempts = 0;
                await _repository.UpdateUser(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ResponseHttp<bool>> UpdateUser(UserRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                var user=_mapper.Map<User>(request);
                await _repository.UpdateUser(user);
                response.Data=true;
                response.Code = CodeResponse.Accepted;
                response.Message=MessageResponse.Accepted;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
