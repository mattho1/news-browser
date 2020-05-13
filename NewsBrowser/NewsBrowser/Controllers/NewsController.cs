using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace NewsBrowser.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
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

        // TEST
        public IEnumerable<SimpleNews> Get()
        {
            var news = _newsService.SimpleSearchNews("test", 1);
            return news;
        }
    }
}