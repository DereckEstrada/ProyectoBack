using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;

    }
}
