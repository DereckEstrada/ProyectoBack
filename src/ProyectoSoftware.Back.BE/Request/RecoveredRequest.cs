using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Request
{
    public class RecoveredRequest
    {
        [Required]
        public required string Token { get; set; }        
        public string? Password { get; set; }
    }
}
