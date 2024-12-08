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
                if (ValidLoginAttempts(user))
                {
                    throw new Exception("La cuenta ha sido bloqueada, vuelva a intentarlo más tarde");
                }

                if (!user.Password.Equals(request.Password.Encrypted()))
                {
                    await UpdateUserFailed(user);
                    throw new Exception("Login Incorrecto");
                }

                var userDto=await BuilUserDto(user);                
                await UpdateUserCorrect(user);
                var token = _keyToken.CreatedToken(userDto);
                token.User = user.NameUser;
                token.Rol = user.RolId;
                response.Data=token;
                response.Code = CodeResponse.Ok;
                response.Message=MessageResponse.Ok;

            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        private async Task<UserDto> BuilUserDto(User user)
        {
            var userDto=new UserDto();
            Expression<Func<User, bool>> expression = userDb => userDb.UserId == user.UserId;
            try
            {
                userDto = await _repository.GetUser(expression).Include(userDb => userDb.Rol).Select(userDb => new UserDto
                {
                    Email = userDb.Email,
                    Rol = userDb.Rol.DescriptionRol,
                    UserId=userDb.UserId
                }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return userDto??new UserDto();
        }
        private async Task UpdateUserFailed(User user)
        {
                try
                {
                    user.Attempts += 1;
                    if (user.Attempts > 5)
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
                if (user.Blocked && user.DateLastAttempts>DateTime.Now)
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

        public async Task<ResponseHttp<bool>> RestedUser(RecoveredRequest request)
        {
            ResponseHttp<bool> response = new();
            Expression<Func<User, bool>> expression = user => user.RecoveredToken!=null&& user.RecoveredToken.Equals(request.Token);
            try
            {
                var user = await _repository.GetUser(expression).FirstOrDefaultAsync();
                if (user == null) 
                {
                    throw new Exception(_invalidToken);
                }
                user.Password = request.Password.Encrypted();
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
            Expression<Func<User, bool>> expression = user => user.Email !=  null && user.Email.Equals(request.To);


            try
            {
                var userDto = await _repository.GetUser(expression)
                    .Include(userDb=>userDb.Rol)
                    .Select(userDb=>new UserDto
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

                var responseEmail = await SendEmailToken(request);
                if (responseEmail.Data == false)
                {
                    throw new Exception("Comuniquese con el departamente de desarrollo para su ayuda");
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
        public async Task<ResponseHttp<bool>> ValidToken(RecoveredRequest request)
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
