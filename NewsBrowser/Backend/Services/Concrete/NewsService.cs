using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Repositories.Abstract;
using Backend.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public NewsDetails GetNews(string newsId)
        {
            var news = _newsRepository.Get(newsId);
            return DTOMapper.GetNewsDetails(news);
        }

        public IEnumerable<SimpleNews> SimpleSearchNews(string searchQuery, int page)
        {
            return _newsRepository.SimpleSearch(searchQuery, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
        }
    }
}
