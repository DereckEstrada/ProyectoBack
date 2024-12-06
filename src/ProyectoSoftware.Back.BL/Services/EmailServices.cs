using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProyectoSoftware.Back.BE.Request;
using ProyectoSoftware.Back.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Headers;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BE.Const;

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
                string username = this._credential.Username;
                string password = this._credential.Password;
                int port = this._credential.Port;
                string host = this._credential.Host;
                var message = new MailMessage();
                message.From = new MailAddress(username);
                message.To.Add(new MailAddress(emailDto.To));
                message.Subject = emailDto.Subject;
                message.Body = GetDocument(emailDto.Template, emailDto.Params);
                message.IsBodyHtml = true;
                var smtpClient = new SmtpClient(host)
                {
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };
                await smtpClient.SendMailAsync(message);

                response.Code = CodeResponse.Accepted;
                response.Data = true;
                response.Message = MessageResponse.Accepted;
            }
            catch (Exception )
            {
                throw;
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
