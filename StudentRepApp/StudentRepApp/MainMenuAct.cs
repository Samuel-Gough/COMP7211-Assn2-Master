using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StudentRepApp
{
    [Activity(Label = "MainMenuAct")]
    public class MainMenuAct : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.viewcomplaints).Click += ViewComplaintsClick;
            FindViewById<Button>(Resource.Id.newcomlaints).Click += NewComplaintsClick;
            FindViewById<Button>(Resource.Id.email).Click += EmailClick;
            FindViewById<Button>(Resource.Id.termsofservice).Click += TermsClick;
        }

        void TermsClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TermsOfServiceAct));
            StartActivity(intent);
        }

        void EmailClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EmailComplaintAct));
            StartActivity(intent);
        }

        void NewComplaintsClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(NewComplaintAct));
            StartActivity(intent);
        }

        void ViewComplaintsClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ViewComplaintAct));
            StartActivity(intent);
        }
    }
    
}