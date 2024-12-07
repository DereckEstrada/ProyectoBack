using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Utilitarian
{
    public class TokenJWT
    {
        [Required]
        public required string Token { get; set; }   
        public  string? User { get; set; }
        public  int? Rol{ get; set; }
        public DateTime? DateExpirated { get; set; }

    }
}
