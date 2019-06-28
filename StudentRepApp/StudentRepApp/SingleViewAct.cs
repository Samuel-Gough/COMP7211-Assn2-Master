using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    [Activity(Label = "SingleViewAct")]
    public class SingleViewAct : Activity
    {
        bool votedUp = false;
        bool votedDown = false;
        HttpClient _client = new HttpClient();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SingleFeedbackView);

            Feedback singleFeedback = JsonConvert.DeserializeObject<Feedback>(Intent.GetStringExtra("Feedback"));
            // Create your application here

            TextView heading = FindViewById<TextView>(Resource.Id.singleviewheading);
            TextView description = FindViewById<TextView>(Resource.Id.singleviewdescript);
            TextView upvote = FindViewById<TextView>(Resource.Id.svUpvote);
            TextView downvote = FindViewById<TextView>(Resource.Id.svDownvote);
            TextView upvoteT = FindViewById<TextView>(Resource.Id.upVoteTitle);
            TextView downvoteT = FindViewById<TextView>(Resource.Id.downVoteTitle);
            Button backbutton = FindViewById<Button>(Resource.Id.backbutton);

            backbutton.Click += delegate
            {
                onBackClick();
            };

            // Update text in view with object passed through intent
            heading.Text = singleFeedback.Title;
            description.Text = singleFeedback.Description;
            upvote.Text = singleFeedback.UpVotes.ToString();
            downvote.Text = singleFeedback.DownVotes.ToString();

            // handle upvote click
            upvote.Click += delegate
            {
                if(votedUp == false)
                {
                    UpVoteClicked(singleFeedback);
                }
                else
                {
                    Toast.MakeText(Application.Context, "You have already upvoted this issue", ToastLength.Short).Show();
                }


            };
            upvoteT.Click += delegate
            {
                if (votedUp == false)
                {
                    UpVoteClicked(singleFeedback);
                }
                else
                {
                    Toast.MakeText(Application.Context, "You have already upvoted this issue", ToastLength.Short).Show();
                }

            };

            // handle downvote click
            if (votedDown == false)
            {
                downvote.Click += delegate
                {
                    DownVoteClicked(singleFeedback);
                    votedDown = true;
                };
                downvoteT.Click += delegate
                {
                    DownVoteClicked(singleFeedback);
                    votedDown = true;
                };
            }
            else
            {
                Toast.MakeText(Application.Context, "You have already downvoted this issue", ToastLength.Short).Show();
            }
   
            void onBackClick()
            {
                Finish();
            }

        }

        // update database with updated upvote/downvote values
        public async void DownVoteClicked(Feedback feedback)
        {
            votedDown = true;
            string fullLink = $"http://192.168.206.157:5002/api/issue/" + feedback.Id;
            Uri api = new Uri(fullLink);
            
            if(feedback.DownVotes == null)
            {
                feedback.DownVotes = -1;
            }
            if(feedback.DownVotes != null)
            {
                feedback.DownVotes -= 1;
            }
            
            HttpContent toPush = new StringContent(JsonConvert.SerializeObject(feedback), Encoding.UTF8, "application/json");
            await _client.PutAsync(api, toPush);
            Toast.MakeText(Application.Context, "Tank You - DownVote Registered", ToastLength.Short).Show();
            Finish();
        }

        public async void UpVoteClicked(Feedback feedback)
        {
            votedUp = true;
            string fullLink = $"http://192.168.206.157:5002/api/issue/" + feedback.Id;
            Uri api = new Uri(fullLink);

            if (feedback.UpVotes == null)
            {
                feedback.UpVotes = 1;
            }
            if (feedback.UpVotes != null)
            {
                feedback.UpVotes += 1;
            }

            HttpContent toPush = new StringContent(JsonConvert.SerializeObject(feedback), Encoding.UTF8, "application/json");
            await _client.PutAsync(api, toPush);
            Toast.MakeText(Application.Context, "Tank You - Upvote Registered", ToastLength.Short).Show();
            Finish();
        }


    }

    
}