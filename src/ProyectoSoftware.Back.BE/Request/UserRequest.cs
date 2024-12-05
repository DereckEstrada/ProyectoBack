using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Request
{
    public class UserRequest
    {
        public int UserId { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Active { get; set; }
        public int? RolId { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateModificate { get; set; }
        public int? UserCreate { get; set; }
        public int? UserModificate { get; set; }
        public int Attempts { get; set; }
        public bool Blocked { get; set; }
        public DateTime? DateLastAttempts { get; set; }
    }
}
