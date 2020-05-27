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
                    .Fuzzy(f => f
                        .Name(searchQuery)
                        .Fuzziness(Fuzziness.EditDistance(1))
                        .Transpositions(false))
            )
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> SynonymsSearchByAllFields(string searchQuery, int page)
        {
            searchQuery = PrepareSynonymsQuery(searchQuery);

            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
                .Wildcard(r => r
                    .Value(searchQuery))
            )
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
                    .Fuzziness(Fuzziness.EditDistance(1))
            ))
            .From((page - 1) * PageSize)
            .Size(PageSize));
            return searchResponse.Documents;
        }

        public IEnumerable<News> SynonymsSearchByFields(string searchQuery, List<string> fieldsName, int page, TextQueryType? typeQuery)
        {
            searchQuery = PrepareSynonymsQuery(searchQuery);

            var searchResponse = _elasticClient.Search<News>(s => s
            .Query(q => q
                .Wildcard(r => r
                    .Value(searchQuery)))
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

        public IEnumerable<News> AggregationTags(string searchQuery, List<string> fieldsName, int page, TextQueryType? typeQuery)
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
            )));

            var searchResponse2 = _elasticClient.Search<News>(s => s
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
            .Aggregations(a => a
                .Terms("aggLocations", ag => ag
                    .Field(f => f.Entities.Locations))
                .ValueCount("valueCountLocations", v => v
                    .Field(f => f.Entities.Locations)
                ))
            .Aggregations(a => a
                .Terms("aggPersons", ag => ag
                    .Field(f => f.Entities.Persons))
                .ValueCount("valueCountPersons", v => v
                    .Field(f => f.Entities.Persons)
                ))
            .Aggregations(a => a
                .Terms("aggOrganizations", ag => ag
                    .Field(f => f.Entities.Organizations))
                .ValueCount("valueCountOrganizations", v => v
                    .Field(f => f.Entities.Organizations)
                )));



            var searchResponse3 = _elasticClient.Search<News>(s => s
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
            .Aggregations(a => a
                .Terms("aggOrganizations", ag => ag
                    .Field(f => f.Entities.Organizations))));

            var searchResponse4 = _elasticClient.Search<News>(s => s
           .Aggregations(a => a
               .Terms("aggOrganizations", ag => ag
                   .Field(f => f.Entities.Organizations))));

            var commitCount = searchResponse2.Aggregations.Terms("aggLocations");
            var commitCount2 = searchResponse2.Aggregations.Terms("aggPersons");
            var commitCount3 = searchResponse2.Aggregations.Terms("aggOrganizations");

            var commitCount1 = searchResponse2.Aggregations.ValueCount("valueCountLocations");
            var commitCount12 = searchResponse2.Aggregations.ValueCount("valueCountPersons");
            var commitCount13 = searchResponse2.Aggregations.ValueCount("valueCountOrganizations");


            var commitCount7 = searchResponse3.Aggregations.Terms("aggLocations");
            var commitCount72 = searchResponse3.Aggregations.Terms("aggPersons");
            var commitCount73 = searchResponse3.Aggregations.Terms("aggOrganizations");

            var commitCount111 = searchResponse4.Aggregations.Terms("aggOrganizations");


            //.Terms("Test", st => st
            //    .Field(f => f.Entities.Locations)
            //    .Order()));


            //.Query(q => q
            //.MultiMatch(mm => mm
            //    .Fields(f => f.Fields(fieldsName.ToArray()))
            //    .Query(searchQuery)
            //    .AutoGenerateSynonymsPhraseQuery(true)
            //    .Type(typeQuery)
            //))
            //.From((page - 1) * PageSize)
            //.Size(PageSize));
            return searchResponse.Documents;
        }

        private string PrepareSynonymsQuery(string searchQuery)
        {
            var words = searchQuery.Split(' ').ToList();
            searchQuery = string.Empty;
            foreach (var word in words)
                searchQuery += word.Substring(0, word.Length - 1) + "*" + " ";

            return searchQuery.TrimEnd();
        }
    }
}
