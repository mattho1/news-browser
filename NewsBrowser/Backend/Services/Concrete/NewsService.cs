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


        public IEnumerable<SimpleNews> SearchByField(string searchQuery, string fieldName, int page)
        {
            fieldName = fieldName.ToLower();
            if (fieldName.Equals("all"))
                return _newsRepository.SimpleSearch(searchQuery, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
            else if (fieldName.Equals("tag"))
            {
                var fieldsName = new List<string>() { "thread.site", "entities.persons.name", "entities.locations.name", "entities.organizations.name" };
                return _newsRepository.SearchByFields(searchQuery, fieldsName, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
            }
            else
                return _newsRepository.SearchByField(searchQuery, fieldName, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
        }
    }
}
