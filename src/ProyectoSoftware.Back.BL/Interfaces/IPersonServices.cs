﻿using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BE.Utilitarian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BL.Interfaces
{
    public interface IPersonServices
    {
        Task<ResponseHttp<List<PersonDto>>> GetPerson(SearchRequest request);
        Task<ResponseHttp<bool>> PostPerson(PersonRequest request);
        Task<ResponseHttp<bool>> UpdatePerson(PersonRequest request);
        Task<ResponseHttp<bool>> DeletePerson(PersonRequest request);
    }
}