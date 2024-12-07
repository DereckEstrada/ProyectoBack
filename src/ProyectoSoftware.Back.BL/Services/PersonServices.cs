using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Fpe;
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
using System.Linq.Expressions;


namespace ProyectoSoftware.Back.BL.Services
{
    public class PersonServices : IPersonServices
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailServices _emailServices;

        public PersonServices(IPersonRepository repository, IMapper mapper, IEmailServices emailServices)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._emailServices = emailServices;
        }

        public async Task<ResponseHttp<bool>> DeletePerson(PersonRequest request)
        {
            ResponseHttp<bool> response = new();
            try
            {
                Person person= _mapper.Map<Person>(request);
                await _repository.DeletePerson(person);
                response.Code=CodeResponse.Accepted;
                response.Data = true;
                response.Message=MessageResponse.Accepted;    
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<ResponseHttp<List<PersonDto>>> GetPerson(SearchRequest request)
        {
            ResponseHttp<List<PersonDto>> response = new();
            Expression<Func<Person, bool>> expression = person => false;
            try
            {
                switch (request.Search)
                {
                    case SwitchOptions.SuperPerson:
                        expression = person => person.LastName != null &&
                        person.LastName.Contains(request.Data)
                        || person.Identification.Contains(request.Data);
                        break;
                }
                IQueryable<Person> iQueryable = _repository.GetPerson(expression);
                response.Data=await  BuildPersons(iQueryable);
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
        private async Task<List<PersonDto>> BuildPersons(IQueryable<Person> iQueryable)
        {
            List<PersonDto> listPerson = new();
            try
            {
                listPerson = await iQueryable.Include(person => person.Sex)
                            .Include(person => person.User)
                            .Include(person =>  person.User.Rol)
                            .Select(person => new PersonDto
                            {
                                PersonaId=person.PersonaId,
                                Address=person.Address,
                                UserId=person.UserId,
                                NameUser=person.User !=null ? person.User.NameUser:"",
                                Email=person.User !=null ? person.User.Email:"",
                                Rol=person.User != null && person.User.Rol!=null ? person.User.Rol.DescriptionRol:"",
                                Phone =person.Phone,
                                Name=person.Name,
                                LastName=person.LastName,
                                DateBirth=person.DateBirth,
                                SexId=person.SexId,
                                DescriptionSex=person.Sex!=null? person.Sex.Description:"",
                                Nationality=person.Nationality,
                                MaritalStatus=person.MaritalStatus,
                                Occupation=person.Occupation,
                                Identification=person.Identification,
                                DateCreate=person.DateCreate,
                                DateModificate=person.DateModificate,
                                UserCreate=person.UserCreate,
                                UserModificate=person.UserModificate,
                            }).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return listPerson;
        }
        public async Task<ResponseHttp<bool>> PostPerson(PersonRequest request)
        {
            ResponseHttp<bool> response = new();
            Expression<Func<Person, bool>> expression = personDb => (personDb.User!=null && personDb.User.Email.Equals(request.User.Email));
            try
            {
                var personDb=await _repository.GetPerson(expression).Include(person=>person.User).FirstOrDefaultAsync();
                if (personDb != null)
                {
                    throw new Exception("Identificación ya registrada");
                }
                Person person = _mapper.Map<Person>(request);
                var userValid=person.User ?? throw new Exception("Invalid post Person, sin user");
                person.User.Password = person.User.Password.Encrypted();
                await _repository.PostPerson(person);
                var responseEmail = await SendEmailPost(request);
                if (responseEmail.Data==false) 
                {
                    throw new Exception("Persona registrada, pero no se pudo enviar el correo");
                }
                response.Code = CodeResponse.Create;
                response.Data = true;
                response.Message = MessageResponse.Create;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        private async Task<ResponseHttp<bool>> SendEmailPost(PersonRequest request)
        {
            ResponseHttp<bool> response=new();
            var emailRequest = new EmailRequest
            {
                Subject="Registro en Proyecto de Software",
                Template="Registro.html",
                To=request.User.Email,
                Params= new Dictionary<string, string>
                {
                    { "proyecto","Proyecto Software" },
                    { "link","http://localhost:4200/login" },
                    { "name",$"{request.Name} {request.LastName}" },
                    { "textoEmail","Ingresar a Proyecto Software" },
                    { "password",request.User.Password},

                },

            };
            try
            {
                response= await _emailServices.SendEmail(emailRequest);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        public async Task<ResponseHttp<bool>> UpdatePerson(PersonRequest request)
        {
            ResponseHttp<bool> response = new();
            Expression<Func<Person, bool>> expression = personDb => personDb.PersonaId != request.PersonaId &&  (personDb.Identification.Equals(request.Identification));
            try
            {
                var personDb = await _repository.GetPerson(expression).FirstOrDefaultAsync();
                if (personDb != null)
                {
                    throw new Exception("No puedes actualizar la cedula a una ya existente");
                }
                Person person = _mapper.Map<Person>(request);
                await _repository.UpdatePerson(person);
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
    }
}
