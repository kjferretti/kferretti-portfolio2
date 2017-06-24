using kferretti_portfolio2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace kferretti_portfolio2.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var from = model.FromEmail;
                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        //Body = model.Body,
                        Body = $"<strong>{model.FromName}</strong> left this message: {model.Body}.<br/><br/>The user's eail address is <strong>{model.FromEmail}</strong>",
                        IsBodyHtml = true
                    };
                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    ViewBag.Message = "Email has been sent";
                    return View();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }

        public ActionResult Portfolio()
        {
            return View();
        }

        public ActionResult Resume()
        {
            return View();
        }


        public ActionResult PortfolioCS()
        {
            return View();
        }
    }
}