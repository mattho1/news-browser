using Backend.Models;
using Backend.Repositories.Abstract;
using Backend.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Services.Concrete
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public News GetNews(string newsId)
        {
            var news = _newsRepository.Get(newsId);
            return news;
        }

        public IEnumerable<News> SimpleSearchNews(string searchQuery, int page)
        {
            var news = _newsRepository.SimpleSearch(searchQuery, page);
            return news;
        }
    }
}
