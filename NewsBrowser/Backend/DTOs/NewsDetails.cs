using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.DTOs
{
    public class NewsDetails
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public List<string> Tags { get; set; }
        public string ImageUrl { get; set; }
        public string Language { get; set; }
        public string SiteSection { get; set; }
        public string TitleSection { get; set; }
    }
}
