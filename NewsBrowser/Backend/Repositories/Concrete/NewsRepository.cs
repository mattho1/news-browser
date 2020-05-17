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
        private const int PageSize = 200;

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
                                .Type(TextQueryType.Phrase)
                                .DefaultOperator(Operator.And)
                            )
                    )
            ))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> FuzzySearchByAllFields(string searchQuery, int page)
        {
            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                            .QueryString(qs => qs
                                .Query(searchQuery)
                                .FuzzyTranspositions(true)
                                .FuzzyPrefixLength(3)
                                .Fuzziness(Fuzziness.EditDistance(2)
                            )
                    )
            )))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> SynonymsSearchByAllFields(string searchQuery, int page)
        {
            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                            .QueryString(qs => qs
                                .Query(searchQuery)
                                .AutoGenerateSynonymsPhraseQuery(true)
                            )
                    )
            ))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> SearchByField(string searchQuery, string fieldName, int page)
        {
            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
            .MatchPhrasePrefix(mm => mm
                .Field(fieldName)
                .Query(searchQuery)
            ))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> SearchByFields(string searchQuery, List<string> fieldsName, int page, TextQueryType? typeQuery)
        {
            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
            .MultiMatch(mm => mm
                .Fields(f => f.Fields(fieldsName.ToArray()))
                .Query(searchQuery)
                .Type(typeQuery)
            ))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> FuzzySearchByFields(string searchQuery, List<string> fieldsName, int page)
        {
            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
            .MultiMatch(mm => mm
                .Fields(f => f.Fields(fieldsName.ToArray()))
                .Query(searchQuery)
                .FuzzyTranspositions(true)
                .PrefixLength(3)
                .Fuzziness(Fuzziness.EditDistance(2))
            ))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> SynonymsSearchByFields(string searchQuery, List<string> fieldsName, int page, TextQueryType? typeQuery)
        {
            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
            .MultiMatch(mm => mm
                .Fields(f => f.Fields(fieldsName.ToArray()))
                .Query(searchQuery)
                .AutoGenerateSynonymsPhraseQuery(true)
                .Type(typeQuery)
            ))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> SearchByFieldsWithFilters(string searchQuery, List<string> fieldsName, int page, string siteFilter, TextQueryType? typeQuery)
        {
            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
            .Bool(b => b
            .Must(m => m
                .MultiMatch(mm => mm
                    .Fields(f => f.Fields(fieldsName.ToArray()))
                    .Query(searchQuery)
                    .Type(typeQuery)
                ) && m
                .MatchPhrase(mp => mp
                    .Field(f => f.Thread.Site)
                    .Query(siteFilter)))))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public string AddNews(News news)
        {
            return _elasticClient.Index<News>(news, i => i).Id;
        }

        public string CheckExistNews(string searchQuery, string idNews)
        {
            return _elasticClient.Search<News>(s => s
                    .Query(q => q
                        .Bool(b => b
                            .Must(m => m
                                .QueryString(qs => qs
                                    .Query(searchQuery)
                                    .Type(TextQueryType.Phrase)
                                    .DefaultOperator(Operator.And)
                                ) && m
                                .MatchPhrase(t => t
                                    .Field("_id")
                                    .Query(idNews)))
                                    )))?.Documents.FirstOrDefault()?.Id;
        }
    }
}
