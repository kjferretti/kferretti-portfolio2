using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kferretti_portfolio2.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            Comments = new HashSet<Comment>();
            Topics = new HashSet<Topic>();
        }
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTimeOffset Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTimeOffset? Updated { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string ImageURL { get; set; }
        public string slug { get; set; }
        public bool Published { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}