using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Dtos
{
    public class PersonDto
    {
        public int? PersonaId { get; set; }
        public string? Address { get; set; } 
        public int? UserId { get; set; }
        public string? NameUser { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Rol { get; set; }
        public DateOnly? DateBirth { get; set; }
        public int? SexId { get; set; }
        public string? DescriptionSex { get; set; }
        public string? Nationality { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Occupation { get; set; }
        public string? Identification { get; set; } 
        public DateTime? DateCreate { get; set; }
        public DateTime? DateModificate { get; set; }
        public int? UserCreate { get; set; }
        public int? UserModificate { get; set; }
    }
}
