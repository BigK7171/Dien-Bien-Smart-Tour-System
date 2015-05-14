using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SmartTourWeb.Models;
using System.Web.Security;
//using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
//using WebMatrix.WebData;
using System.Security.Cryptography;
using System.Text;
using PagedList;
using AttributeRouting.Web.Mvc;
using AttributeRouting;
using SmartTourWeb.Controllers.Home;
using SmartTourWeb.Helper;
using System.Data;
using System.Web.Services;

namespace SmartTourWeb.Controllers
{
    public class ListCategory {
        public string ParentCategory { get; set; }
        public string ChildCategory { get; set; }
    }
    //[RoutePrefix("Trang-chu")]    
    public class HomeController : BaseController
    {
        private Models.smtdbEntities db = new Models.smtdbEntities();

        [GET("Home")]
        public ActionResult Index(string stringSearch = null)
        {
            ViewData["parent"] = db.smt_parent_category.Where(x=> x.parent_category_id != 1 && x.parent_category_id != 3).ToList();   // khac quan ly nguoi dung va tour
            ViewData["sub"] = db.smt_sub_category.Where(x=> x.sub_category_id!=12 && x.sub_category_id !=13).OrderBy(a => a.sub_category_id).ToList();                                          // category khac Tour
            var smt_services = db.smt_location.Where(a=> (a.location_shared==true) &&  a.location_name_en !=null && a.location_name_vi !=null && a.agent_id ==1 && a.sub_category_id !=12).ToList();
            if (stringSearch != null)
            {
                if ((int)Session["CurrentCulture"] == 1) //English
                { smt_services = db.smt_location.Where(a => (a.location_name_en.ToLower().Contains(stringSearch.ToLower())) && (a.location_shared == true) &&  a.location_name_en !=null && a.location_name_vi !=null).ToList(); }
                    else // Vietnamese
                {smt_services = db.smt_location.Where(a=> (a.location_name_vi.ToLower().Contains(stringSearch.ToLower())) && (a.location_shared == true) &&  a.location_name_en !=null && a.location_name_vi !=null).ToList();}
              
            }
                return View(smt_services);
        }

        public ActionResult Menudropdown()
        {
            ViewData["parent"] = db.smt_parent_category.ToList();
            ViewData["sub"] = db.smt_sub_category.ToList();
            return PartialView("Menudropdown");
        }
        public ActionResult Slideshow()
        {
            return PartialView("Slideshow");
        }

        public ActionResult MostPopularPosts()
        {
            var mostPopularposts = (from post in db.smt_location orderby post.location_counter descending select post).Take(4).ToList();
            ViewData["popularposts"] = mostPopularposts ;
            return PartialView("MostPopularPosts");       
        }

        public ActionResult ListSupporters()
        {
            var listSupporters = (from supporter in db.smt_support_online select supporter).ToList();
            ViewData["supporter"] = listSupporters;
            return PartialView("ListSupporters");
        }
        [GET("List-{category_name}-{category}")]
        public ActionResult List(int category = 0, int? page = 1)
        {
            ViewBag.id = category;
            var smt_services = db.smt_location.Where(a => (a.sub_category_id == category && a.sub_category_id!=13) && a.location_shared==true).ToList();
            //|| )
            ViewBag.Category = db.smt_sub_category.Where(a=> a.sub_category_id == category).Select(y=> y.sub_category_name_vi).FirstOrDefault();
            ViewBag.Category_en = db.smt_sub_category.Where(a => a.sub_category_id == category).Select(y => y.sub_category_name_en).FirstOrDefault();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(smt_services.ToPagedList(pageNumber, pageSize));
        }
        /*
        [GET("List-tour")]
        public ActionResult TourIndex(int? page = 1) // List Tour trang home
        {   
            var smt_tour = db.smt_tours.Where(x=> x.tour_status==true).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(smt_tour.ToPagedList(pageNumber, pageSize));
        }
        [GET("Tour-detail-{id}")]
        public ActionResult TourDetail(int id=0)
        {
            var smt_tour = db.smt_tours.Find(id);   
            if (smt_tour.tour_status != true) { return HttpNotFound(); } // ko show cac tour ko hoat dong
            ViewData["tour_item"] = db.smt_tour_items.Where(x => x.tour_id == id ).ToList();
            return View(smt_tour);
        }
         */
        [GET("Detail-{id}/{name_lacation}")] //name_location not null
        public ActionResult Details(int id = 0)
        {            
            Models.smt_location smt_services = db.smt_location.Where(m => m.location_id == id).SingleOrDefault();
      //      ViewData["tour_item"] = db.smt_tour_items.Where(x => x.location_id == id && x.smt_tours.tour_status==true).ToList();
            if (smt_services.location_counter == null)
                smt_services.location_counter = 0;
            smt_services.location_counter += 1;
            db.Entry(smt_services).State = EntityState.Modified;
            db.SaveChanges(); 
            ViewData["sub_location"] = db.smt_location.Where(a => a.location_parent_id == id && a.location_shared==true).ToList();     //


            if (smt_services == null)
            {
                return HttpNotFound();
            }
            return View(smt_services);
        }
        
        // Search tieng Viet
        public ActionResult Search(Models.smt_location smt_location)
        {
            ViewBag.listSearch = new SelectList(db.smt_location.Where(a => a.location_shared == true), "ID", "NAME_VI", smt_location.location_name_vi);
            return PartialView("Search");

        }
        [HttpPost]
        public ActionResult Search(string term)
        {
            var result = (from r in db.smt_location
                          where r.location_name_vi.ToLower().Contains(term.ToLower()) && r.location_shared==true
                          select new { r.location_name_vi, r.location_id}).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Search nang cao tieng Viet
        
        //public ActionResult AdvancedSearch()
        //{
        //    var category = (from list in db.smt_sub_category select list).ToList();
        //    ViewData["listCategory"] = category;
        //    return PartialView("AdvancedSearch");
        //}

        public ActionResult AdvancedSearch(int category = 0)
        {
            ViewBag.sub_category_id = new SelectList(db.smt_sub_category.Where(x => x.sub_category_id != 13), "sub_category_id", "sub_category_name_vi");
            ViewBag.sub_category_id_en = new SelectList(db.smt_sub_category.Where(x => x.sub_category_id != 13), "sub_category_id", "sub_category_name_en");   
            ViewData["locate"] = db.smt_location.ToList();
            //ViewBag.Category_vi = db.smt_sub_category.Where(a=> a.sub_category_id == category).Select(y=> y.sub_category_name_vi).FirstOrDefault();
            //ViewBag.Category_en = db.smt_sub_category.Where(a => a.sub_category_id == category).Select(y => y.sub_category_name_en).FirstOrDefault();
            return PartialView("AdvancedSearch");
        }
        //[HttpPost]
        //public ActionResult AdvancedSearch(int category = 0, int? page = 1)
        //{
        //    var result = (from location in db.smt_location join sub in db.smt_sub_category on location.sub_category_id equals sub.sub_category_id
        //                  select location).Distinct();
            
        //}
        //Search tieng Anh
        public ActionResult Search_en(Models.smt_location smt_location)
        {
            ViewBag.listSearch = new SelectList(db.smt_location.Where(a => a.location_shared == true), "ID", "NAME_EN", smt_location.location_name_en);
            return PartialView("Search_en");

        }
        [HttpPost]
        public ActionResult Search_en(string term)
        {
            var result = (from r in db.smt_location
                          where r.location_name_en.ToLower().Contains(term.ToLower()) && r.location_shared == true
                          select new { r.location_name_en, r.location_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Advanced Search Eng
        public ActionResult AdvancedSearch_en()
        {
            ViewBag.sub_category_id = new SelectList(db.smt_sub_category.Where(x => x.sub_category_id != 13), "sub_category_id", "sub_category_name_en");
            return View();
        }  
        public ActionResult Sorry()
        {
            return View();
       }

        public string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            md5Hash = null;
            return sBuilder.ToString();
        }
        public ActionResult ChangeCurrentCulture(int id)
        {
            SessionManager.CurrentCulture = id;
            Session["CurrentCulture"] = id;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Datepicker()
        {
            return View();
        }
        public ActionResult Contact()
        {
            var smt_contact = db.smt_contact.FirstOrDefault();
            return View(smt_contact);
        }
        public ActionResult Introduce()
        {
            var smt_intro = db.smt_intro.FirstOrDefault();
            return View(smt_intro);
        }
        public ActionResult ViewTour()
        {
            var smt_link_tour = db.smt_link_tour.ToList();
            return View(smt_link_tour);
        }

        [WebMethod]
        public JsonResult SearchQuery(int category_id, string searchString, int language)
        {
           
            if (category_id != 0)
            {
               
                if(language ==1) // english
                {
                    
                   var list = db.smt_location.Where(a => a.sub_category_id == category_id && (a.location_name_en.ToLower().Contains(searchString.ToLower()) || a.location_addr_en.ToLower().Contains(searchString.ToLower()))).ToList();
                    return Json(list.Select(c => new {location_name_en = c.location_name_en,location_category = c .sub_category_id, location_id = c.location_id, location_name = c.location_name_en, location_addr = c.location_addr_en, location_short_des = c.location_short_des_en, location_icon = c.location_icondir }).ToList(), JsonRequestBehavior.AllowGet);
                }
                    
                else
                {
                    var list = db.smt_location.Where(a => a.sub_category_id == category_id && (a.location_name_vi.ToLower().Contains(searchString.ToLower()) || a.location_addr.ToLower().Contains(searchString.ToLower()))).ToList();
                    return Json(list.Select(c => new {location_name_en = c.location_name_en, location_category = c.sub_category_id, location_id = c.location_id, location_name = c.location_name_vi, location_addr = c.location_addr, location_short_des = c.location_short_des_vi, location_icon = c.location_icondir }).ToList(), JsonRequestBehavior.AllowGet);
                } 
            }
            else 
            {
                if (language == 1) // english
                {
                    var list = db.smt_location.Where(a => a.location_name_en.ToLower().Contains(searchString.ToLower()) || a.location_addr_en.ToLower().Contains(searchString.ToLower()) && a.agent_id == '1').ToList();
                    return Json(list.Select(c => new { location_name_en = c.location_name_en, location_category = c.sub_category_id, location_id = c.location_id, location_name = c.location_name_en, location_addr = c.location_addr_en, location_short_des = c.location_short_des_en, location_icon = c.location_icondir }).ToList(), JsonRequestBehavior.AllowGet);
                }

                else
                {
                    var list = db.smt_location.Where(a => a.location_name_vi.ToLower().Contains(searchString.ToLower()) || a.location_addr.ToLower().Contains(searchString.ToLower()) && a.agent_id == '1').ToList();
                    return Json(list.Select(c => new { location_name_en = c.location_name_en, location_category = c.sub_category_id, location_id = c.location_id, location_name = c.location_name_vi, location_addr = c.location_addr, location_short_des = c.location_short_des_vi, location_icon = c.location_icondir }).ToList(), JsonRequestBehavior.AllowGet);
                } 
            }
        }
   
    }
}
