using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTourWeb.WIFI
{
    public class PointMembers
    {
        public int[] RealXY = new Int32[2];
        public Hashtable BSSID;
        public double[] ComputedXy { get; set; }

        public PointMembers()
        {
            RealXY = new int[] { 0, 0 };
            BSSID = new Hashtable();
        }
        public void setXY(int x, int y)
        {
            this.RealXY = new int[] { x, y };
        }
        public void setBSSID(Hashtable bssid)
        {
            this.BSSID = bssid;
        }
        public int[] getXY()
        {
            return this.RealXY;
        }
        public Hashtable getBSSID()
        {
            return this.BSSID;
        }
        public double getErrorDistance()
        {
            double denta = (RealXY[0] - ComputedXy[0]) * (RealXY[0] - ComputedXy[0]) + (RealXY[1] - ComputedXy[1]) * (RealXY[1] - ComputedXy[1]);
            return Math.Sqrt(denta);
        }
    }
}
