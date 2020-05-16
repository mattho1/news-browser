using Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Repositories.Abstract
{
    public interface INewsRepository
    {
        News Get(string id);
        //void Add(News news);
        IEnumerable<News> SimpleSearch(string searchQuery, int page);
        IEnumerable<News> SearchByField(string searchQuery, string fieldName, int page);
        IEnumerable<News> SearchByFields(string searchQuery, List<string> fieldsName, int page);
    }
}
