using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Abstract
{
    public interface INewsService
    {
        NewsDetails GetNews(string newsId);
        //bool CreateNews(News news);
        void CreateNews(News news);
        IEnumerable<SimpleNews> SimpleSearchNews(string searchQuery, int page);
        IEnumerable<SimpleNews> SearchByField(string searchQuery, string fieldName, int page);
        IEnumerable<SimpleNews> CombinationSearch(string queryType, string fieldName, string searchQuery, int page);


        News SimpleSearchNewsTEST(string searchQuery);
    }
}
