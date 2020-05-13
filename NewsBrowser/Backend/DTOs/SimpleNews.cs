using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.DTOs
{
    public class SimpleNews
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public List<string> Tags { get; set; } 
    }
}
