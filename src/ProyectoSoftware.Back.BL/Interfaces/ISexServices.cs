using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Utilitarian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BL.Interfaces
{
    public interface ISexServices
    {
        Task<ResponseHttp<List<Sex>>> GetSex();
    }
}
