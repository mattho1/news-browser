using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsBrowser.DTOs
{
    public class NewNewsDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Site { get; set; }
        public string Language { get; set; }
        public string ImgUrl { get; set; }
        public string Contents { get; set; }
    }
}
