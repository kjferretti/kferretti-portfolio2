using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kferretti_portfolio2.Models
{
    public class BlogTopicsViewModel
    {
        public BlogPost Post { get; set; }
        public string[] Topics { get; set; }
    }
}