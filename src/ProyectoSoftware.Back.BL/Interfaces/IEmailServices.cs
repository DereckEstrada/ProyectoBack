using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BE.Utilitarian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BL.Interfaces
{
    public interface IEmailServices
    {
        Task<ResponseHttp<bool>> SendEmail(EmailRequest request);

    }
}
