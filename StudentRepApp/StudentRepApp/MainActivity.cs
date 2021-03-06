﻿using System;
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
            SetContentView(Resource.Layout.LoginPage);

            FindViewById<Button>(Resource.Id.studlogin).Click += LoginClick;
           
        }

        void LoginClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainMenuAct));
            StartActivity(intent);
        }

       
    }
}