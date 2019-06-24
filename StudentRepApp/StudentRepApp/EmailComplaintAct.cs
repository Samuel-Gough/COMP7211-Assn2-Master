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

using System.Net.Mail;
//using System.Net.Mime;

namespace StudentRepApp
{
    [Activity(Label = "EmailComplaintAct")]
    public class EmailComplaintAct : Activity
    {
        public List<string> emailTarget = new List<string>();
        public List<string> emailAddress = new List<string>();
        public int selectionIndex = 0;
        public string msgTitle = "";
        public string msgContent = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmailComplaints);
            // Create your application here
            
            // Make fake data for testing
            emailTarget.Add("Select a contact");
            emailTarget.Add("Comp6210 : Student Rep");
            emailTarget.Add("Comp7109 : Student Rep");
            emailTarget.Add("Comp6109 : Student Rep");
            emailTarget.Add("Comp7031 : Student Rep");
            emailTarget.Add("Comp7111 : Student Rep");

            emailAddress.Add("NA");
            emailAddress.Add("joesf.ms@gmail.com");
            emailAddress.Add("secondPerson@someEmail.com");
            emailAddress.Add("thirdPerson@someEmail.com");
            emailAddress.Add("fourthPerson@someEmail.com");
            emailAddress.Add("fifthPerson@someEmail.com");

            // Spinner / Drop down menu
            Spinner spinner = FindViewById<Spinner>(Resource.Id.contactSpinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SpinnerItemSelected);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, emailTarget);
            spinner.Adapter = adapter;

            // Message subject
            var msgSubjectText = FindViewById<EditText>(Resource.Id.msgSubjectText);
            msgSubjectText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => { msgTitle = e.Text.ToString(); };

            // Message entry
            var msgContentText = FindViewById<EditText>(Resource.Id.msgContentText);
            msgContentText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => { msgContent = e.Text.ToString(); };

            // Send button
            var sendButton = FindViewById<Button>(Resource.Id.sendButton);
            sendButton.Click += (o, e) => {

                // Check a target address has been selected
                if (selectionIndex == 0) {
                    Toast.MakeText(this, "Failed to send : Please select a recipient", ToastLength.Long).Show();
                    return;
                }

                // Check message title has been entered
                if (msgTitle.Length < 10) {
                    Toast.MakeText(this, "Failed to send : Message title must contain at least 10 characters", ToastLength.Long).Show();
                    return;
                }

                // Check message content has been entered
                if (msgContent.Length < 10) {
                    Toast.MakeText(this, "Failed to send : Message content must contain at least 10 characters", ToastLength.Long).Show();
                    return;
                }

                // Send email if clicked, then return to the main menu
                if (SendEmail())
                {
                    //FinishActivity(0);
                    Finish();
                }

                //string toast = "You pressed the button!";
                //Toast.MakeText(this, toast, ToastLength.Long).Show();

            };


        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e) {
            Spinner spinner = (Spinner)sender;
            selectionIndex = e.Position;
        }

        private bool SendEmail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("mxsacnt@gmail.com");
                //mail.To.Add("joesf.ms@gmail.com");
                mail.To.Add(emailAddress[selectionIndex]);
                //mail.Subject = height > width ? pt_msgSubject.Text : ls_msgSubject.Text;
                mail.Subject = msgTitle;
                mail.Body = msgContent;

                smtpServer.Port = 587;
                smtpServer.Host = "smtp.gmail.com";
                smtpServer.EnableSsl = true;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new System.Net.NetworkCredential("mxsacnt@gmail.com", "xx11333377xx");

                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                string failMsg = "Failed to send : " + ex.Message;
                Toast.MakeText(this, failMsg, ToastLength.Long).Show();
                //DisplayAlert("Failed", ex.Message, "OK");
                return false;
            }





        }

    }
}