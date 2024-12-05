using ProyectoSoftware.Back.BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.DAL.Interfaces
{
    public interface IRolRepository
    {
        Task<List<Rol>> GetRol();
    }
}
