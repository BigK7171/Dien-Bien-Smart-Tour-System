using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using SmartTourWeb.Models;

namespace SmartTourWeb.Controllers.Home
{
    public class WebgisController : Controller
    {
        //
        // GET: /Webgis/
        private smtdbEntities db = new smtdbEntities();
        public ActionResult Index()
        {
            ViewBag.sub_category_id = new SelectList(db.smt_sub_category.Where(x => x.sub_category_id != 13), "sub_category_id", "sub_category_name_vi");
            return View();
        }
        [WebMethod]
        public JsonResult getLayer(int category_id)
        {
            var list = db.smt_location.Where(a => a.sub_category_id == category_id).ToList();
            return Json(list.Select(c => new { location_id = c.location_id, location_name = c.location_name_vi, location_lat = c.location_latitude, locaton_lon = c.location_longitude }).ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
