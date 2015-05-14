using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Drawing;
using System.Text;
using System.Web.Hosting;
using System.Security.Cryptography;
using System.Collections;
using MySql.Data.MySqlClient;
using SmarttourLib.Route;
using SmarttourLib.Wbs;
namespace SmartTourWeb
{
    /// <summary>
    /// Summary description for SmarttourService
    /// </summary>
    [WebService(Namespace = "http://smt.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SmarttourService : System.Web.Services.WebService
    {
        private static string SERVER = "localhost";
        private static string DATABASE = "smtdb";
        private static string USER = "smt";
        private static string PASSWORD = "Tinhdx@2015";
        // This function get modified Categories and new categories  after check timestamp
        // The format of string  _categories as same as 1#timestamp;2#timestamp
        public MySqlConnection GetConnection()
        {
            string ctr = @"server=" + SERVER + ";database=" + DATABASE + ";userid=" + USER + ";password=" + PASSWORD;
            // MySqlConnection con = null;
            try
            {
                MySqlConnection con = new MySqlConnection(ctr);
                con.Open();
                return con;

            }
            catch (MySqlException err)
            {

                Console.Out.WriteLine(err.ToString());
                return null;
            }
        }
        [WebMethod]
        public List<SmarttourLib.Wbs.Category> getUpdateCategory(string _categories)
        {


            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Category> category = service.getUpdateCategory(_categories);
            service = null;
            return category;

        }
        // This function gets list parent category 
        [WebMethod]
        public List<SmarttourLib.Wbs.ParentCategory> getUpdateParentCategory(string _ParentCategories)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.ParentCategory> parent = service.getUpdateParentCategory(_ParentCategories);
            service = null;
            return parent;


        }
        [WebMethod]
        public List<SmarttourLib.Wbs.Location> getLocationOnLayer(int layer_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> lstLocation = service.getLocationOnLayer(layer_id);
            service = null;
            return lstLocation;
        }

        // this method get location when user search
        [WebMethod]
        public List<SmarttourLib.Wbs.Location> searchLocation(string name, int category, int user_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> lstLocation = service.searchLocation(name, category, user_id);
            service = null;
            return lstLocation;

        }


        //  This function check location update 
        // The format of string  _location as same as 1#timestamp;2#timestamp
        [WebMethod]
        public List<SmarttourLib.Wbs.Location> getUpdateLocation(string _locations, int user_id)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> lstLocation = service.getUpdateLocation(_locations, user_id);
            service = null;
            return lstLocation;

        }
        public bool getLocationPermitView(int user_id, Int64 location_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            bool lstLocation = service.getLocationPermitView(user_id, location_id);
            service = null;
            return lstLocation;

        }
        [WebMethod]
        public SmarttourLib.Wbs.Location getLocation(int locationid, string language)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            SmarttourLib.Wbs.Location lstLocation = service.getLocation(locationid, language);
            service = null;
            return lstLocation;

        }

        // This function insert location to database server
        [WebMethod]
        public string insertLocation(double lat, double lon, int user_id, string user_email, string password, int category, string language, string location_name, string location_adddr, string location_des, string location_contact_web, string location_contact_phone, string image, int location_shared, int check_image)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            string lstLocation = service.insertLocation(lat, lon, user_id, user_email, password, category, language, location_name, location_adddr, location_des, location_contact_web, location_contact_phone, image, location_shared, check_image);
            service = null;
            return lstLocation;

        }
        [WebMethod]
        public string updateLocation(int locationid, double lat, double lon, int user_id, string user_email, string password, int category, string language, string location_name, string location_adddr, string location_des, string location_contact_web, string location_contact_phone, string image, int location_shared, int check_image)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            string lstLocation = service.updateLocation(locationid, lat, lon, user_id, user_email, password, category, language, location_name, location_adddr, location_des, location_contact_web, location_contact_phone, image, location_shared, check_image);
            service = null;
            return lstLocation;

        }

        [WebMethod]
        public List<SmarttourLib.Wbs.Location> getLocationOnstart(int index, string language)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> lstpermision = service.getLocationOnstart(index, language);
            service = null;
            return lstpermision;
        }
        // For debug
        /*
           [WebMethod]
           public string getChildLocation(int parent_id, string language)
           {
               SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
               string lstpermision = service.getChildLocation(parent_id, language);
               service = null;
               return lstpermision;
           } 
         * */

        [WebMethod]
        public List<SmarttourLib.Wbs.Location> getChildLocation(int parent_id, string language)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> lstpermision = service.getChildLocation(parent_id, language);
            service = null;
            return lstpermision;
        }

        public string[] getPriveFilePath(int user_id)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            //Int64 lstLocation = service.insertLocation(lat, lon, user_id, user_email, password, category, language, location_name, location_adddr, location_des, location_contact_web, location_contact_phone, image, location_shared, check_image);
            string[] paths = service.getPriveFilePath(user_id);

            service = null;
            return paths;

        }

        public Int64 getMaxLocationid()
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            Int64 id = service.getMaxLocationid();
            service = null;
            return id;

        }

        // This function get user permission
        [WebMethod]
        public List<SmarttourLib.Wbs.Permission> getUserPermission(int user_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Permission> lstpermision = service.getUserPermission(user_id);
            service = null;
            return lstpermision;
        }

        // This function get description of a location
        [WebMethod]
        public SmarttourLib.Wbs.Location getLocationDes(int location_id, string language)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            SmarttourLib.Wbs.Location lstpermision = service.getLocationDes(location_id, language);
            service = null;
            return lstpermision;

        }

        // This function get icon of location as byte array
        [WebMethod]
        public SmarttourLib.Wbs.Location getIconLocation(Int64 location_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            SmarttourLib.Wbs.Location lstpermision = service.getIconLocation(location_id);
            service = null;
            return lstpermision;

        }

        [WebMethod]
        public List<SmarttourLib.Wbs.Location> getLayer2(string layername)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> layer = service.getLayer(layername);
            service = null;
            return layer;

        }

        // This function get all layer of web map services

        //[WebMethod]
        //public List<SmarttourLib.Wbs.Layers> getLayer()
        //{
        //    SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
        //    List<SmarttourLib.Wbs.Layers> lstpermision = service.getLayer();
        //    service = null;
        //    return lstpermision;

        //}

        [WebMethod]
        public string Register(string useremail, string username, string password, string phone, string addr)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            string lstpermision = service.Register(useremail, username, password, phone, addr);
            service = null;
            return lstpermision;


        }

        [WebMethod]
        public string RegisterWithImage(string useremail, string username, string password, string phone, string addr, string image)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            string lstpermision = service.RegisterWithImage(useremail, username, password, phone, addr, image);
            service = null;
            return lstpermision;


        }

        [WebMethod]
        public List<SmarttourLib.Wbs.UserHistory> Login(string email, string password)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.UserHistory> lstpermision = service.Login(email, password);
            service = null;
            return lstpermision;


        }

        [WebMethod]
        public List<SmarttourLib.Wbs.Location> getLocationHistory(int user_id, int numbers, int offset)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> lstpermision = service.getLocationHistory(user_id, numbers, offset);
            service = null;
            return lstpermision;


        }

        [WebMethod]
        public SmarttourLib.Wbs.UserHistory updateUserHistory(int user_id, int location_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            SmarttourLib.Wbs.UserHistory lstpermision = service.updateUserHistory(user_id, location_id);
            service = null;
            return lstpermision;

        }

        [WebMethod]
        public List<SmarttourLib.Wbs.Comment> getComment(int location_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Comment> lstpermision = service.getComment(location_id);
            service = null;
            return lstpermision;

        }
        [WebMethod]
        public SmarttourLib.Wbs.Comment insertComment(int user_id, int location_id, string comment_contain)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            SmarttourLib.Wbs.Comment lstpermision = service.insertComment(user_id, location_id, comment_contain);
            service = null;
            return lstpermision;

        }
        [WebMethod]
        public SmarttourLib.Wbs.Comment updateComment(int comment_id, string comment_contain)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            SmarttourLib.Wbs.Comment lstpermision = service.updateComment(comment_id, comment_contain);
            service = null;
            return lstpermision;

        }

        [WebMethod]
        public bool deleteComment(int comment_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            bool lstpermision = service.deleteComment(comment_id);
            service = null;
            return lstpermision;

        }
        [WebMethod]
        public List<SmarttourLib.Wbs.Location> searchAdvance(int category, double lat0, double lon0, double radius, int user_id, int location_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> lstpermision = service.searchAdvance(category, lat0, lon0, radius, user_id, location_id);
            service = null;
            return lstpermision;
        }
        public double getDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 111189.577;
            return (dist);
        }
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        [WebMethod]
        public List<SmarttourLib.Wbs.Tour> searchTour(string tourname)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Tour> lstpermision = service.searchTour(tourname);
            service = null;
            return lstpermision;

        }
        [WebMethod]
        public List<SmarttourLib.Wbs.Tour> getTourRelated(int locationid)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Tour> lstpermision = service.getTourRelated(locationid);
            service = null;
            return lstpermision;

        }
        [WebMethod]
        public SmarttourLib.Wbs.Tour getTour(int id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            SmarttourLib.Wbs.Tour lstpermision = service.getTour(id);
            service = null;
            return lstpermision;

        }
        [WebMethod]
        public int uploadUserImage(string image, int user_id, string current_pass, string user_email)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            int lstpermision = service.uploadUserImage(image, user_id, current_pass, user_email);
            service = null;
            return lstpermision;

        }
        [WebMethod]
        public List<SmarttourLib.Wbs.User> searchUser(string username)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.User> lstpermision = service.searchUser(username);
            service = null;
            return lstpermision;


        }

        [WebMethod]
        public SmarttourLib.Wbs.User getUserInfo(string email, string password)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            SmarttourLib.Wbs.User lstpermision = service.getUserInfo(email, password);
            service = null;
            return lstpermision;

        }


        [WebMethod]
        public List<SmarttourLib.Wbs.Location> getUserLocation(int user_id)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Location> lstpermision = service.getUserLocation(user_id);
            service = null;
            return lstpermision;


        }
        [WebMethod]
        public bool clearUserHistory(int user_id)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            bool lstpermision = service.clearUserHistory(user_id);
            service = null;
            return lstpermision;


        }
        [WebMethod]
        public int updateUserInfo(string user_email, string current_pass, string new_pass, string fullname, string phone, string addr)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            int lstpermision = service.updateUserInfo(user_email, current_pass, new_pass, fullname, phone, addr);
            service = null;
            return lstpermision;


        }

        [WebMethod]
        public List<SmarttourLib.Route.Position> getRoute(string pos1, string pos2)
        {

            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Route.Position> lstpermision = service.getRoute(pos1, pos2);
            service = null;
            return lstpermision;

        }
        [WebMethod]
        public string createGroup(string useremail, string user_id, string password, string groupname)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            string group = service.createGroup(useremail, user_id, password, groupname);
            service = null;
            return group;
        }
        [WebMethod]
        public string deleteLocation(int userid, string useremail, string userpass, int locationId)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            string rs = service.deleteLocation(userid, useremail, userpass, locationId);
            service = null;
            return rs;
        }
        [WebMethod]
        public string searchGroup(string groupname)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            string rs = service.searchGroup(groupname);
            service = null;
            return rs;
        }
        [WebMethod]
        public List<SmarttourLib.Wbs.Conversation> viewMoreConversation(string email, string pass, string currentdate, int from_user, int to_user)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Conversation> lst = service.viewMoreConversation(email, pass, currentdate, from_user, to_user);
            service = null;
            return lst;
        }
        [WebMethod]
        public List<SmarttourLib.Wbs.Conversation> getAllConversations(string email, string pass, string currentdate, int from_user)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Conversation> lst = service.getAllConversations(email, pass, currentdate, from_user);
            service = null;
            return lst;
        }
        [WebMethod]
        public List<SmarttourLib.Wbs.Conversation> getMesgOffline(string email, string pass, int from_user)
        {
            SmarttourLib.SmarttourService service = new SmarttourLib.SmarttourService(SERVER, USER, DATABASE, PASSWORD);
            List<SmarttourLib.Wbs.Conversation> lst = service.getMesgOffline(email, pass, from_user);
            service = null;
            return lst;
        }


        // Its for Wifi indoor positioning system
        [WebMethod]
        public string getPFPosition(string listLevel)
        {
            // List level seem as mac1:level1;mac2:level2....
            string[] lstMACandLevel = listLevel.Split(';');
            string listMac = "WPS_AccessPoint.MAC = 'TEST'";
            Hashtable MacAndLevel = new Hashtable();
            try
            {
                for (int i = 0; i < lstMACandLevel.Length; i++)
                {
                    try
                    {
                        string[] macs = lstMACandLevel[i].Split(',');
                        // macs[0] as mac id
                        // macs[1] as level
                        if (macs[0] != null && macs[1] != null)
                        {
                            // Add this mac and level to hashtable
                            MacAndLevel.Add(macs[0], macs[1]);
                            listMac += " OR WPS_AccessPoint.MAC = " + "'" + macs[0] + "'";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
            }

            if (MacAndLevel != null)
            {

                // Get list reference point that related to macs

                string listRefPoint = " REFERENCEPOINT_RP_ID = 0 ";
                // Connect database to get all Reference point that have mac id 
                MySqlConnection con = GetConnection();
                MySqlCommand cmd = con.CreateCommand();

                // Get all reference points that have mac id 
                cmd.CommandText = "select distinct REFERENCEPOINT_RP_ID from WPS_SignalStrengthRelationship left join WPS_AccessPoint on WPS_SignalStrengthRelationship.AccessPoint_AP_ID = WPS_AccessPoint.AP_ID where " + listMac;
                MySqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    int refID = read["REFERENCEPOINT_RP_ID"] == DBNull.Value ? 0 : read.GetInt32("REFERENCEPOINT_RP_ID");
                    if (refID > 0)
                        listRefPoint += " OR REFERENCEPOINT_RP_ID = " + refID.ToString();
                }
                read.Close();

                // Complete read all reference point

                List<WIFI.ReferencePoint> lstRefPoints = new List<WIFI.ReferencePoint>();

                cmd.Clone();

                cmd.CommandText = " select WPS_REFERENCEPOINT.RP_ID, WPS_REFERENCEPOINT.x,WPS_REFERENCEPOINT.y,WPS_AccessPoint.MAC,WPS_SignalStrengthRelationship.SS,WPS_SignalStrengthRelationship.Var FROM WPS_SignalStrengthRelationship left join WPS_AccessPoint on WPS_SignalStrengthRelationship.AccessPoint_AP_ID = WPS_AccessPoint.AP_ID left join  WPS_REFERENCEPOINT on WPS_SignalStrengthRelationship.REFERENCEPOINT_RP_ID = WPS_REFERENCEPOINT.RP_ID where WPS_SignalStrengthRelationship.SS > -100 and " + listRefPoint;
                read = cmd.ExecuteReader();

                while (read.Read())
                {
                    int refID = read["RP_ID"] == DBNull.Value ? 0 : read.GetInt32("RP_ID");

                    if (refID != 0)
                    {
                        string mac = read.GetString("MAC");
                        double mean = read["SS"] == DBNull.Value ? -100 : read.GetDouble("SS");
                        double var = read["Var"] == DBNull.Value ? 0 : read.GetDouble("Var");

                        WIFI.ReferencePoint refPoint = lstRefPoints.Where(x => x.refID == refID).FirstOrDefault();
                        if (refPoint != null) // That reference point existed
                        {
                            Hashtable BssidVarAndMean = refPoint.BssidVarAndMean;
                            BssidVarAndMean.Add(mac, new double[2] { mean, var });
                            refPoint.BssidVarAndMean = BssidVarAndMean;
                        }
                        else
                        {
                            WIFI.ReferencePoint newRefPoint = new WIFI.ReferencePoint();
                            newRefPoint.refID = refID;
                            int x = read["x"] == DBNull.Value ? -1 : read.GetInt32("x");
                            int y = read["y"] == DBNull.Value ? -1 : read.GetInt32("y");
                            if (x > -1 && y > -1)
                            {
                                newRefPoint.XY = new int[2] { x, y };
                                Hashtable BssidVarAndMean = new Hashtable();
                                BssidVarAndMean.Add(mac, new double[2] { mean, var });
                                newRefPoint.BssidVarAndMean = BssidVarAndMean;
                            }
                            lstRefPoints.Add(newRefPoint);
                        }

                    }
                }

                read.Close();
                con.Close();
                string result = getXY(MacAndLevel, lstRefPoints);

                return result;
            }
            return "Null";
        }

        public string getXY(Hashtable MacAndLevel, List<WIFI.ReferencePoint> lstRefPoints)
        {
            if (lstRefPoints != null && MacAndLevel != null)
            {
                foreach (WIFI.ReferencePoint refpoint in lstRefPoints)
                {
                    Hashtable bssrefpoint = refpoint.BssidVarAndMean;
                    double distance = getEuclidDistance(bssrefpoint, MacAndLevel);
                    refpoint.distance = distance;
                }
                List<WIFI.ReferencePoint> newLstRef = lstRefPoints.OrderBy(x => x.distance).ToList();
                double[] computedPosition = getXYStandard(newLstRef);
                return computedPosition[0].ToString() + ":" + computedPosition[1].ToString();

            }
            else
                return "null";
        }
        public double getEuclidDistance(Hashtable bssrefpoint, Hashtable bsslevel2)
        {
            double distance = 0.0;
            foreach (DictionaryEntry bss in bsslevel2)
            {
                //    int a = (int)bss.Key;
                if (bssrefpoint.ContainsKey(bss.Key))
                {
                    double[] level_var = bssrefpoint[bss.Key] as double[];
                    double var = level_var[1];
                    //   double level1 = 0.1 * level_var[0];                                                                                                                                                                                                                 Q // convert to mW
                    double level2 = double.Parse(bss.Value.ToString());
                    double diffdb = level_var[0] - level2;
                    //     distance += diffdb * diffdb / var;
                    distance += diffdb * diffdb;// / var;
                }

            }
            //    distance[1] = Math.Sqrt(distance[1]*1000000.000000d); // distance computed following mw
            // Debuging     
            distance = Math.Sqrt(distance);
            //        double[] test = distance;


            return distance;
            //}

        }
        public double[] getXYStandard(List<WIFI.ReferencePoint> listpoint)
        {
            // Get 10 reference points nearest following euclid distance
            double[] xy = new double[2];
            int N = 6; //
            double[] pi = new double[N];
            double sumpi = 0.0;

            // log = "Get XY by using Finger-Printing \r\n";
            //  tblog.Text += log;

            for (int i = 0; i < N; i++)
            {
                double distance = listpoint[i].distance;
                if (Math.Abs(distance) < 0.1)
                    return (new double[] { (double)listpoint[i].XY[0], (double)listpoint[i].XY[1] });
                //pi[i] = 1 / (distance * distance * distance);

                // Tinh theo ham Gauss
                double VarErrMea = 8; // Sai so do luong 5 m.
                pi[i] = Math.Exp(-0.5 * distance * distance / VarErrMea);
                sumpi += pi[i];
            }
            // get xy from pi
            for (int i = 0; i < N; i++)
            {
                int[] _xy = listpoint[i].XY;

                xy[0] += _xy[0] * pi[i] / sumpi;
                xy[1] += _xy[1] * pi[i] / sumpi;
                //            log += "Weight of " + _xy[0].ToString() + ":" + _xy[1].ToString() + " is " + (pi[i] / sumpi).ToString() + "\r\n";

            }
            //         log = "Computed Position of this Point: X= " + xy[0].ToString() + "---- Y= " + xy[1].ToString() + "\r\n";
            //         log += "Real Position of this Point: X= " + member.getXY()[0].ToString() + "---- Y= " + member.getXY()[1].ToString() + "\r\n";

            //        tblog.Text += log;
            xy[0] = Math.Round(xy[0], 3);
            xy[1] = Math.Round(xy[1], 3);
            return xy;

        }





        // For debug


        static string GetMd5Hash(string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            MD5 md5Hash = MD5.Create();
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
            return sBuilder.ToString();
        }

        public int CheckLogin(string email, string passwd)
        {

            int user_id = 0;
            string md5pass = GetMd5Hash(email + passwd);
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT user_id FROM SMT_USER WHERE user_email =@email AND secret = @password";
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password", md5pass);
            cmd.Prepare();
            MySqlDataReader read = cmd.ExecuteReader();
            if (read.Read())
            {

                user_id = Int32.Parse(read["user_id"].ToString());
                read.Close();

            }
            read.Close();
            con.Close();

            return user_id;
        }


    }
}
