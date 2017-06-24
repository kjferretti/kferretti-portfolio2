using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kferretti_portfolio2.Models
{
    public class EmailViewModel
    {
        [Required]
        public string FromName { get; set; }
        [Required, EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}