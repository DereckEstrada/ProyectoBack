﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Request
{
    public class AuthenticationRequest
    {
        [Required]
        public required string Email { get; set; }
        public  string? Token{ get; set; }
        [Required]
        public required string Password{ get; set; }
    }
}