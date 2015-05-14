using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SmartTourWeb.Controllers.Home
{
    public class testController : Controller
    {
        //
        // GET: /test/

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GetRainfallChart()
        {
            var key = new Chart(width: 600, height: 400)
                .AddSeries(
                    chartType: "bar",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "50", "40", "10", "10" })
                .Write();

            return null;
        }
        public ActionResult Datepicker()
    {
        return View();
    }
    }
}
