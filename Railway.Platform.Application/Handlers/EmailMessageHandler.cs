using Railway.EventConsumer.CrossCutting.HandleErrors;
using Railway.EventConsumer.Domain.Events;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Railway.EventConsumer.Domain.Enums;
using Railway.EventConsumer.Application.Messaging;
using Microsoft.Extensions.Logging;

namespace Railway.EventConsumer.Application.Handlers
{
    public class EmailMessageHandler : IMessageHandler
    {
        public string MessageType => "SendEmail";

        private readonly IConfiguration _config;
        private readonly ILogger<EmailMessageHandler> _logger;

        public EmailMessageHandler(IConfiguration config, ILogger<EmailMessageHandler> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<HandlerResult> HandleAsync(JsonElement data, int retryCount)
        {
            try
            {
                var message = JsonSerializer.Deserialize<EmailMessaje>(data.GetRawText());
                if (message == null)
                {
                    _logger.LogError(Errors.InvalidMessage.Description);
                    return new HandlerResult { Status = HandlerResultStatus.Discard, ErrorMessage = Errors.InvalidMessage.Description };
                }

                var host = _config["SMTP:HOST"];
                var port = int.Parse(_config["SMTP:PORT"]!);
                var user = _config["SMTP:USER"];
                var pass = _config["SMTP:PASS"];

                using var smtp = new SmtpClient(host)
                {
                    Port = port,
                    Credentials = new NetworkCredential(user, pass),
                    EnableSsl = true
                };

                var mail = new MailMessage(user!, message.To, message.Subject, message.Body)
                {
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(mail);

                return new HandlerResult { Status = HandlerResultStatus.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email: {Message}", ex.Message);

                if (retryCount < 1)
                    return new HandlerResult { Status = HandlerResultStatus.Retry, ErrorMessage = ex.Message };

                return new HandlerResult { Status = HandlerResultStatus.Discard, ErrorMessage = ex.Message };
            }
        }
    }
}
