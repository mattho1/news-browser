using Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public interface IEmailSender
    {
        Task SendEmail(EmailMessage message);
    }
}
