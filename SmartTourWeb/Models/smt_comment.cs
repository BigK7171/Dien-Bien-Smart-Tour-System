//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartTourWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class smt_comment
    {
        public int comment_id { get; set; }
        public int user_id { get; set; }
        public long location_id { get; set; }
        public string comment_contain { get; set; }
        public Nullable<System.DateTime> comment_time { get; set; }
    
        public virtual smt_location smt_location { get; set; }
        public virtual smt_user smt_user { get; set; }
    }
}
