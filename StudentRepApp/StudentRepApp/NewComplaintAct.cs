﻿using System;
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
    [Activity(Label = "NewComplaintAct")]
    public class NewComplaintAct : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NewComplaints);

            FindViewById<Button>(Resource.Id.cancelcomplaint).Click += cancelclick;
            FindViewById<EditText>(Resource.Id.complaintbody);

            void cancelclick(object sender, EventArgs e)
            {/*
                Button cancelcomplaint = (Button)sender;
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                dialog.setMessage("Are you sure you want to cancel? Your post will be deleted.");
                */
                
            }
        }
    }
}