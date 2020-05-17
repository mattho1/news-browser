using Backend.Helpers;
using Backend.Models;
using Backend.Repositories.Abstract;
using Backend.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Concrete
{
    public class SubscribeService : ISubscribeService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IEmailSender _emailSender;
        private readonly ISubscribeRepository _subscribeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubscribeService(INewsRepository newsRepository, ISubscribeRepository subscribeRepository, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _newsRepository = newsRepository;
            _subscribeRepository = subscribeRepository;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddedNewNews(string idNews)
        {
            if (string.IsNullOrEmpty(idNews))
                return;

            var subscribers = _subscribeRepository.GetAllSubscribers();
            foreach(var subscriber in subscribers)
            {
                var result = _newsRepository.CheckExistNews(subscriber.Query, idNews);
                if (!string.IsNullOrEmpty(result))
                    SendMessageAboutNewNews(subscriber.Email, result);
            }
        }

        public void AddSubscribe(string email, string subscribeQuery)
        {
            _subscribeRepository.Add(new Subscriber() { DateSubscribe = DateTime.Now, Email = email, Query = subscribeQuery });
        }

        private async Task SendMessageAboutNewNews(string email, string newsId)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext.Request;
                var linkToNews = new Uri(new Uri($"{request.Scheme}://{request.Host.Value}"), "news-details/" + newsId).ToString();
                var message = "News has been added that matches your subscription.\n\n" + linkToNews;
                var title = $"[NewsBrowser] - New news";
                var emailMessage = new EmailMessage() { Receiver = email, Title = title, Content = message };
                await _emailSender.SendEmail(emailMessage);
            }
            catch
            {}
        }
    }
}
