using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Models
{
    public class Social
    {
        public ReactionForm Gplus { get; set; }
        public ReactionForm Pinterest { get; set; }
        public ReactionForm Vk { get; set; }
        public ReactionForm Linkedin { get; set; }
        public ReactionForm Facebook { get; set; }
        public ReactionForm Stumbledupon { get; set; }

        public Social()
        {
            Gplus = new ReactionForm();
            Pinterest = new ReactionForm();
            Vk = new ReactionForm();
            Linkedin = new ReactionForm();
            Facebook = new ReactionForm();
            Stumbledupon = new ReactionForm();
        }
    }
}
