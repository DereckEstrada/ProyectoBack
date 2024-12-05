using AutoMapper;
using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Utilitarian
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            ConfigurarMapeoPerson();
            ConfigurarMapeoUserRequest();
            ConfigurarMapeoUser();
            ConfigurarMapeoUserDto();
        }
        private void ConfigurarMapeoPerson()
        {
            CreateMap<PersonRequest, Person>();
        }
        private void ConfigurarMapeoUser()
        {
            CreateMap<UserRequest, User>();
        }
        private void ConfigurarMapeoUserRequest()
        {
            CreateMap<User, UserRequest>();
        }
        private void ConfigurarMapeoUserDto()
        {
            CreateMap<User, UserDto>();
        }
    }
}
