using Backend.Helpers;
using Backend.Models;
using Backend.Repositories.Abstract;
using Backend.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Concrete
{
    public class SubscribeService : ISubscribeService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IEmailService _emailService;
        private readonly ISubscribeRepository _subscribeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubscribeService(INewsRepository newsRepository, ISubscribeRepository subscribeRepository, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _newsRepository = newsRepository;
            _subscribeRepository = subscribeRepository;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddedNewNews(string idNews)
        {
            if (string.IsNullOrEmpty(idNews))
                return;

            var queriesOneSubscriber = _subscribeRepository
                                        .GetAllSubscribers()
                                        .GroupBy(s => s.Email,
                                                 s => s.Query,
                                                 (email, queries) => new
                                                 {
                                                    Email = email,
                                                    Queries = queries
                                                 });

            foreach (var subscriber in queriesOneSubscriber)
            {
                foreach (var query in subscriber.Queries)
                {
                    var result = _newsRepository.CheckExistNews(query, idNews);
                    if (!string.IsNullOrEmpty(result))
                    {
                        SendMessageAboutNewNews(subscriber.Email, result);
                        break;
                    }
                }
            }
        }

        public void AddSubscribe(string email, string subscribeQuery)
        {
            _subscribeRepository.Add(new Subscriber() { Id = Cryptography.GetHashString(email + subscribeQuery), DateSubscribe = DateTime.Now, Email = email, Query = subscribeQuery });
        }

        public void RemoveSubscribe(string id)
        {
            _subscribeRepository.Remove(id);
        }

        public async Task SendConfirmationSubscribeMessage(string email, string subscribeQuery)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var unsubscribeLink = new Uri(new Uri($"{request.Scheme}://{request.Host.Value}"), "unsubscribe/" + Cryptography.GetHashString(email + subscribeQuery)).ToString();
            await _emailService.SendMessage(new EmailMessage()
            {
                Receiver = email,
                Title = "New subscription",
                Content = $"New subscription has been added to News Browser. \n\nSubscribe query: \n{subscribeQuery}\n\nIf you want to unsubscribe, click the link:\n{unsubscribeLink}"
            });
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
                await _emailService.SendMessage(emailMessage);
            }
            catch
            {}
        }
    }
}
