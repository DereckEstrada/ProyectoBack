using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProyectoSoftware.Back.BE.Utilitarian
{
    public class ResponseHttp<T>
    {
        public int? Code { get; set; }
        public T? Data{ get; set; }
        public string? Message { get; set; }
    }
}
