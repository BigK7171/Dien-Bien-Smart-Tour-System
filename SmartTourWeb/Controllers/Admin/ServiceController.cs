using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.IO;
using SmartTourWeb.Models;
using PagedList;

namespace SmartTourWeb.Controllers.Admin
{
    [Authorize(Roles = "")]  // dang nhap moi dung dc trang
    public class ServiceController : Controller
    {
        private Models.smtdbEntities db = new Models.smtdbEntities();

        //
        // GET: /Service/

        //MenudropdownAdmin

        public ActionResult MenudropdownAdmin()
        {
            int user_id = (int)Session["USER_ID"];
            int agent_id = (int)Session["AGENT"];
            List<int> lstCate = new List<int>();
            lstCate = db.agent_category_permission.Where(a => a.agent_id == agent_id && a.permission ==1).Select( r => r.category_id).ToList();
            
            //var lstParent = new List<int>();

            var lstParent = db.smt_sub_category.Where(a => lstCate.Contains(a.sub_category_id)).Select(r => r.parent_category_id).ToList();

            //SelectList list_cate = db.agent_category_permission.Select(x => x.smt_sub_category).ToList();
            ViewData["parent"] = db.smt_parent_category.Where(a => lstParent.Contains(a.parent_category_id)).ToList();
            ViewData["sub"] = db.smt_sub_category.Where(a => lstCate.Contains(a.sub_category_id)).ToList();
            return PartialView("MenudropdownAdmin");
        }
        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Index(int category = 1, string sortOrder = null, string searchString = null, int? page = 1)
        {

            ViewBag.id = category;
            int user_id = (int)Session["USER_ID"];
            int agent_id = (int)Session["AGENT"];

            var smt_services = db.smt_location.Where(a => a.sub_category_id == category);
            if (getPermission(category)) // kiem tra quyen
            {
                if (agent_id == 1) // phan quyen xem Location trang index
                {
                    smt_services = db.smt_location.Where(a => a.sub_category_id == category);
                }
                else if (agent_id == 3)
                {
                    smt_services = db.smt_location.Where(a => (a.sub_category_id == category) && (a.location_user_created == user_id));
                }
                else
                {
                    smt_services = db.smt_location.Where(a => ((a.sub_category_id == category) && (a.agent_id == agent_id)));
                }

                //  search sap xep theo ten, ngay tao
                ViewBag.CurrentSort = sortOrder;
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;                
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                if (!String.IsNullOrEmpty(searchString))
                {
                    smt_services = smt_services.Where(s => s.location_name_vi.ToUpper().Contains(searchString.ToUpper()) && s.sub_category_id == category);
                    //       || s.location_name_en.ToUpper().Contains(searchString.ToUpper())); Tìm theo tên tiếng anh thì thêm vào
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        smt_services = smt_services.OrderByDescending(s => s.location_name_vi);
                        break;
                    case "Date":
                        smt_services = smt_services.OrderBy(s => s.location_time_created);
                        break;
                    case "date_desc":
                        smt_services = smt_services.OrderByDescending(s => s.location_time_created);
                        break;
                    default:
                        smt_services = smt_services.OrderBy(s => s.location_name_vi);
                        break;
                }

                ViewBag.category = category;
                ViewBag.namecategory = db.smt_sub_category.Where(a => a.sub_category_id == category).FirstOrDefault().sub_category_name_vi;
                ViewBag.user_name = (string)(Session["USER_NAME"]);

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(smt_services.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                Response.Write("<script> alert('Bạn không có quyền truy cập vào trang này') </script>");
                return RedirectToAction("Welcome");

            }
        }



        //
        // GET: /Service/Details/5

        public ActionResult Details(int id = 0)
        {

            smt_location smt_services = db.smt_location.Where(a => a.location_id == id).Include(u => u.smt_comment).FirstOrDefault();
            ViewBag.user_modified = db.smt_user.Where(x => x.user_id == smt_services.location_user_modified).Select(y => y.user_email).FirstOrDefault();
            //int sub_category_id = (smt_services.sub_category_id != 0) ? smt_services.sub_category_id : 0;
            // if (getPermission(category)) // kiem tra quyen

            if (smt_services.Equals(null) || (!getPermission(smt_services.sub_category_id))) // lay quyen user
            {
                return HttpNotFound();
            }
            return View(smt_services);
        }
        public bool getPermission(int category_id)
        {
            int user_id = (int)Session["USER_ID"];
            bool permission = false;
            int per = 0;
            try
            {
                int agent_id = db.smt_user.Where(x => x.user_id == user_id).Select(y => y.agent_id).FirstOrDefault();
                per = db.agent_category_permission.Where(x => x.agent_id == agent_id && x.category_id == category_id).Select(y => y.permission).FirstOrDefault();
            }
            catch (Exception e) { }

            permission = (per == 1) ? true : false;
            if ((int)Session["AGENT"] == 1) { permission = true; } // nếu là admin thi quyền bằng true
            return permission;
        }

        //
        // GET: /Service/Create

        public ActionResult Create(int category)
        {
            ViewBag.category_name = db.smt_sub_category.Where(x => x.sub_category_id == category).Select(x => x.sub_category_name_vi).FirstOrDefault();
            string category_name_x = db.smt_sub_category.Where(x => x.sub_category_id == category).Select(x => x.sub_category_name_vi).FirstOrDefault();
            // chi view cac category người dùng có thể chọn
            //var listcategory = db.smt_sub_category.Where(x => x.parent_category_id != 3 && (x.agent_category_permission.Where(y => y.category_id == x.sub_category_id && y.agent_id == Convert.ToInt32(Session["AGENT"])).Select(z => z.permission).FirstOrDefault() == 1));
            ViewBag.sub_category_id = new SelectList(db.smt_sub_category.Where(x => x.parent_category_id != 3), "sub_category_id", "sub_category_name_vi", category);
            ViewBag.parent_location = new SelectList(db.smt_location.Where(x => x.location_shared == true), "location_id", "location_name_vi",0);
            return View();
        }

        //
        // POST: /Service/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(smt_location smt_services, FormCollection c) //  ham creat da kiem tra usar ngoai trang view
        {
            ViewBag.parent_location = new SelectList(db.smt_location.Where(x => x.location_shared == true), "location_id", "location_name_vi");
            if (ModelState.IsValid && (getPermission(smt_services.sub_category_id)))
            {
                string check = db.smt_location.Where(x => x.location_name_vi == smt_services.location_name_vi).Select(y => y.location_name_vi).FirstOrDefault();
                
                // kiem tra trung ten
                if (check != null)
                {
                    ViewBag.Error = "Trùng tên hoặc bài đã đăng rồi, vui lòng sửa lại";
                    ViewBag.sub_category_id = new SelectList(db.smt_sub_category.Where(x => x.parent_category_id != 3), "sub_category_id", "sub_category_name_vi", smt_services.sub_category_id);

                    ModelState.Clear();
                    smt_services = null;
                    return View(smt_services);
                }
                    if (Session["AGENT"].ToString() == "3")  // neu Guess tao moi dia điểm thì share = false
                    { smt_services.location_shared = false; }
                    else
                    {
                        smt_services.location_shared = (Convert.ToInt32(c["locationshared"]) == 1) ? true : false;
                    }
                smt_services.location_time_created = DateTime.Now;
                int user_id = (int)Session["USER_ID"];
                smt_services.location_user_created = user_id;
                smt_services.agent_id = db.smt_user.Where(x => x.user_id == user_id).Select(x => x.agent_id).Single();
                if (smt_services.location_parent_id == null) { smt_services.location_parent_id = 0; }
                db.smt_location.Add(smt_services);
                db.SaveChanges();

                ViewBag.Success = "Tạo mới thành công";
                ViewBag.sub_category_id = new SelectList(db.smt_sub_category.Where(x => x.parent_category_id != 3), "sub_category_id", "sub_category_name_vi", smt_services.sub_category_id);
                ModelState.Clear();
                smt_services = null;
                return View(smt_services);

            }
            ViewBag.Error = "Đã xảy ra lỗi, vui lòng thử lại";

            ViewBag.sub_category_id = new SelectList(db.smt_sub_category.Where(x => x.parent_category_id != 3), "sub_category_id", "sub_category_name_vi", smt_services.sub_category_id);
            return View(smt_services);
        }

        //
        // GET: /Service/Edit/5

        public ActionResult Edit(int id = 0)
        {
            int _id = id;
            int user_id = (int)Session["USER_ID"];
            int agent_id = db.smt_user.Where(x => x.user_id == user_id).Select(x => x.agent_id).Single();
            smt_location smt_services = db.smt_location.Where(a => a.location_id == id).SingleOrDefault();

            if (smt_services == null)
            {
                return HttpNotFound();
            }
            ViewBag.SERVICE_CATEGORY_ID = new SelectList(db.smt_sub_category.Where(x => x.parent_category_id != 3), "sub_category_id", "sub_category_name_vi", smt_services.sub_category_id);
            ViewBag.parent_location = new SelectList(db.smt_location.Where(x => x.location_shared == true), "location_id", "location_name_vi",smt_services.location_parent_id);
            ViewBag.layer_location = new SelectList(db.smt_layer, "layer_id", "layer_des_vi", smt_services.location_layer_id);
            return View(smt_services);
        }

        //
        // POST: /Service/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(smt_location smt_services, FormCollection c)
        {
            if (ModelState.IsValid && (getPermission(smt_services.sub_category_id)))
            {
                smt_services.location_shared = (Convert.ToInt32(c["locationshared"]) == 1) ? true : false;

                smt_services.location_time_modified = DateTime.Now;
                smt_services.location_user_modified = (int)Session["USER_ID"];
                if (smt_services.location_parent_id == null) { smt_services.location_parent_id = 0; }
            //    if (smt_services.location_layer_id == null) { smt_services.location_layer_id = 0; }
                db.Entry(smt_services).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Success = "Chỉnh sửa thành công";

            }
            ViewBag.SERVICE_CATEGORY_ID = new SelectList(db.smt_sub_category.Where(x => x.parent_category_id != 3), "sub_category_id", "sub_category_name_vi", smt_services.sub_category_id);
            ViewBag.parent_location = new SelectList(db.smt_location.Where(x => x.location_shared == true), "location_id", "location_name_vi", smt_services.location_parent_id);
            ViewBag.layer_location = new SelectList(db.smt_layer, "layer_id", "layer_des_vi", smt_services.location_layer_id);
            return View(smt_services);
        }

        //
        // GET: /Service/Delete/5
        public ActionResult Delete(int id = 0)
        {
            smt_location smt_services = db.smt_location.Find(id);
            int user_id = (int)Session["USER_ID"];
            int agent_id = db.smt_user.Where(x => x.user_id == user_id).Select(x => x.agent_id).Single();
            ViewBag.user_modified = db.smt_user.Where(x => x.user_id == smt_services.location_user_modified).Select(y => y.user_email).FirstOrDefault();
            bool check = agent_id == 1 || (agent_id == 3 && user_id == smt_services.location_user_created) || (agent_id != 3 && agent_id == smt_services.agent_id);
            if (check)  // kiem tra quyen user
            {
                if (smt_services == null)
                {
                    return HttpNotFound();
                }
                return View(smt_services);
            }
            else
            {
                return HttpNotFound();
            }

        }

        //
        // POST: /Service/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult DeleteConfirmed(int id)
        {
            smtdbEntities db2 = new smtdbEntities();
            smt_location smt_services = db2.smt_location.Find(id);
            db2.smt_location.Remove(smt_services);
            db2.SaveChanges();
            db2.Dispose();
            ViewBag.Success = "Xóa bài đăng thành công";
            return RedirectToAction("Index", new { category = smt_services.sub_category_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //public ActionResult ListBooking(int id =0) 
        //{
        //    var service_room_bookings = db.SERVICE_ROOM_BOOKINGS.Include(s => s.SERVICE_ROOMS).Include(s => s.smt_userS);  
        //    return View(service_room_bookings.ToList());
        //}


        //public ActionResult BookingDelete(int id = 0)
        //{
        //    SERVICE_ROOM_BOOKINGS service_room_bookings = db.SERVICE_ROOM_BOOKINGS.Find(id);
        //    if (service_room_bookings == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(service_room_bookings);
        //}
        ////
        //// POST: /test/Delete/5

        //[HttpPost, ActionName("DeleteBooking")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmedBooking(int id)
        //{
        //    SERVICE_ROOM_BOOKINGS service_room_bookings = db.SERVICE_ROOM_BOOKINGS.Find(id);
        //    db.SERVICE_ROOM_BOOKINGS.Remove(service_room_bookings);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        public string currentFilter { get; set; }
    }

}