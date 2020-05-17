using Backend.Helpers;
using Backend.Models;
using Backend.Repositories.Abstract;
using Backend.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IEmailSender _emailSender;

        public EmailService(INewsRepository newsRepository, IEmailSender emailSender)
        {
            _newsRepository = newsRepository;
            _emailSender = emailSender;
        }

        public async Task SendMessage(EmailMessage sendEmailMessage)
        {
            await _emailSender.SendEmail(sendEmailMessage);
        }
        public async Task SendConfirmationSubscribeMessage(string email, string subscribeQuery)
        {
            await _emailSender.SendEmail(new EmailMessage()
            {
                Receiver = email,
                Title = "New subscription",
                Content = $"New subscription has been added to News Browser. \n\nSubscribe query: \n{subscribeQuery}"
            });
        }
    }
}
