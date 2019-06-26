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
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

// NOTE FOR JACOB : Please change the smtp email address after you've finished with this.

namespace StudentRepApp
{

    public class Contact
    {
        public int      id            { get; set; }
        public string   name          { get; set; }
        public string   role          { get; set; }
        public string   department    { get; set; }
        public string   emailAddress  { get; set; }
    }

    [Activity(Label = "EmailComplaintAct")]
    public class EmailComplaintAct : Activity
    {
        public List<string> emailTarget = new List<string>();
        public List<string> emailAddress = new List<string>();
        public static List<Contact> contacts = new List<Contact>();

        public int selectionIndex = 0;
        public string msgTitle = "";
        public string msgContent = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmailComplaints);

            // Add default selection
            emailTarget.Add("Select a contact");
            emailAddress.Add("NA");

            // Load database
            string json = new WebClient().DownloadString("http://192.168.206.157:5002/api/contact");
            contacts = JsonConvert.DeserializeObject<List<Contact>>(json);

            // Populate lists
            foreach (var c in contacts) {
                emailTarget.Add(c.role);
                emailAddress.Add(c.emailAddress);
            }

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
                if (SendEmail()) {
                    Finish();
                }
            };
        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e) {
            Spinner spinner = (Spinner)sender;
            selectionIndex = e.Position;    // Get index into the list of contacts
        }

        private bool SendEmail()
        {
            try {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("mxsacnt@gmail.com");
                mail.To.Add(emailAddress[selectionIndex]);
                mail.Subject = msgTitle;
                mail.Body = msgContent;

                smtpServer.Port = 587;
                smtpServer.Host = "smtp.gmail.com";
                smtpServer.EnableSsl = true;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new NetworkCredential("mxsacnt@gmail.com", "xx11333377xx");

                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex) {
                string failMsg = "Failed to send : " + ex.Message;
                Toast.MakeText(this, failMsg, ToastLength.Long).Show();
                return false;
            }
        }
    }
}