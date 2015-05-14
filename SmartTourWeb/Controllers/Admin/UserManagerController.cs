using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
//using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
//using WebMatrix.WebData;
using System.Security.Cryptography;
using System.Text;
using SmartTourWeb.Models;
using PagedList;
using System.Net;
using AttributeRouting.Web.Mvc;
using AttributeRouting;
using System.IO;

namespace SmartTourWeb.Controllers.Admin
{
    public class UserManagerController : Controller
    {
        private Models.smtdbEntities db = new Models.smtdbEntities();

        //
        // GET: /UserManager/
        [Authorize(Roles = "1")] // admin moi duoc dung trang nay
        public ActionResult Index(string sortOrder = null, string searchString = null, int? page = 1)
        {
            if (Convert.ToInt32(Session["USER_ID"]) == 1)
            {
                var smt_user = db.smt_user.Include("agent").AsQueryable();
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
                    smt_user = smt_user.Where(s => s.user_email.ToUpper().Contains(searchString.ToUpper())
                                           || s.user_email.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        smt_user = smt_user.OrderByDescending(s => s.user_email);
                        break;
                    case "Date":
                        smt_user = smt_user.OrderBy(s => s.created_time);
                        break;
                    case "date_desc":
                        smt_user = smt_user.OrderByDescending(s => s.created_time);
                        break;
                    default:
                        smt_user = smt_user.OrderBy(s => s.user_email);
                        break;
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(smt_user.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return HttpNotFound();
            }

        }

        //
        // GET: /UserManager/Details/5
        [Authorize(Roles = "")]// dăng nhập mới dùng được chức năng này
        public ActionResult Details(int id = 0)
        {
            int user_id = Convert.ToInt32(Session["USER_ID"]);
            if (Convert.ToInt32(Session["agent"]) == 1 || id == user_id)
            {
                smt_user smt_user = db.smt_user.Single(s => s.user_id == id);
                if (smt_user == null)
                {
                    return HttpNotFound();
                }
                return View(smt_user);
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // GET: /UserManager/Create
        [Authorize(Roles = "1")] // admin moi duoc dung trang nay
        public ActionResult Create()
        {
            if (Convert.ToInt32(Session["agent"]) == 1)
            {
                ViewBag.agent_id = new SelectList(db.agents, "agent_id", "agent_name");
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // POST: /UserManager/Create

        [HttpPost]
        [Authorize(Roles = "1")] // admin moi duoc dung trang nay
        public ActionResult Create(smt_user smt_user)
        {
            if (ModelState.IsValid)
            {
                string email = smt_user.user_email;
                bool check = true;
                try
                {
                    string _email = db.smt_user.Where(a => a.user_email == email).Select(x => x.user_email).Single();
                    check = false;
                }
                catch (Exception e)
                {
                }

                if (!check)  // kiem tra email ton tai ko
                {
                    ViewBag.Message = "Email đã tồn tại";
                    ViewBag.agent_id = new SelectList(db.agents, "agent_id", "agent_name", smt_user.agent_id);
                    return View(smt_user);
                }
                else
                {
                    string usergroup = db.agents.Where(x => x.agent_id == smt_user.agent_id).Select(y => y.group.group_name).FirstOrDefault().ToString();
                    smt_user.created_time = DateTime.Now;
                    string ID = smt_user.user_email;
                    string PASS = smt_user.pasword;
                    smt_user.secret = GetMd5Hash(ID + PASS);   // ma hoa pass
                    smt_user.state = 0;
                    db.smt_user.Add(smt_user);
                    db.SaveChanges();
                    //Create folder
                    var folder = Server.MapPath("~/Uploads/" + usergroup + "/" + smt_user.agent_id + "/" + smt_user.user_email);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    var folder1 = Server.MapPath("~/Uploads/" + usergroup + "/" + smt_user.agent_id + "/" + smt_user.user_email + "/Videos");
                    if (!Directory.Exists(folder1))
                    {
                        Directory.CreateDirectory(folder1);
                    }
                    var folder2 = Server.MapPath("~/Uploads/" + usergroup + "/" + smt_user.agent_id + "/" + smt_user.user_email + "/Images");
                    if (!Directory.Exists(folder2))
                    {
                        Directory.CreateDirectory(folder2);
                    }

                    ModelState.Clear();
                    smt_user = null;
                    ViewBag.agent_id = new SelectList(db.agents, "agent_id", "agent_name");
                    ViewBag.Message = "Tạo mới tài khoản thành công";
                    smt_user = null;
                    return View(smt_user);
                }
            }
            ViewBag.agent_id = new SelectList(db.agents, "agent_id", "agent_name", smt_user.agent_id);
            return View(smt_user);
        }

        #region [Register]
        public ActionResult Register()
        {
            ViewBag.Error = null;
            ViewBag.Success = null;
            return View();
        }
        [HttpPost]
        public ActionResult Register(smt_user smt_user)
        {
            ViewBag.Error = null;
            ViewBag.Success = null;
            if (ModelState.IsValid)
            {
                string email = smt_user.user_email;
                bool check = true;
                try
                {
                    string _email = db.smt_user.Where(a => a.user_email == email).Select(x => x.user_email).Single();
                    check = false;
                }
                catch (Exception e)
                {
                }

                if (!check)  // kiem tra email ton tai ko
                {
                    ViewBag.Error = "Email đã tồn tại";
                    smt_user.pasword = null;
                    smt_user.ConfirmPassword = null;
                    return View(smt_user);
                }
                else
                {
                    smt_user.agent_id = 3;
                    string usergroup = db.agents.Where(x => x.agent_id == smt_user.agent_id).Select(y => y.group.group_name).FirstOrDefault().ToString();
                    smt_user.created_time = DateTime.Now;
                    string ID = smt_user.user_email;
                    string PASS = smt_user.pasword;
                    smt_user.secret = GetMd5Hash(ID + PASS);   // ma hoa pass
                    smt_user.state = 0;
                    db.smt_user.Add(smt_user);
                    db.SaveChanges();
                    //Create folder
                    var folder = Server.MapPath("~/Uploads/" + usergroup + "/" + smt_user.agent_id + "/" + smt_user.user_email);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    var folder1 = Server.MapPath("~/Uploads/" + usergroup + "/" + smt_user.agent_id + "/" + smt_user.user_email + "/Videos");
                    if (!Directory.Exists(folder1))
                    {
                        Directory.CreateDirectory(folder1);
                    }
                    var folder2 = Server.MapPath("~/Uploads/" + usergroup + "/" + smt_user.agent_id + "/" + smt_user.user_email + "/Images");
                    if (!Directory.Exists(folder2))
                    {
                        Directory.CreateDirectory(folder2);
                    }

                    ModelState.Clear();
                    smt_user = null;
                    ViewBag.Success = "Tạo mới tài khoản thành công";
                    return View(smt_user);
                }
            }
            return View(smt_user);
        }

        #endregion

        //
        // GET: /UserManager/Edit/5
        [Authorize(Roles = "")] // dang nhap moi duoc dung trang nay
        public ActionResult Edit(int id = 0)
        {
            if (Convert.ToInt32(Session["agent"]) == 1 || id == Convert.ToInt32(Session["USER_ID"]))
            {
                smt_user smt_user = db.smt_user.Single(s => s.user_id == id);
                if (smt_user == null)
                {
                    return HttpNotFound();
                }
                ViewBag.agent_id = new SelectList(db.agents, "agent_id", "agent_name", smt_user.agent_id);
                return View(smt_user);
            }
            else
            {
                return HttpNotFound();
            }

        }

        //
        // POST: /UserManager/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(smt_user smt_user)
        {
            if (ModelState.IsValid)
            {
                if (smt_user.user_id == (int)Session["USER_ID"])
                {
                Session["USER_NAME"] = smt_user.username;
                Session["LOGO"] = smt_user.user_logo;
                }
                db.Entry(smt_user).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.agent_id = new SelectList(db.agents, "agent_id", "agent_name", smt_user.agent_id);
            return RedirectToAction("Details", new { id = smt_user.user_id });
        }

        //
        // GET: /UserManager/Delete/5
        [Authorize(Roles = "1")] // admin moi duoc dung trang nay
        public ActionResult Delete(int id = 0)
        {
            if (Convert.ToInt32(Session["USER_ID"]) == 1)
            {
                smt_user smt_user = db.smt_user.SingleOrDefault(s => s.user_id == id);
                if (smt_user == null)
                {
                    return HttpNotFound();
                }
                return View(smt_user);
            }
            else
            {
                return HttpNotFound();
            }

        }

        //
        // POST: /UserManager/Delete/5
        [Authorize(Roles = "1")] // admin moi duoc dung trang nay
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            smt_user smt_user = db.smt_user.Single(s => s.user_id == id);
            db.smt_user.Remove(smt_user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        #region [login]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection, smt_user smt_user)
        {
          //  return View();
            string PASS = collection["PASS"];
            string ID = collection["IDemail"];
            
            DateTime t = DateTime.Now;
            bool validEmail = db.smt_user.Any(x => x.user_email == ID);

            if (!validEmail)
            {
                ViewBag.Message = "Sai email hoặc mật khẩu, vui lòng thử lại"; 
                return View();
            }

            string secret = db.smt_user.Where(x => x.user_email == ID)
                                 .Select(x => x.secret)
                                 .Single();

            MD5 md5Hash = MD5.Create();
            string hash = GetMd5Hash(ID + PASS);
            if (secret.Equals(hash)) // Login success
            {
                var user_infomation = db.smt_user.Where(x => x.user_email == ID)
                                 .Single();
                Session["USER_NAME"] = user_infomation.username;
                Session["LOGO"] = user_infomation.user_logo;
                Session["USER_EMAIL"] = ID;
                Session["USER_ID"] = user_infomation.user_id;
                Session["AGENT"] = user_infomation.agent_id;
                Session["GROUPNAME"] = user_infomation.agent.group.group_name;
                Session["GROUPID"] = user_infomation.agent.group_id;
                //Lưu thời gian đăng nhập và ip đăng nhập
                //smtdbEntities1 db1 = new smtdbEntities1(); 
                //smt_user.user_id = user_infomation.user_id;
                //smt_user.user_email = user_infomation.user_email;
                //smt_user.secret = user_infomation.secret;
                //smt_user.username = user_infomation.username;
                //smt_user.user_logo = user_infomation.user_logo;
                //smt_user.phone = user_infomation.phone;
                //smt_user.address = user_infomation.address;
                //smt_user.state = user_infomation.state;
                //smt_user.created_time = user_infomation.created_time;
                //smt_user.agent_id = user_infomation.agent_id;
                //smt_user.last_login_time = DateTime.Now;
                //// IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                ////smt_user.last_login_ip = localIPs[3].ToString(); // lay ip cua may  ????
                //db1.Entry(smt_user).State = EntityState.Modified;
                //db1.SaveChanges();
                //db1.Dispose();
                FormsAuthentication.SetAuthCookie(ID, false); // Lay Role trong CustomRoleProvider.cs

                //if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                //       && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))//doan code nay nham muc dich khi ma ban vao mot trang nao do mo khong dung quyen han no se bat ban dang nhap.neu thanh cong no se tro lai trang truoc do ma ban muon vao.
                //{
                //    return Redirect(returnUrl); //tro lai trang truoc do ma ban muon vao
                //}
                //else
                //{
                    return RedirectToAction("Index", "Home"); //chuyen sang trang Index cua controllers Home.
                //}
            }
            else
            {
                ViewBag.Message = "Sai email hoặc mật khẩu, vui lòng thử lại";
                return View();
            }

        }
        #endregion

        #region [Change password]
        [Authorize(Roles = "")] // dang nhap moi duoc dung trang nay
        public ActionResult ChangePass(int id = 0)
        {
            smt_user smt_user = db.smt_user.Single(s => s.user_id == id);
            if (smt_user == null)
            {
                return HttpNotFound();
            }
            return View(smt_user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ChangePass(smt_user smt_user, FormCollection c)
        {
            if (ModelState.IsValid)
            {
                string ID = smt_user.user_email;
                string PASS = smt_user.pasword;
                string oldsecret = GetMd5Hash(ID + c["oldpass"]);
                string abc = db.smt_user.Where(x => x.user_email == ID).Select(y => y.secret).First();

                if (oldsecret == abc)
                {
                    smt_user.secret = GetMd5Hash(ID + PASS);
                    db.Entry(smt_user).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Đổi mật khẩu thành công";
                    return View();
                }

                ViewBag.Message = "Mật khẩu cũ chưa chính xác";
                return View(smt_user);
            }
            return View(smt_user);
        }
        #endregion

        public int getpermission(int user_id, int category_id)
        {
            int per = 0;
            try
            {
                int agent_id = db.smt_user.Where(x => x.user_id == user_id).Select(y => y.agent_id).First();
                per = db.agent_category_permission.Where(x => x.category_id == category_id && x.agent_id == agent_id).Select(y => y.permission).First();
            }
            catch (Exception e)
            {
            }
            return per;

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

        [Authorize(Roles = "")] // dang nhap moi duoc dung trang nay
        public ActionResult Logout()
        {

            Session["USER_NAME"] = null;
            Session["USER_ID"] = null;
            FormsAuthentication.SignOut();//xoa du lieu quyen va tai khoan
            return RedirectToAction("Index", "Home");
        }

        public string currentFilter { get; set; }
    }
}