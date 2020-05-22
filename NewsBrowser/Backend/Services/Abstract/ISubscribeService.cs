using Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Abstract
{
    public interface ISubscribeService
    {
        void AddedNewNews(string idNews);
        void AddSubscribe(string email, string subscribeQuery);
        void RemoveSubscribe(string id);
        Task SendConfirmationSubscribeMessage(string email, string subscribeQuery);
    }
}
