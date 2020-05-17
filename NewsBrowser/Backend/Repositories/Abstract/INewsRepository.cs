using Backend.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Repositories.Abstract
{
    public interface INewsRepository
    {
        News Get(string id);
        string AddNews(News news);
        IEnumerable<News> SimpleSearch(string searchQuery, int page);
        IEnumerable<News> FuzzySearchByAllFields(string searchQuery, int page);
        IEnumerable<News> SynonymsSearchByAllFields(string searchQuery, int page);
        IEnumerable<News> SearchByField(string searchQuery, string fieldName, int page);
        IEnumerable<News> SearchByFields(string searchQuery, List<string> fieldsName, int page, TextQueryType? typeQuery = TextQueryType.Phrase);
        IEnumerable<News> FuzzySearchByFields(string searchQuery, List<string> fieldsName, int page);
        IEnumerable<News> SynonymsSearchByFields(string searchQuery, List<string> fieldsName, int page, TextQueryType? typeQuery = TextQueryType.Phrase);
        IEnumerable<News> SearchByFieldsWithFilters(string searchQuery, List<string> fieldsName, int page, string siteFilter, TextQueryType? typeQuery = TextQueryType.Phrase);

        string CheckExistNews(string searchQuery, string idNews);

    }
}
