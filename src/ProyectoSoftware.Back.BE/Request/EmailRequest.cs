using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Request
{
    public class EmailRequest
    {
        [Required]
        public required string? To { get; set; }
        [Required]
        public required string? Subject { get; set; }
        [Required]
        public required string? Template { get; set; }
        [Required]
        public required Dictionary<string, string>? Params { get; set; }
    }
}
