using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTourWeb.WIFI
{
    public class ReferencePoint
    {
        public List<PointMembers> ListMembers { get; set; }
        public Hashtable BssidVarAndMean { get; set; } // Save Mean and Variance of AP's level as double array []
        public double Mean { get; set; }
        public double distance { get; set; }
        public int[] XY { get; set; }
        public int refID { get; set; }
        public ReferencePoint()
        {

            Mean = 0.0;
            ListMembers = new List<PointMembers>();
            BssidVarAndMean = new Hashtable();

        }
    }
}