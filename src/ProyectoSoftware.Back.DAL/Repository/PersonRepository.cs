using Microsoft.EntityFrameworkCore;
using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.DAL.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ProyectoSoftwareDbContext _context;

        public PersonRepository(ProyectoSoftwareDbContext context)
        {
            this._context = context;
        }
        public async Task DeletePerson(Person person)
        {
            try
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  IQueryable<Person> GetPerson(Expression<Func<Person, bool>> expression)
        {
            IQueryable<Person> iQueryable;
            try
            {
                iQueryable = _context.People.Where(expression);                
            }
            catch (Exception)
            {
                throw;
            }
            return iQueryable;
        }

        public async Task PostPerson(Person person)
        {
            try
            {
                _context.People.Add(person);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdatePerson(Person person)
        {
            try
            {
                _context.People.Update(person);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
