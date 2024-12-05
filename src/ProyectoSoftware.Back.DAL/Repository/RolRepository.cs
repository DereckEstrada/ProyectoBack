using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.DAL.Repository
{
    public class RolRepository : IRolRepository
    {
        private readonly ProyectoSoftwareDbContext _context;

        public RolRepository(ProyectoSoftwareDbContext context)
        {
            this._context = context;
        }
        public async Task<List<Rol>> GetRol()
        {
            List<Rol> listRol = new();
            try
            {
                listRol = await _context.Rols.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return listRol;
        }
    }
}
