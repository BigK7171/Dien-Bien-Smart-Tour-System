using System;
using System.Collections.Generic;
using System.Linq;
//using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
//using WebMatrix.WebData;
using System.Security.Cryptography;
using System.Text;
using SmartTourWeb.Models;
using AttributeRouting.Web.Mvc;
using AttributeRouting;

namespace SmartTourWeb.Controllers
{
    public class UserController : Controller
    {
        private Models.smtdbEntities db = new Models.smtdbEntities();
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "")] 
        [HttpGet]
        public ActionResult CreateComment(int ITEM_ID)
        {
            ViewBag.ITEM_ID = ITEM_ID;
            return PartialView("CreateCommentPartial");
            

        }
        [HttpPost]
        public ActionResult CreateComment([Bind(Exclude = "CreateTime")] smt_comment comment)
        {

            comment.user_id = (int)Session["USER_ID"];
            comment.comment_time = DateTime.Now;
            db.smt_comment.Add(comment);
            db.SaveChanges();


            Models.smtdbEntities db2 = new Models.smtdbEntities();
            var cm = db2.smt_comment.Where(x => x.location_id == comment.location_id).ToList();
            return PartialView("CommentPartial", cm);
        }
        public ActionResult DeleteComment(int id)
        {
            var comment = db.smt_comment.Find(id);
            db.smt_comment.Remove(comment);
            db.SaveChanges();
            var cm = db.smt_comment.Where(x => x.location_id == comment.location_id).ToList();
            return PartialView("CommentPartial", cm);
        }

    }
}
