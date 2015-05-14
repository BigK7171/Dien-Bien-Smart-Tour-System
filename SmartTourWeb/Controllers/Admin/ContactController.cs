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
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        private smtdbEntities db = new smtdbEntities();
        public ActionResult Index()
        {
            var smt_contact = db.smt_contact.FirstOrDefault();
            return View(smt_contact);
        }
        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(smt_contact smt_contact)
        {
            //    ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            if (ModelState.IsValid)
            {
                int check = db.smt_contact.Where(a => a.contact_id > 0).Select(y => y.contact_id).FirstOrDefault();
                //     db.smt_sub_category.Where(x => x.sub_category_name_vi == smt_sub_category.sub_category_name_vi).Select(y => y.sub_category_name_vi).FirstOrDefault();
                if (check > 0)
                {

                    return View();
                }

                db.smt_contact.Add(smt_contact);
                db.SaveChanges();

                ViewBag.Success = "Tạo mới thành công";
                ModelState.Clear();
                smt_contact = null;
                return View(smt_contact);
            }
            return View(smt_contact);
        }
        public ActionResult Edit(int id = 0)
        {
            //smtdbEntities db1 = new smtdbEntities();
            smt_contact smt_contact = db.smt_contact.Find(id);
            if (smt_contact == null)
            {
                return HttpNotFound();
            }
            //ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
            return View(smt_contact);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(smt_contact smt_contact)
        {
            // ViewBag.parent_category_id = new SelectList(db.smt_parent_category.Where(x => x.parent_category_id != 3 && x.parent_category_id != 1), "parent_category_id", "parent_category_name_vi", smt_sub_category.parent_category_id);
        //    smtdbEntities db1 = new smtdbEntities();
            if (ModelState.IsValid)
            {
                db.Entry(smt_contact).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Success = "Chỉnh sửa thành công";
                return View(smt_contact);
            }
            ViewBag.Error = "Có lỗi xảy ra! Vui lòng thử lại";
            return View(smt_contact);
        }
    }
}
