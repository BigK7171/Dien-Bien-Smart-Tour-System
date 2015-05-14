using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace QuanLyAccount
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string user_email)
        {
            ////-----------------ket noi voi database de lay quyen cua tai khoan-----------------
            SmartTourWeb.Models.smtdbEntities db = new SmartTourWeb.Models.smtdbEntities(); //tao ket noi voi database
             var account = db.smt_user.Single(x => x.user_email.Equals(user_email)); //tuong duong voi cau lenh "Select Top 1 From Accounts Where NameAccount = username"
            if (account != null) //neu tim thay tai khoan
            {
                return new String[] { account.agent_id.ToString() }; //tra ve quyen cua nguoi dung
            }
            else
                return new String[] { }; //new khong tim thay tai khoan thi gan quyen bang null
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }



        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}