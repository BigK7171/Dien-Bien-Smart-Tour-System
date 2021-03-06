﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
namespace SmartTourWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class smt_user
    {
        public smt_user()
        {
            this.smt_comment = new HashSet<smt_comment>();
            this.smt_layer = new HashSet<smt_layer>();
            this.smt_location = new HashSet<smt_location>();
            this.smt_parent_category = new HashSet<smt_parent_category>();
            this.smt_sub_category = new HashSet<smt_sub_category>();
            
            this.smt_user_history = new HashSet<smt_user_history>();
        }

        [Display(Name = "Mã thành viên")]
        public int user_id { get; set; }
        [Display(Name = "Email đăng nhập")]
        [Required(ErrorMessage = "Vui lòng cung cấp địa chỉ Email")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Vui lòng nhập đúng địa chỉ email")]
        public string user_email { get; set; }
        [Display(Name = "Mật khẩu mã hóa")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string secret { get; set; }
        [Display(Name = "Mật khẩu")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage = "Vui lòng điền mật khẩu")]
        [StringLength(20, ErrorMessage = "Mật khẩu phải có ít nhất 6 đến 20 kí tự", MinimumLength = 6)]
        public string pasword { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("pasword", ErrorMessage = "Xác nhận mật khẩu chưa chính xác")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Tên thành viên")]
        [Required(ErrorMessage = "Vui lòng cung cấp tên của bạn")]
        [StringLength(30, ErrorMessage = "Tên quá dài hoặc quá ngắn", MinimumLength = 2)]
        public string username { get; set; }
        [Display(Name = "Địa chỉ")]
        public string address { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(11, ErrorMessage = "Nhập sai số điện thoại", MinimumLength = 10)]
        public string phone { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<sbyte> state { get; set; }
        [Display(Name = "Thời gian đăng ký")]
        public System.DateTime created_time { get; set; }
        [Display(Name = "IP đăng nhập trước")]
        public string last_login_ip { get; set; }
        [Display(Name = "Thời gian đăng nhập trước")]
        public Nullable<System.DateTime> last_login_time { get; set; }
        [Display(Name = "Ảnh thành viên")]
        public string user_logo { get; set; }
        [Display(Name = "Mã đơn vị quản lý")]
        [Required(ErrorMessage = "Bắt buộc nhập đơn vị quản lý")]
        public int agent_id { get; set; }

        public virtual agent agent { get; set; }
        public virtual ICollection<smt_comment> smt_comment { get; set; }
        public virtual ICollection<smt_layer> smt_layer { get; set; }
        public virtual ICollection<smt_location> smt_location { get; set; }
        public virtual ICollection<smt_parent_category> smt_parent_category { get; set; }
        public virtual ICollection<smt_sub_category> smt_sub_category { get; set; }
      
        public virtual ICollection<smt_user_history> smt_user_history { get; set; }
    }
}
