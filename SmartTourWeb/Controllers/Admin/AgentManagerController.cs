using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartTourWeb.Models;
using AttributeRouting.Web.Mvc;
using AttributeRouting;
using System.IO;

namespace SmartTourWeb.Controllers.Admin
{
    [Authorize(Roles = "1")] // admin moi duoc dung trang nay
    public class AgentManagerController : Controller
    {
        private smtdbEntities db = new smtdbEntities();

        //
        // GET: /AgentManager/
        public ActionResult Index()
        {
            var agents = db.agents.Include(a => a.group);
            return View(agents.ToList());
        }

        //
        // GET: /AgentManager/Details/5
        public ActionResult Details(int id = 0)
        {
           // smtdbEntities1 db1 = new smtdbEntities1();

            agent agent = db.agents.Find(id);
            ViewData["category"] = db.smt_sub_category.ToList();
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }


        [HttpPost] // chinh sưa quyen han nhom
        public ActionResult Details(agent_category_permission agent_category_permission, FormCollection c,int id = 0)
        {

                foreach (var cate in db.smt_sub_category)
                {
                    smtdbEntities db1 = new smtdbEntities(); 
                    string cateid = cate.sub_category_id.ToString(); // category id kieu string de nhap du lieu tu cac input trang view
                    int check = Convert.ToInt32(c[cateid]);
                    int indb = -1;
                    try
                    {
                        //kiem tra su ton tai cua per
                        indb = db1.agent_category_permission.Where(x => x.agent_id == id && x.category_id == cate.sub_category_id).Select(y => y.permission).First();

                    }
                    catch (Exception e)
                    {
                    }
                    if (indb != -1)
                    {
                        agent_category_permission.agent_id = id;
                        agent_category_permission.category_id = cate.sub_category_id;
                        agent_category_permission.time_create = DateTime.Now;
                        agent_category_permission.user_created = (int)Session["USER_ID"];
                        agent_category_permission.permission = (sbyte)((check == 1) ? 1 : 0);

                        db1.Entry(agent_category_permission).State = EntityState.Modified;
                        db1.SaveChanges();
                    }
                    else
                    {
                        if (check == 1)
                        {
                            agent_category_permission agent_permission = new agent_category_permission();
                            agent_permission.agent_id = id;
                            int abc = agent_category_permission.category_id;
                            agent_permission.category_id = cate.sub_category_id;
                            agent_permission.permission = (sbyte)((check == 1) ? 1 : 0);
                            agent_permission.time_create = DateTime.Now;
                            agent_permission.user_created = (int)Session["USER_ID"];
                            db1.agent_category_permission.Add(agent_permission);
                            db1.SaveChanges();
                        }
                        else
                        { }
                    }
                    //db.SaveChanges();
                }
              //  db1.Dispose();

          //  AGENT agent = db.AGENTs.Find(id);
            return RedirectToAction("Index");

        }

        //
        // GET: /AgentManager/Create
     
        public ActionResult Create()
        {
            ViewBag.group_id = new SelectList(db.groups, "id", "group_name");
            return View();
        }

        //
        // POST: /AgentManager/Create

        [HttpPost]
        public ActionResult Create(agent agent)
        {
            if (ModelState.IsValid)
            {
                db.agents.Add(agent);
                db.SaveChanges();
               
                 
                //Tao thu muc img fa video
                string groupname = db.groups.Where(x => x.id == agent.group_id).Select(y => y.group_name).FirstOrDefault().ToString();
                var folder = Server.MapPath("~/Uploads/" + groupname + "/" + agent.agent_id);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }



                return RedirectToAction("Index");
            }

            ViewBag.group_id = new SelectList(db.groups, "id", "group_name", agent.group_id);
            return View(agent);
        }

        //
        // GET: /AgentManager/Edit/5
        public ActionResult Edit(int id = 0)
        {
            agent agent = db.agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            ViewBag.group_id = new SelectList(db.groups, "id", "group_name", agent.group_id);



            return View(agent);
        }

        //
        // POST: /AgentManager/Edit/5

        [HttpPost]
        public ActionResult Edit(agent agent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.group_id = new SelectList(db.groups, "id", "group_name", agent.group_id);
            return View(agent);
        }

        //
        // GET: /AgentManager/Delete/5
        public ActionResult Delete(int id = 0)
        {
            agent agent = db.agents.Find(id);
            if (agent == null )
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        //
        // POST: /AgentManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            agent agent = db.agents.Find(id);
            db.agents.Remove(agent);
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