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
   
        public class EmailServices(IOptions<EmailCredential> credential)
            : IEmailServices
        {
        private readonly EmailCredential _credential = credential.Value;
        public Task<ResponseHttp<string>> SendEmail(EmailRequest emailDto)
        {
            ResponseHttp<string> response = new();
            try
            {
                var username = _credential.Username ?? throw new Exception("missing username configuration");
                var password = _credential.Password ?? throw new Exception("missing password configuration");
                var smtpClient = new SmtpClient
                {
                    Host = _credential.Host ?? throw new Exception("missing host configuration"),
                    Port = _credential.Port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var from = new MailAddress(_credential.From ?? throw new Exception("missing from configuration"), "Ecuasuiza");
                var receiver = new MailAddress(emailDto.To ?? throw new Exception("missing to value"), "Beneficiario");

                var mailMessage = ParserMailMessage(new MailMessage(from, receiver)
                {
                    IsBodyHtml = true,
                    Subject = emailDto.Subject ?? "Valor por defecto",
                    Body = GetDocument(emailDto.Template ?? "<h1>Valor por defecto</h1>", emailDto.Params)
                });

                smtpClient.Send(mailMessage);

                response.Code = CodeResponse.Ok;
                response.Message = MessageResponse.Ok;
            }
            catch (Exception ex)
            {
                throw;
            }

            return Task.FromResult(response);
        }

        private MailMessage ParserMailMessage(MailMessage mailMessage)
        {
            try
            {
                foreach (var adr in _credential.Bcc.Split(";"))
                {
                    mailMessage.Bcc.Add(adr);
                }

                return mailMessage;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetDocument(string template, Dictionary<string, string> parameters)
        {
            string body;

            try
            {
                var config = Configuration.GetConfiguration();
                var path = config["RutaDocuments"] + template;

                body = ReplaceParams(File.ReadAllText(path), parameters);
            }
            catch (Exception ex)
            {
                throw;
            }
            return body;
        }

        private static string ReplaceParams(string body, Dictionary<string, string> parameters)
        {
            string bodyParams;
            try
            {
                foreach (var param in parameters.Where(parameter => body.Contains(parameter.Key)))
                {
                    body = body.Replace("[" + param.Key + "]", param.Value);
                }

                bodyParams = body;
            }
            catch (Exception ex)
            {
                throw;
            }

            return bodyParams;
        }
    }
}
