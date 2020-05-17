using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Models
{
    public class EmailMessage
    {
        public string Receiver { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
