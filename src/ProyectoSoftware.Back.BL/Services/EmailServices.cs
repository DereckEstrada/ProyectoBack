using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BL.Interfaces;

using System.Net;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BE.Const;
using System.Net.Mail;

namespace ProyectoSoftware.Back.BL.Services
{
   
        public class EmailServices(IOptions<EmailCredential> credential )
            : IEmailServices
        {
        private readonly EmailCredential _credential = credential.Value;

        public async Task<ResponseHttp<bool>> SendEmail(EmailRequest emailDto)
        {
            ResponseHttp<bool> response = new();
            try
            {               
                string username = this._credential.Username ?? throw new Exception("Falta de credenciales");
                string password = this._credential.Password ?? throw new Exception("Falta de credenciales");
                int port = this._credential.Port;
                string host = this._credential.Host ?? throw new Exception("Falta de configuración");
                var message = new MailMessage();
                message.From = new MailAddress(username);
                message.To.Add(new MailAddress(emailDto.To ?? throw new Exception("Sin email a cual enviar")));
                message.Subject = emailDto.Subject;
                message.Body = GetDocument(emailDto.Template ?? throw new Exception("Sin template"), emailDto.Params ?? throw new Exception("Sin parametros"));
                message.IsBodyHtml = true;
                var smtpClient = new SmtpClient(host)
                {
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,                   
                };

                smtpClient.Send(message);

                response.Code = CodeResponse.Accepted;
                response.Data = true;
                response.Message = MessageResponse.Accepted;
            }
            catch (Exception)
            {
                response.Data=false;
            }
            return response;
        }
       
        public string GetDocument(string template, Dictionary<string, string> parametros)
        {
            var body = string.Empty;
            try
            {
                var config = Configuration.GetConfiguration();
                var ruta = config["RutaDocuments"] + template;
                string bodySinParams = File.ReadAllText(ruta);
                body = this.ReplaceParams(bodySinParams, parametros);
            }
            catch (Exception)
            {
                throw;
            }
            return body;
        }
        public string ReplaceParams(string body, Dictionary<string, string> parametros)
        {
            var bodyParams = string.Empty;
            try
            {
                foreach (var parametro in parametros)
                {
                    if (body.Contains(parametro.Key))
                    {
                        body = body.Replace("[" + parametro.Key + "]", parametro.Value);
                    }
                }
                bodyParams = body;
            }
            catch (Exception)
            {
                throw;
            }
            return bodyParams;
        }
    }
}
