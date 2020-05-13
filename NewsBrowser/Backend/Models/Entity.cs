using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Models
{
    public class Entity
    {
        public List<Tag> Persons { get; set; }
        public List<Tag> Locations { get; set; }
        public List<Tag> Organizations { get; set; }
    }
}
