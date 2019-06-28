using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace StudentRepApp
{
    [Serializable]
    public class Feedback
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int? UpVotes { get; set; }
        public int? DownVotes { get; set; }
        public byte Show { get; set; }
        public string StatusDescription { get; set; }
        public int? Priority { get; set; }
        public int? FlaggedInappropriate { get; set; }

    }
    public class DBQuery
    {
        public static IList<Feedback> feedback { get; private set; }
        public static void AllComplaints()
        {
            //string json = new WebClient().DownloadString("https://192.168.206.157:5002/api/issues");
            //feedback = JsonConvert.DeserializeObject<List<Feedback>>(json);
        }
    }
}