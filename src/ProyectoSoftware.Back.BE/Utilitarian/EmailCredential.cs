using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Utilitarian
{
    public class EmailCredential
    {
        public const string Credential = "MailCredential";
        public string Bcc { get; set; } = "";
        public string? Host { get; set; }
        public string? From { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; } = 587;

    }
}
