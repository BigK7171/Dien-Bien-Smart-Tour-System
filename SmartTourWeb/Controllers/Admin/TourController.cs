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
    public class TourController : Controller
    {
        //
        // GET: /Tour/
        private smtdbEntities db = new smtdbEntities();
        public ActionResult Index()
        {
            var smt_link_tour = db.smt_link_tour.ToList();
            return View(smt_link_tour);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(smt_link_tour smt_link_tour)
        {
            //    ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            if (ModelState.IsValid)
            {
                string check = db.smt_link_tour.Where(a => a.tour_name == smt_link_tour.tour_name).Select(y => y.tour_link).FirstOrDefault();
                //     db.smt_sub_category.Where(x => x.sub_category_name_vi == smt_sub_category.sub_category_name_vi).Select(y => y.sub_category_name_vi).FirstOrDefault();
                if (check != null)
                {
                    ViewBag.Error = "Tên đã bị trùng, vui lòng thử lại";

                    ModelState.Clear();
                    smt_link_tour.tour_name = null;
                    return View();
                }

                db.smt_link_tour.Add(smt_link_tour);
                db.SaveChanges();

                ViewBag.Success = "Tạo mới thành công";
                ModelState.Clear();
                smt_link_tour = null;
                return View(smt_link_tour);
            }
            return View(smt_link_tour);
        }

        public ActionResult Edit(int id = 0)
        {

            smt_link_tour smt_linktour = db.smt_link_tour.Find(id);
            if (smt_linktour == null)
            {
                return HttpNotFound();
            }
            //ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            return View(smt_linktour);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(smt_link_tour smt_link_tour)
        {
            // ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
          //  smtdbEntities db1 = new smtdbEntities();
            if (ModelState.IsValid)
            {
                db.Entry(smt_link_tour).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Success = "Chỉnh sửa thành công";
                return View(smt_link_tour);
            }
            ViewBag.Error = "Có lỗi xảy ra! Vui lòng thử lại";
            return View(smt_link_tour);
        }
        public ActionResult Delete(int id = 0)
        {
            smt_link_tour smt_link_tour = db.smt_link_tour.Find(id);
            ///      ViewBag.user_modified = db.smt_user.Where(x => x.user_id == smt_sub_category.sub_category_user_modified).Select(y => y.user_email).FirstOrDefault();

            return View(smt_link_tour);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            smt_link_tour smt_link_tour = db.smt_link_tour.Find(id);
            db.smt_link_tour.Remove(smt_link_tour);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
