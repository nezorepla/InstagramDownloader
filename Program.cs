using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
class PhotoId
{
    Int64 _id;
    Int32 _uid;
    //string role;

    public Int64 ID
    {
        get { return _id; }
        set { _id = value; }
    }
    public Int32 uID
    {
        get { return _uid; }
        set { _uid = value; }
    }


}

      
namespace InstagramDownloader
{

    class Program
    {
               static string folder =@"C:\InstagramDownloader";
        
        static string c;
        static void Main(string[] args)
        {

            List<PhotoId> ids = new List<PhotoId> {/*new PhotoId { ID = 0, uID = 0 }*/ };
            string user = "hotyogaholic";
 

            string subPath = @""+folder+"\\" + user + "\\";
  
            System.IO.Directory.CreateDirectory(subPath);
           string zm = DateTime.Now.ToString("yyyy-M-d-h");
            File.AppendAllText(@""+folder+"\\" + user + "\\" + user + ".txt", zm); 

            for (int i = 0; i < 1000; i++)
            {



                var query = from p in ids where p.uID > 0 select p.ID;
                var uquery = from p in ids where p.uID > 0 select p.uID;
                string vl;

                try
                {
                    vl = query.Min().ToString() + "_" + uquery.Max().ToString();
                }
                catch
                {
                    vl = "0";
                }



                if (c != vl)
                {
                    c = vl;



                    string url = "https://www.instagram.com/" + user + "/media/?max_id=" + vl;


                    string json = rtJ(url);

                    Regex p1 = new Regex(@"(\d{18,19})([_]{1})(\d{9})");
                    Match m1 = p1.Match(json);
                    while (m1.Success)
                    {
                        string v = m1.Value;
                        int IntSon = v.IndexOf("_", 1);
                        Int64 id = Int64.Parse(v.Substring(0, IntSon));


                        int IntBas = v.IndexOf("_") + 1;
                        int IntSon2 = v.Length - IntBas;
                        Int32 uid = Int32.Parse(v.Substring(IntBas, IntSon2));

                        ids.Add(new PhotoId { ID = id, uID = uid });
                        //Console.WriteLine(m1.Value);
                        m1 = m1.NextMatch();
                    }



                    Regex pattern = new Regex(@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*");
                    // Match the regular expression pattern against a text string.
                    Match m = pattern.Match(json);
                    while (m.Success)
                    {
                        download_F_U(m.Value, user);
                        //do things with your matching text 
                        m = m.NextMatch();
                    }

                }
                else { return; }
            }
            Console.ReadLine();
        }
        public static void download_F_U(string val, string user)
        {
            if (val.Substring(0, 6) == "ahttps")
            {
                val = val.Replace("ahttps", "https");

                string t = "jpg";
                string s = "e35/";
                int ss = 1000;

                int chk = val.IndexOf(".mp4");
                if (chk > -1)
                {
                    t = "mp4"; s = "_"; ss = 4000;
                }

                string mid = vericek(val,s, "_n."+t);
                string ur = "https" + vericek(val, "https", t) + t;
                 string fileName = @""+folder+"\\" + user + "\\" + user + "_" + mid + "." + t;
               

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFileAsync(new System.Uri(ur), fileName);
                   Console.WriteLine(val);
                   System.Threading.Thread.Sleep(ss);
                }

                //using (WebClient myWebClient = new WebClient())
                //{
                //    myStringWebResource = val;
                //    myWebClient.DownloadFile(myStringWebResource, fileName);
                //} 
            }







        }
        public static string vericek(string StrData, string StrBas, string StrSon)
        {
            try
            {
                int IntBas = StrData.IndexOf(StrBas) + StrBas.Length;
                int IntSon = StrData.IndexOf(StrSon, IntBas + 1);
                return StrData.Substring(IntBas, IntSon - IntBas);
            }
            catch
            {
                return "";
            }

        }
        static string rtJ(string u)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string source = "";
            try
            {
                source = wc.DownloadString(u);

                string json = source;
                json = json.Replace(@"""", "");
                json = json.Replace(@"\", "");
                json = json.Replace("standard_resolution: {url: https", "ahttps");

                source = json;

            }
            catch (Exception e)
            { }
            return source;
        }


    }
}
