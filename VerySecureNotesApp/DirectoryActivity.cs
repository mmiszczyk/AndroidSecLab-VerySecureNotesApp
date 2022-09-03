using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Net.Http.SslCertificate;

namespace VerySecureNotesApp
{
    [Activity]
    internal class DirectoryActivity : Activity
    {
        private NoteDirectory? dir;
        private string?        dirname;
        private bool           started = false;
        private  const string  placeholder = "SELECT A NOTE";

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_dir);

            if(Intent != null)  dirname = Intent.GetStringExtra("directory");
            if(dirname != null) dir = new NoteDirectory(dirname);
            ConfigureSpinner();
            ConfigureNewNoteButton();
            ConfigureDeleteButton();
        }

        private void ConfigureDeleteButton()
        {
            var but = FindViewById<Button>(Resource.Id.button2);
            if (but == null) return;
            but.Click += (s, e) => {
                if (dir != null) dir.Delete();
                Finish();
            };
        }

        private void ConfigureNewNoteButton()
        {
            var but = FindViewById<Button>(Resource.Id.button1);
            if (but == null) return;
            but.Click += (s, e) => {
                if (dir == null) return;
                var edit = FindViewById<EditText>(Resource.Id.editText1);
                if (edit == null) return;
                var text = edit.Text;
                if (text == null) return;
                StartNoteActivity(dir.AddNote(text).Name); 
            };
        }

        private void ConfigureSpinner()
        {
            if (dir == null) return;
            var spin = FindViewById<Spinner>(Resource.Id.spinner1);
            if (spin == null) return;
            var notes = from n in dir.Notes select n;
            spin.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem,
                                                    new string[] { placeholder }.Concat(from n in notes select n.Name).ToArray());
            spin.Clickable = true;
            spin.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(OnSelectNote);
        }

        private void OnSelectNote(object? sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (sender == null) return;
            var item = ((Spinner)sender).GetItemAtPosition(e.Position);
            if (item == null) return;
            var noteName = item.ToString();
            if (noteName == null) return;
            if (noteName == placeholder) return;
            StartNoteActivity(noteName);
        }

        private void StartNoteActivity(string noteName)
        {
            if (dir == null) return;
            var i = new Intent(this, typeof(NoteActivity));
            i.PutExtra("note", noteName);
            StartActivity(i);
        }
    }
}
