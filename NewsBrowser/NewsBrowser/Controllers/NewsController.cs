using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Abstract;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace NewsBrowser.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IEmailService _emailService;

        public NewsController(INewsService newsService, IEmailService emailService)
        {
            _newsService = newsService;
            _emailService = emailService;
        }

        // GET: News/1
        [HttpGet("{id}", Name = "GetNewsById")]
        public ActionResult<NewsDetails> Get(string id)
        {
            var news = _newsService.GetNews(id);
            return Ok(news);
        }

        // GET: News/simple/test
        [HttpGet("simple/{searchPhrase}", Name = "SimpleSearch")]
        public IActionResult SimpleSearch(string searchPhrase, int page = 1)
        {
            var news = _newsService.SimpleSearchNews(searchPhrase, page);
            return Ok(news);
        }

        [HttpGet("advanced/{fieldName}/{searchPhrase}", Name = "SearchByField")]
        public IActionResult SearchByField(string fieldName, string searchPhrase, int page = 1)
        {
            var news = _newsService.SearchByField(searchPhrase, fieldName, page);
            return Ok(news);
        }

        [HttpGet("combination", Name = "CombinationSearch")]
        public IActionResult CombinationSearch(string queryType, string fieldType, string queryContent, int page = 1)
        {
            var news = _newsService.CombinationSearch(queryType, fieldType, queryContent,  page);
            return Ok(news);
        }

        // TEST
        public IEnumerable<SimpleNews> Get()
        {
            var news = _newsService.SimpleSearchNews("test", 1);
            return news;
        }

        // TEST
        [HttpGet("addedNews/{idNews}", Name = "TestAddedAndSubscribe")]
        public IActionResult GetTest(string idNews)
        {
            var news = _newsService.SimpleSearchNewsTEST(idNews);
            _newsService.CreateNews(news);
            return Ok();
        }
    }
}