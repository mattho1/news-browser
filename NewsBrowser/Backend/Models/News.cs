using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Models
{
    public class News
    {
        [Text(Name = "uuid")]
        public string Id { get; set; }
        public Thread Thread { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public Entity Entities { get; set; }
        public string Language { get; set; }
        public string Text { get; set; }
        public News(){ }

        public News(string title, string author, string site, string language, string imgUrl, string contents)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Author = author;
            Url = site;
            Language = language;
            Text = contents;
            Entities = new Entity();
            Thread = new Thread(site, title, imgUrl);
        }
    }
}
