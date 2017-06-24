using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kferretti_portfolio2.Models
{
    public class TopicHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void AddTopicToBlog(int topicId, int blogId)
        {
            Topic topic = db.Topics.Find(topicId);
            BlogPost blog = db.BlogPosts.Find(blogId);
            blog.Topics.Add(topic);
            db.SaveChanges();
        }
        public void RemoveTopicFromBlog(int topicId, int blogId)
        {
            Topic topic = db.Topics.Find(topicId);
            BlogPost blog = db.BlogPosts.Find(blogId);
            blog.Topics.Remove(topic);
            db.SaveChanges();
        }
    }
}