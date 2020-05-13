using Backend.DTOs;
using Backend.Models;
using Backend.Repositories.Abstract;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Repositories.Concrete
{
    public class NewsRepository : INewsRepository
    {
        private readonly IElasticClient _elasticClient;
        private const int PageSize = 10;

        public NewsRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public News Get(string id)
        {
            var response = _elasticClient.Search<News>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                            .Match(m => m
                                .Field(f => f.Id)
                                .Query(id)
                            )
                    )
            )));
            return response.Documents.FirstOrDefault();
        }

        public IEnumerable<News> SimpleSearch(string searchQuery, int page)
        {
            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                            .QueryString(qs => qs
                                .Query(searchQuery)
                            )
                    )
            ))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }
    }
}
