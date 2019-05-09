using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;

namespace StudentRepApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
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