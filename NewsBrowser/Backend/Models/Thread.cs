using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Thread
    {
        public Social Social { get; set; }

        [Text(Name = "site_full")]
        public string SiteFullName { get; set; }

        [Text(Name = "main_image")]
        public string ImageUrl { get; set; }

        [Text(Name = "site_section")]
        public string SiteSection { get; set; }

        [Text(Name = "section_title")]
        public string TitleSection { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// Only value "US"
        /// </summary>
        public string Country { get; set; }

        public string Site { get; set; }

        [Text(Name = "title_full")]
        public string FullTitle { get; set; }

        [Text(Name = "spam_score")]
        public double SpamScore { get; set; }

        /// <summary>
        /// Only value "News"
        /// </summary>
        [Text(Name = "site_type")]
        public string SiteType { get; set; }

        public Thread() { }

        public Thread(string site, string title, string imgUrl)
        {
            Site = site.Split('.')[0];
            FullTitle = title;
            SiteFullName = site;
            SiteType = "News";
            Country = "US";
            Social = new Social();
            SiteSection = site.Split('.')[0];
            TitleSection = title;
            ImageUrl = imgUrl;
            Url = site;
            SpamScore = 0;
        }
    }
}
