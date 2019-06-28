using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;


namespace StudentRepApp
{
    [Activity(Label = "ViewComplaintAct")]
    public class ViewComplaintAct : Activity
    {
        public static List<Feedback> feedback { get; set; }
        HttpClient _client = new HttpClient();
        LinearLayout horizontal;
        TextView loading;
        List<LinearLayout> layouts;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewComplaints);

            // Create your application here

            //Call method to get data from api
            Task.Run(() => GetData().Wait());


            Button backButton = FindViewById<Button>(Resource.Id.backtomain);
            backButton.Click += delegate
            {
                BackToMain();
            };

            // Old way of pulling down database 
            //string json = new WebClient().DownloadString("http://192.168.206.157:5002/api/issue");
            //feedback = JsonConvert.DeserializeObject<List<Feedback>>(json);

        }

        private void BackToMain()
        {
            Finish();
        }

        protected override void OnStart()
        {
            SetupView();
            base.OnStart();
            layouts = new List<LinearLayout>();
        }

        // called when coming back from single view to refresh the main view
        protected override void OnRestart()
        {
            base.OnRestart();
            foreach (LinearLayout x in layouts)
            {
                x.Visibility = ViewStates.Gone;
            }
            loading.Visibility = ViewStates.Visible;
            Task.Run(() => GetData().Wait());

        }

        // start single view intent when clicking on a title of an issue
        public void TitleClicked(View v)
        {
            var clickedView = JsonConvert.SerializeObject(feedback.Find(x => x.Id == v.Id));
            Intent intent = new Intent(this, typeof(SingleViewAct));
            intent.PutExtra("Feedback", clickedView);
            StartActivity(intent);

        }

        // populate the view with the title / upvotes and downvotes from api 
        public async void SetupView()
        {
            await Task.Delay(8000);
            loading = FindViewById<TextView>(Resource.Id.loadingtext);
            loading.Visibility = ViewStates.Invisible;

            // stop makes this run on the UI thread to avoid deadlocks
            RunOnUiThread(() =>
            {

                if (feedback != null)
                {
                    foreach (Feedback x in feedback)
                    {
                        LinearLayout content = FindViewById<LinearLayout>(Resource.Id.feedbackcontent);
                        horizontal = new LinearLayout(this);
                        horizontal.Orientation = Orientation.Horizontal;
                        horizontal.SetPadding(0, 15, 0, 15);

                        // add layout to list of layouts to clear on reload
                        layouts.Add(horizontal);

                        TextView feedTitle = new TextView(this);
                        feedTitle.Id = x.Id;
                        feedTitle.Text = x.Title;
                        feedTitle.SetMaxWidth(197);
                        feedTitle.LayoutParameters = new LinearLayout.LayoutParams(197, LinearLayout.LayoutParams.WrapContent, 0.6f);
                        feedTitle.Clickable = true;
                        feedTitle.SetPadding(0, 0, 5, 0);
                        feedTitle.Click += delegate
                        {
                            TitleClicked(feedTitle);
                        };

                        TextView upVotes = new TextView(this);
                        upVotes.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent, 0.2f);
                        upVotes.SetTextColor(Android.Graphics.Color.GreenYellow);
                        upVotes.Gravity = GravityFlags.Center;
                        upVotes.TextAlignment = TextAlignment.Center;
                        if (x.UpVotes == null)
                        {
                            x.UpVotes = 0;
                        }
                        upVotes.Text = x.UpVotes.ToString();

                        TextView downVotes = new TextView(this);
                        downVotes.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent, 0.2f);
                        downVotes.SetPadding(5, 0, 0, 0);
                        downVotes.SetTextColor(Android.Graphics.Color.Red);
                        downVotes.Gravity = GravityFlags.Center;
                        downVotes.TextAlignment = TextAlignment.Center;
                        if (x.DownVotes == null)
                        {
                            x.DownVotes = 0;
                        }
                        downVotes.Text = x.DownVotes.ToString();

                        horizontal.AddView(feedTitle);
                        horizontal.AddView(upVotes);
                        horizontal.AddView(downVotes);
                        content.AddView(horizontal);

                    }
                }
                else
                {
                    Toast.MakeText(Application.Context, "Unable to connect to API please try again later", ToastLength.Short).Show();
                }
            });

        }

        // call an async task to get the data down from the api
        public async Task<List<Feedback>> GetData()
        {
            //HttpClient _client = new HttpClient();
            Uri api = new Uri("http://192.168.206.157:5002/api/issue");
            var response = await _client.GetAsync(api);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                feedback = JsonConvert.DeserializeObject<List<Feedback>>(content);
            }
            if (!response.IsSuccessStatusCode)
            {
                feedback = null;
            }
            return feedback;

        }
    }
}