using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.DAL.Interfaces
{
    public interface IPersonRepository
    {
        IQueryable<Person> GetPerson(Expression<Func<Person, bool>> expression);
        Task PostPerson(Person person);
        Task UpdatePerson(Person person);
        Task DeletePerson(Person person);
    }
}
