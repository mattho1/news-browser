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
        private readonly ISemanticService _semanticService;

        public NewsController(INewsService newsService, ISemanticService semanticService)
        {
            _newsService = newsService;
            _semanticService = semanticService;
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

        [HttpGet("broaderQuery/{searchPhrase}", Name = "BroaderQuery")]
        public IActionResult BroaderQuery(string searchPhrase)
        {
            var simQry = _semanticService.GetBroaderConcepts(searchPhrase);
            return Ok(simQry);
        }

        [HttpGet("narrowerQuery/{searchPhrase}", Name = "NarrowerQuery")]
        public IActionResult NarrowerQuery(string searchPhrase)
        {
            var simQry = _semanticService.GetNarrowerConcepts(searchPhrase);
            return Ok(simQry);
        }

        [HttpGet("relatedQuery/{searchPhrase}", Name = "RelatedQuery")]
        public IActionResult RelatedQuery(string searchPhrase)
        {
            var simQry = _semanticService.GetRelatedConcepts(searchPhrase);
            return Ok(simQry);
        }

        // TEST
        public IEnumerable<SimpleNews> Get()
        {
            var news = _newsService.SimpleSearchNews("test", 1);
            return news;
        }
    }
}