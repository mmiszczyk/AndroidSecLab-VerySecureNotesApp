using Android.Util;
using Java.Lang;
using Java.Nio.FileNio.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using String = System.String;

namespace VerySecureNotesApp
{
    public class Note
    {
        public readonly string Filepath;
        public readonly string Name;
        public const string TAG = "NOTE";
        public string Content
        {
            get {
                try
                {
                    return File.ReadAllText(Filepath);
                }
                catch
                {
                    return "";
                }
            }
            set => File.WriteAllText(Filepath, value);
        }

        public Note(string filename)
        {
            this.Name     = filename;
            this.Filepath = Utils.FullPath(filename);
            Log.Debug(TAG, String.Format("Creating a note named {0}, in location {1}", Name, Filepath));
        }

        public void Delete() => File.Delete(Filepath);

    }

    public class NoteDirectory
    {
        public readonly string  Name;
        private readonly string Filepath;

        public static NoteDirectory[] Directories
        {
            get => (from f in Directory.EnumerateDirectories(Utils.InternalDir)
                    where !Path.GetFileName(f).StartsWith(".")
                    select new NoteDirectory(Path.GetFileName(f))).ToArray();
        }

        public Note[] Notes
        {
            get => (from f in Directory.EnumerateFiles(Filepath) select new Note(Path.Combine(Name, Path.GetFileName(f)))).ToArray();
        }
        
        public NoteDirectory(string name)
        {
            Name = name;
            Filepath = Utils.FullPath(name);
            if(!Directory.Exists(Filepath)) Directory.CreateDirectory(Filepath);
        }
        public Note AddNote(string name)
        {
            return new Note(Path.Combine(Name, name));
        }

        public void Delete() => Runtime.GetRuntime()?.Exec(new string[] { "/system/bin/sh", "-c", string.Format("rm -rf {0}", Filepath) });
        public static void Delete(string name) => Runtime.GetRuntime()?.Exec(new string[] { "/system/bin/sh", "-c", string.Format("rm -rf {0}", Utils.FullPath(name)) });

    }

    internal class Utils
    {
        public static String InternalDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
       public static String FullPath(string p) => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), p);
    }

}
