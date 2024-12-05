using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Utilitarian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.DAL.Interfaces
{
    public interface ISexRepository
    {
        Task<List<Sex>> GetSex();
    }
}
