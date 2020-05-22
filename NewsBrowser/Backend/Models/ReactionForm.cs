using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class ReactionForm
    {
        [Text(Name = "likes")]
        public int LikesNumber { get; set; }

        [Text(Name = "shares")]
        public int SharesNumber { get; set; }

        [Text(Name = "comments")]
        public int CommentsNumber { get; set; }

        public ReactionForm()
        {
            SharesNumber = 0;
            CommentsNumber = 0;
            LikesNumber = 0;
        }
    }
}
