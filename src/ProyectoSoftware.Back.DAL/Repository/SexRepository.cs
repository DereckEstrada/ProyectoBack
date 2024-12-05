using Microsoft.EntityFrameworkCore;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.DAL.Repository
{
    public class SexRepository:ISexRepository
    {
        private readonly ProyectoSoftwareDbContext _context;

        public SexRepository(ProyectoSoftwareDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Sex>> GetSex()
        {
            List<Sex> listSex= new();
            try
            {
                listSex = await _context.Sexes.ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return listSex; 
        }
    }
}
