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
                Tags = CreateTags(news?.Entities).Take(5).ToList(),
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
                Tags = CreateTags(news?.Entities).ToList(),
                ImageUrl = news?.Thread?.ImageUrl,
                Language = news?.Language,
                SiteSection = news?.Thread?.SiteSection,
                TitleSection = news?.Thread?.TitleSection,
            };
        }

        private static List<string> CreateTags(Entity entity)
        {
            var tags = new List<string>();
            if (entity == null)
                return tags;

            tags.AddRange(entity.Persons.Select(l => l.Name));
            tags.AddRange(entity.Organizations.Select(l => l.Name));
            tags.AddRange(entity.Locations.Select(l => l.Name));

            return tags;
        }
    }
}
