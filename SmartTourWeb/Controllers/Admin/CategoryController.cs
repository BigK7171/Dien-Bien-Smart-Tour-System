using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartTourWeb.Models;

namespace SmartTourWeb.Controllers.Admin
{
    [Authorize(Roles = "1")] // admin moi duoc dung trang nay
    public class CategoryController : Controller
    {
        private smtdbEntities db = new smtdbEntities();

        //
        // GET: /Category/

        public ActionResult Index()
        {
            var smt_sub_category = db.smt_sub_category.Where(x=> x.parent_category_id != 3 && x.parent_category_id !=1).Include(s => s.agent).Include(s => s.smt_parent_category).Include(s => s.smt_user).OrderByDescending(y=> y.sub_category_time_created);
            return View(smt_sub_category.ToList());
        }

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id = 0)
        {
            smt_sub_category smt_sub_category = db.smt_sub_category.Find(id);
            ViewBag.user_modified = db.smt_user.Where(x => x.user_id == smt_sub_category.sub_category_user_modified).Select(y => y.user_email).FirstOrDefault();
            if (smt_sub_category == null || smt_sub_category.parent_category_id ==3 || smt_sub_category.parent_category_id==1)
            {
                return HttpNotFound();
            }
            return View(smt_sub_category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi");
            ViewBag.sub_category_user_created = new SelectList(db.smt_user, "user_id", "user_email");
            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(smt_sub_category smt_sub_category)
        {
            ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            if (ModelState.IsValid)
            {
                string check = db.smt_sub_category.Where(x => x.sub_category_name_vi == smt_sub_category.sub_category_name_vi).Select(y => y.sub_category_name_vi).FirstOrDefault();
                if (check != null) 
                { 
                    ViewBag.Error = "Tên đã bị trùng, vui lòng thử lại";
                    
                    ModelState.Clear();
                    smt_sub_category.sub_category_name_vi = null;
                    return View(); 
                }
                smt_sub_category.sub_category_time_created = DateTime.Now;
                smt_sub_category.sub_category_user_created = (int)Session["USER_ID"];
                smt_sub_category.sub_category_agent_id = (int)Session["AGENT"];
                db.smt_sub_category.Add(smt_sub_category);
                db.SaveChanges();

                ViewBag.Success = "Tạo mới thành công";
                ModelState.Clear();
                smt_sub_category = null;
                return View(smt_sub_category);
            }
            return View(smt_sub_category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            smt_sub_category smt_sub_category = db.smt_sub_category.Find(id);
            if (smt_sub_category == null)
            {
                return HttpNotFound();
            }
            ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            return View(smt_sub_category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(smt_sub_category smt_sub_category)
        {
            ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);

            if (ModelState.IsValid)
            {
                smt_sub_category.sub_category_user_modified = (int)Session["USER_ID"] ;
                smt_sub_category.sub_category_time_modified = DateTime.Now; ;
                db.Entry(smt_sub_category).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Success = "Chỉnh sửa thành công";
                return View(smt_sub_category);
            }
            ViewBag.Error = "Có lỗi xảy ra! Vui lòng thử lại";
            return View(smt_sub_category);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            smt_sub_category smt_sub_category = db.smt_sub_category.Find(id);
            ViewBag.user_modified = db.smt_user.Where(x => x.user_id == smt_sub_category.sub_category_user_modified).Select(y => y.user_email).FirstOrDefault();
            if (smt_sub_category == null || smt_sub_category.parent_category_id == 3 || smt_sub_category.parent_category_id == 1)
            {
                return HttpNotFound();
            }
            return View(smt_sub_category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            smt_sub_category smt_sub_category = db.smt_sub_category.Find(id);
            db.smt_sub_category.Remove(smt_sub_category);
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