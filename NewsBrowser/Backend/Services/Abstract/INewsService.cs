using Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Abstract
{
    public interface INewsService
    {
        News GetNews(string newsId);
        //bool CreateNews(News news);
        IEnumerable<News> SimpleSearchNews(string searchQuery, int page);
    }
}
