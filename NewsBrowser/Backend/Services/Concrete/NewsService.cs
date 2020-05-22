using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Repositories.Abstract;
using Backend.Services.Abstract;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Services.Concrete
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly ISubscribeService _subscribeService;

        public NewsService(INewsRepository newsRepository, ISubscribeService subscribeService)
        {
            _newsRepository = newsRepository;
            _subscribeService = subscribeService;
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

        public News SimpleSearchNewsTEST(string searchQuery)
        {
            return _newsRepository.Get(searchQuery);
        }

        public IEnumerable<SimpleNews> SearchByField(string searchQuery, string fieldName, int page)
        {
            fieldName = fieldName.ToLower();

            if (fieldName.Equals("content"))
                fieldName = "text";

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

        public IEnumerable<SimpleNews> CombinationSearch(string queryType, string fieldName, string searchQuery, int page)
        {
            fieldName = fieldName.ToLower();
            queryType = queryType.ToLower();

            if (fieldName.Equals("content"))
                fieldName = "text";

            if (fieldName.Equals("all"))
            {
                if (queryType.Equals("fuzzy"))
                    return _newsRepository.FuzzySearchByAllFields(searchQuery, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
                else if(queryType.Equals("synonyms"))
                    return _newsRepository.SynonymsSearchByAllFields(searchQuery, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
                else
                    return _newsRepository.SimpleSearch(searchQuery, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
            }
            else if (fieldName.Equals("tag"))
            {
                var fieldsName = new List<string>() { "thread.site", "entities.persons.name", "entities.locations.name", "entities.organizations.name" };

                if (queryType.Equals("fuzzy"))
                    return _newsRepository.FuzzySearchByFields(searchQuery, fieldsName, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
                else if (queryType.Equals("synonyms"))
                    return _newsRepository.SynonymsSearchByFields(searchQuery, fieldsName, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
                else
                    return _newsRepository.SearchByFields(searchQuery, fieldsName, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
            }
            else
            {
                if (queryType.Equals("fuzzy"))
                    return _newsRepository.FuzzySearchByFields(searchQuery, new List<string>() { fieldName }, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
                else if (queryType.Equals("synonyms"))
                    return _newsRepository.SynonymsSearchByFields(searchQuery, new List<string>() { fieldName }, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
                else
                    return _newsRepository.SearchByField(searchQuery, fieldName, page).Select(n => DTOMapper.GetSimpleNews(n)).ToList();
            }
        }

        void INewsService.CreateNews(News news)
        {
            var idNewNews = _newsRepository.AddNews(news);
            _subscribeService.AddedNewNews(idNewNews);
        }

        public IEnumerable<News> AggregationTags(string searchQuery, List<string> fieldsName, int page)
        {
            return _newsRepository.AggregationTags(searchQuery, fieldsName, page).ToList();
        }
    }
}
