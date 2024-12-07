using ProyectoSoftware.Back.BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Request
{
    public class PersonRequest
    {
        public int PersonaId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string Address { get; set; } = null!;
        public string? Phone { get; set; }
        public DateOnly? DateBirth { get; set; }
        public int? SexId { get; set; }
        public string? Nationality { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Occupation { get; set; }
        public string Identification { get; set; } = null!;
        public DateTime DateCreate { get; set; }
        public DateTime? DateModificate { get; set; }
        public int? UserCreate { get; set; }
        public int? UserModificate { get; set; }        
        public virtual User? User { get; set; } = null!;
    }
}
