using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Helpers
{
    public class DTOMapper
    {
        public static SimpleNews GetSimpleNews(News news)
        {
            return new SimpleNews()
            {
                Id = news?.Id,
                Url = string.IsNullOrEmpty(news?.Url) ? news?.Thread?.Url : news.Url,
                Author = news?.Author,
                Title = string.IsNullOrEmpty(news?.Title) ? news?.Thread?.FullTitle : news.Title,
                Text = news?.Text?.Length > 500 ? news?.Text?.Substring(0, 500) + "..." : news?.Text ?? string.Empty,
                Tags = CreateTags(news).Take(5).ToList(),
            };
        }

        public static NewsDetails GetNewsDetails(News news)
        {
            return new NewsDetails()
            {
                Id = news?.Id,
                Url = string.IsNullOrEmpty(news?.Url) ? news?.Thread?.Url : news.Url,
                Author = news?.Author,
                Title = string.IsNullOrEmpty(news?.Title) ? news?.Thread?.FullTitle : news.Title,
                Text = news?.Text ?? string.Empty,
                Tags = CreateTags(news).ToList(),
                ImageUrl = news?.Thread?.ImageUrl,
                Language = news?.Language,
                SiteSection = news?.Thread?.SiteSection,
                TitleSection = news?.Thread?.TitleSection,
            };
        }

        private static List<DTOs.Tag> CreateTags(News news)
        {
            var tags = new List<DTOs.Tag>();
            if (news == null || news.Entities == null)
                return tags;

            if(!string.IsNullOrEmpty(news?.Thread?.SiteSection))
                tags.Add(new DTOs.Tag() { Name = news.Thread.SiteSection, Type = 1 });
            
            tags.AddRange(news.Entities.Persons.Select(l => new DTOs.Tag() { Name = l.Name, Type = 2 }));
            tags.AddRange(news.Entities.Organizations.Select(l => new DTOs.Tag() { Name = l.Name, Type = 3 }));
            tags.AddRange(news.Entities.Locations.Select(l => new DTOs.Tag() { Name = l.Name, Type = 4 }));

            return tags;
        }
    }
}
