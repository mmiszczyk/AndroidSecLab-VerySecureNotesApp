using Android.Content;
using Android.Widget;

namespace VerySecureNotesApp
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private const string placeholder = "SELECT A DIRECTORY";
        private bool started = false;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            ConfigureSpinner();
            ConfigureButton();
        }

        private void ConfigureButton()
        {
            var but = FindViewById<Button>(Resource.Id.button1);
            if (but == null) return;
            but.Click += (s, e) => {
                var edit = FindViewById<EditText>(Resource.Id.editText1);
                if(edit == null) return;
                var text = edit.Text;
                if (text == null) return;
                StartDirectoryActivity(text);
            };
        }

        private void ConfigureSpinner()
        {
            var spin = FindViewById<Spinner>(Resource.Id.spinner1);
            if (spin == null) return;
            var dirs = from d in NoteDirectory.Directories select d;
            spin.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem,
                                                    new string[] { placeholder }.Concat(from d in dirs select d.Name).ToArray());
            spin.Clickable = true;
            spin.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(OnSelectDirectory);
        }

        private void OnSelectDirectory(object? sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (sender == null) return;
            var item = ((Spinner)sender).GetItemAtPosition(e.Position);
            if (item == null) return;
            var dirName = item.ToString();
            if (dirName == null) return;
            if (dirName == placeholder) return;
            StartDirectoryActivity(dirName);
        }

        private void StartDirectoryActivity(string dirName)
        {
            var i = new Intent(this, typeof(DirectoryActivity));
            i.PutExtra("directory", dirName);
            StartActivity(i);
        }
    }
}