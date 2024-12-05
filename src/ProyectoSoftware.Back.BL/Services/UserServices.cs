using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProyectoSoftware.Back.BE.Const;
using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;
using ProyectoSoftware.Back.DAL.Interfaces;


namespace ProyectoSoftware.Back.BL.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailServices _emailServices;
        private readonly string _keyToken;
        private const string _invalidToken = "invalid token";
        public UserServices(IUserRepository repository, IMapper mapper, IConfiguration configuration, IEmailServices emailServices)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._emailServices = emailServices;
            this._keyToken= configuration["keyJWT"]!;
        }
        public async Task<ResponseHttp<TokenJWT>> GetUser(AuthenticationRequest request)
        {
            ResponseHttp<TokenJWT> response = new();
            Expression<Func<User, bool>> expression = user => user.Email.Equals(request.Email);
            try
            {
                var user = await _repository.GetUser(expression).FirstOrDefaultAsync();
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

        public async Task<ResponseHttp<bool>> ChangesPassword(AuthenticationRequest request)
        {
            ResponseHttp<bool> response = new();
            Expression<Func<User, bool>> expression = user => user.Email.Equals(request.Email);
            try
            {

                var user = await _repository.GetUser(expression).FirstOrDefaultAsync();
                if (user != null)
                {
                    throw new Exception("invalid Email");
                }
                user.Password = user.Password.Encrypted();
                await _repository.UpdateUser(user);
                response.Code = CodeResponse.Accepted;
                response.Data=true;
                response.Message = MessageResponse.Accepted;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<ResponseHttp<bool>> RestedUser(AuthenticationRequest request)
        {
            ResponseHttp<bool> response = new();
            Expression<Func<User, bool>> expression = user => user.Email.Equals(request.Email);
            try
            {
                var user = await _repository.GetUser(expression).FirstOrDefaultAsync();
                if (user != null) 
                {
                    throw new Exception(_invalidToken);
                }
                user.Password = user.Password.Encrypted();
                await _repository.UpdateUser(user);
                response.Code = CodeResponse.Accepted;
                response.Data = true;
                response.Message = MessageResponse.Accepted;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async  Task<ResponseHttp<bool>> GeneratedToken(EmailRequest request)
        {
            ResponseHttp<bool> response = new();
            Expression<Func<User, bool>> expression = user => user.RecoveredToken !=  null && user.RecoveredToken.Equals(request.To);


            try
            {
                var userDto = await _repository.GetUser(expression).Include(userDb=>userDb.Rol).Select(userDb=>new UserDto
                {
                    UserId=userDb.UserId,
                    Email=userDb.Email,
                    Rol=userDb.Rol != null? userDb.Rol.DescriptionRol:""
                }).FirstOrDefaultAsync();

                if (userDto == null)
                {
                    throw new Exception(_invalidToken);
                }
                expression = user => user.UserId == userDto.UserId ;
                var user = await _repository.GetUser(expression).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new Exception(_invalidToken);
                }
                var token = _keyToken.CreatedToken(userDto).Token;
                user.RecoveredToken=token;
                await _repository.UpdateUser(user);
                var validParams=request.Params ?? throw new Exception("Sin parametros");
                request.Params["link"] += token;

                response = await SendEmailToken(request);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        private async Task<ResponseHttp<bool>> SendEmailToken(EmailRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                response = await _emailServices.SendEmail(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        public async Task<ResponseHttp<bool>> ValidToken(AuthenticationRequest request)
        {
            ResponseHttp<bool> response = new();
            Expression<Func<User, bool>> expression = user => user.RecoveredToken != null && user.RecoveredToken.Equals(request.Token);
            try
            {
                var user = await _repository.GetUser(expression).FirstOrDefaultAsync();
                if (user == null) 
                {
                    throw new Exception(_invalidToken);
                }
                response.Code = CodeResponse.Accepted;
                response.Data = true;
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
