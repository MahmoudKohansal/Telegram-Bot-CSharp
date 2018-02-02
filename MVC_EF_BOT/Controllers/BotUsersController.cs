using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_EF_BOT.DAL;
using MVC_EF_BOT.Models;

namespace MVC_EF_BOT.Controllers
{
    public class BotUsersController : Controller
    {
        private BotContext db = new BotContext();

        // GET: BotUsers
        public ActionResult Index()
        {
            return View();
        }

        public bool Register([Bind(Include = "teleID,getNews")] BotUser botUser)
        {

            if (db.BotUsers.Any(u => u.teleID == botUser.teleID))
            {
                return false;
            }
            else
            {
                if (ModelState.IsValid)
                {

                    db.BotUsers.Add(botUser);
                    db.SaveChanges();
                    return true;
                }
            }

            return false;
        }
        //RegisterAdv
        public bool RegisterAdv([Bind(Include = "teleID,getNews")] BotUser botUser)
        {

            var bu = db.BotUsers.SingleOrDefault(u => u.teleID == botUser.teleID);
            if (bu != null)
            {
                bu.getNews = botUser.getNews;
                db.SaveChanges();
            }

            return false;
        }
        public List<BotUser> SelectUsersForAdv()
        {
            List<BotUser> bUsrsList = db.BotUsers.Where(u => u.getNews == true).ToList();
            return bUsrsList;
            
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        internal void StoreJoke(BotJoke botJoke)
        {
            if (ModelState.IsValid)
            {
                db.BotJokes.Add(botJoke);
            }
            return;
        }

        internal List<BotJoke> SelectAllJokes()
        {
            return (db.BotJokes.ToList());
        }
    }
}
