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
    public class IntroduceController : Controller
    {
        //
        // GET: /Introduce/
        private smtdbEntities db = new smtdbEntities();
        public ActionResult Index()
        {
            var smt_intro = db.smt_intro.FirstOrDefault();
            return View(smt_intro);
       
        }
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /Category/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(smt_intro smt_intro)
        {
            //    ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            if (ModelState.IsValid)
            {
                int check = db.smt_intro.Where(a => a.intro_id > 0).Select(y => y.intro_id).FirstOrDefault();
                //     db.smt_sub_category.Where(x => x.sub_category_name_vi == smt_sub_category.sub_category_name_vi).Select(y => y.sub_category_name_vi).FirstOrDefault();
                if (check >0 )
                {

                    return View();
                }

                db.smt_intro.Add(smt_intro);
                db.SaveChanges();

                ViewBag.Success = "Tạo mới thành công";
                ModelState.Clear();
                smt_intro = null;
                return View(smt_intro);
            }
            return View(smt_intro);
        }

        public ActionResult Edit(int id = 0)
        {
            smtdbEntities db1 = new smtdbEntities();
            smt_intro smt_intro = db1.smt_intro.Find(id);
            if (smt_intro == null)
            {
                return HttpNotFound();
            }
            //ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            return View(smt_intro);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(smt_intro smt_intro)
        {
            // ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            smtdbEntities db1 = new smtdbEntities();
            if (ModelState.IsValid)
            {
                db1.Entry(smt_intro).State = EntityState.Modified;
                db1.SaveChanges();
                ModelState.Clear();
                ViewBag.Success = "Chỉnh sửa thành công";
                return View(smt_intro);
            }
            ViewBag.Error = "Có lỗi xảy ra! Vui lòng thử lại";
            return View(smt_intro);
        }

    }
}
