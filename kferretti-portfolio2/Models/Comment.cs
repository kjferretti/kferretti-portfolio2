using System;

namespace kferretti_portfolio2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string TimeSincePosted { get; set; }
        public string UpdateReason { get; set; }

        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public int BlogPostId { get; set; }
        public virtual BlogPost BlogPost {get; set;}
    }
}