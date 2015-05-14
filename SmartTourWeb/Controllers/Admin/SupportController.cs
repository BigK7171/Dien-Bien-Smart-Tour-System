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
    public class SupportController : Controller
    {
        //
        // GET: /Support/
        private smtdbEntities db = new smtdbEntities();
        public ActionResult Index()
        {

            var smt_support_online = db.smt_support_online.ToList();
            return View(smt_support_online);
        }

        //
        // GET: /Support/Create

        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(smt_support_online smt_support_online)
        {
        //    ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            if (ModelState.IsValid)
            {
                string check = db.smt_support_online.Where(a => a.support_nick == smt_support_online.support_nick).Select(y => y.support_nick).FirstOrDefault();
               //     db.smt_sub_category.Where(x => x.sub_category_name_vi == smt_sub_category.sub_category_name_vi).Select(y => y.sub_category_name_vi).FirstOrDefault();
                if (check != null)
                {
                    ViewBag.Error = "Tên đã bị trùng, vui lòng thử lại";

                    ModelState.Clear();
                    smt_support_online.support_name = null;
                    return View();
                }

                db.smt_support_online.Add(smt_support_online);
                db.SaveChanges();

                ViewBag.Success = "Tạo mới thành công";
                ModelState.Clear();
                smt_support_online = null;
                return View(smt_support_online);
            }
            return View(smt_support_online);
        }


        //
        // GET: /Support/Edit/5

        public ActionResult Edit(int id = 0)
        {
            smtdbEntities db1 = new smtdbEntities();
            smt_support_online smt_support_online = db1.smt_support_online.Find(id);
            if (smt_support_online == null)
            {
                return HttpNotFound();
            }
            //ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            return View(smt_support_online);
        }

        [HttpPost]
        public ActionResult Edit(smt_support_online smt_support_online)
        {
           // ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            smtdbEntities db1 = new smtdbEntities();
            if (ModelState.IsValid)
            {
                db1.Entry(smt_support_online).State = EntityState.Modified;
                db1.SaveChanges();
                ModelState.Clear();
                ViewBag.Success = "Chỉnh sửa thành công";
                return View(smt_support_online);
            }
            ViewBag.Error = "Có lỗi xảy ra! Vui lòng thử lại";
            return View(smt_support_online);
        }

        public ActionResult Delete(int id = 0)
        {
            smt_support_online smt_sub_category = db.smt_support_online.Find(id);
      ///      ViewBag.user_modified = db.smt_user.Where(x => x.user_id == smt_sub_category.sub_category_user_modified).Select(y => y.user_email).FirstOrDefault();

            return View(smt_sub_category);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            smt_support_online smt_support_online = db.smt_support_online.Find(id);
            db.smt_support_online.Remove(smt_support_online);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
