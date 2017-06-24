using System.Collections.Generic;

namespace kferretti_portfolio2.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<BlogPost> BlogPosts { get; set; }
    }
}