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
    public class SubscribeRepository : ISubscribeRepository
    {
        private readonly IElasticClient _elasticClient;
        private const int PageSize = 400;

        public SubscribeRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public Subscriber Get(string email)
        {
            var response = _elasticClient.Search<Subscriber>(s => s
                .Index("subscriptions")
                .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                            .Match(m => m
                                .Field(f => f.Email)
                                .Query(email)
                            )
                    ))));
            return response.Documents.FirstOrDefault();
        }

        public List<Subscriber> GetAllSubscribers()
        {
            var response = _elasticClient.Search<Subscriber>(c => c
                .Index("subscriptions")
                .Query(q => q
                    .MatchAll()));
            return response.Documents.ToList();
        }

        public void Add(Subscriber subscriber)
        {
            if(!string.IsNullOrEmpty(subscriber.Query) && !string.IsNullOrEmpty(subscriber.Email))
            {
                //var numberIdenticalSubscribes = _elasticClient.Search<Subscriber>(s => s
                //    .Index("subscriptions")
                //    .Query(q => q
                //        .Bool(b => b
                //            .Must(m => m
                //                .MatchPhrase(t => t
                //                    .Field(f => f.Query)
                //                    .Query(subscriber.Query))
                //                && m
                //                .MatchPhrase(t => t
                //                    .Field(f => f.Email)
                //                    .Query(subscriber.Email)))
                //                    )))?.Documents?.Count;

                var numberIdenticalSubscribes = _elasticClient.Search<Subscriber>(s => s
                    .Index("subscriptions")
                    .Query(q => q
                        .Bool(b => b
                            .Must(m => m
                                .MatchPhrase(t => t
                                    .Field(f => f.Id)
                                    .Query(subscriber.Id))))))?.Documents?.Count;

                if (numberIdenticalSubscribes == null || numberIdenticalSubscribes == 0)
                    _elasticClient.Index<Subscriber>(subscriber, i => i
                            .Index("subscriptions"));
            }
        }

        public void Remove(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                _elasticClient.DeleteByQuery<Subscriber>(s => s
                      .Index("subscriptions")
                          .Query(q => q
                              .Bool(b => b
                                  .Must(m => m
                                      .MatchPhrase(t => t
                                          .Field(f => f.Id)
                                          .Query(id))))));
            }
        }
    }
}
