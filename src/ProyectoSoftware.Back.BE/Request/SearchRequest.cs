using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Request
{
    public class SearchRequest
    {
        [Required]
        public required string Search{ get; set; }
        [Required]
        public required string Data{ get; set; }
    }
}
