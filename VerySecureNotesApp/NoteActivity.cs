using Android.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Net.Http.SslCertificate;

namespace VerySecureNotesApp
{
    [Activity]
    internal class NoteActivity : Activity
    {
        private string? notename;
        private Note?   note;
        private const string TAG = "NOTE_ACTIVITY";

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_note);

            if (Intent != null)   notename = Intent.GetStringExtra("note");
            if (notename != null) note = new Note(notename);
            ConfigureContents();
            ConfigureSaveButton();
            ConfigureDeleteButton();
        }

        private void ConfigureSaveButton()
        {
            var but = FindViewById<Button>(Resource.Id.button1);
            if (but == null) return;
            but.Click += (s, e) => {
                var txt = FindViewById<EditText>(Resource.Id.editText2);
                if ((txt == null) || (txt.Text == null) || (note == null)) return;
                Log.Debug(TAG, String.Format("Saving note {0} with contents {1}", note.Name, txt));
                note.Content = txt.Text;
                Log.Debug(TAG, String.Format("Note content is now {0}", note.Content));
            };
        }

        private void ConfigureDeleteButton()
        {
            var but = FindViewById<Button>(Resource.Id.button2);
            if (but == null) return;
            but.Click += (s, e) => {
                if (note != null) note.Delete();
                Finish();
            };
        }

        private void ConfigureContents()
        {
            if (note == null) return;
            var txt = FindViewById<EditText>(Resource.Id.editText2);
            if(txt == null) return;
            txt.Text = note.Content;
        }
    }
}
