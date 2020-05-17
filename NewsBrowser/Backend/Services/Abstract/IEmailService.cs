using Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Abstract
{
    public interface IEmailService
    {
        Task SendMessage(EmailMessage emailMessage);
        Task SendConfirmationSubscribeMessage(string email, string subscribeQuery);
    }
}
