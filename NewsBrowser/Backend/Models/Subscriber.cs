﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Models
{
    public class Subscriber
    {
        public string Id { get; set; }
        public DateTime DateSubscribe { get; set; }
        public string Email { get; set; }
        public string Query { get; set; }
    }
}