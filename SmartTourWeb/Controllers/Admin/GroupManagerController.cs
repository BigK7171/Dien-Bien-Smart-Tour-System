using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartTourWeb.Models;

namespace SmartTourWeb.Controllers.Admin
{
    [Authorize(Roles = "1")] // admin moi duoc dung trang nay
    public class GroupManagerController : Controller
    {
        private smtdbEntities db = new smtdbEntities();

        //
        // GET: /GroupManager/

        public ActionResult Index()
        {
            return View(db.groups.ToList());
        }

        //
        // GET: /GroupManager/Details/5

        public ActionResult Details(int id = 0)
        {
            group group = db.groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // GET: /GroupManager/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /GroupManager/Create

        [HttpPost]
        public ActionResult Create(group group)
        {
            if (ModelState.IsValid)
            {
                db.groups.Add(group);
                db.SaveChanges();
                string groupname = group.group_name.ToString();
                var folder = Server.MapPath("~/Uploads/" +groupname);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                return RedirectToAction("Index");
            }

            return View(group);
        }

        //
        // GET: /GroupManager/Edit/5

        public ActionResult Edit(int id = 0)
        {
            group group = db.groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /GroupManager/Edit/5

        [HttpPost]
        public ActionResult Edit(group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        //
        // GET: /GroupManager/Delete/5

        public ActionResult Delete(int id = 0)
        {
            group group = db.groups.Find(id);
            if (group == null || id==1 ||id==2 || id==3)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /GroupManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            group group = db.groups.Find(id);
            db.groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}