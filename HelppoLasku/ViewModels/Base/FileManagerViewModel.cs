using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace HelppoLasku.ViewModels
{
    public class FileManagerViewModel : ViewModelBase
    {
        List<FileViewModel> deletedFiles;

        public FileManagerViewModel(string path)
        {
            Path = path;
            deletedFiles = new List<FileViewModel>();

            if (Files == null)
                Files = new ObservableCollection<FileViewModel>();

            if (FileTypes == null)
                FileTypes = new List<string>();

            New = new CommandViewModel("Lisää tiedosto", OnGetFile, CanGetFile);
        }

        public FileManagerViewModel(string path, string filelist) : this(path)
        {
            Files = new ObservableCollection<FileViewModel>();
            if (!string.IsNullOrEmpty(filelist))
            {
                string[] files = filelist.Split(';');
                foreach (string file in files)
                    if (!string.IsNullOrEmpty(file))
                        Files.Add(new FileViewModel(path + @"\" + file, this));
            }
        }

        #region Properties

        public List<string> FileTypes { get; set; }

        string filter;

        public string Filter
        {
            get
            {
                if (filter == null)
                    return DefaultFilter;
                return filter;
            }
            set => filter = value;
        }

        string DefaultFilter
        {
            get
            {
                if (FileTypes.Count <= 0)
                    return "Kaikki tiedostot (*.*)|*.*";

                string filter = "Tuetut muodot (";
                for (int i = 0; i < 2; i++)
                {
                    foreach (string type in FileTypes)
                    {
                        if (type != FileTypes[0])
                            filter += ';';
                        filter += '*' + type;
                    }
                    if (i == 0)
                        filter += ")|";
                }
                return filter;
            }
        }

        public ObservableCollection<FileViewModel> Files { get; private set; }

        public string Path { get; private set; }

        public Func<string> FileName { get; set; }

        #endregion

        #region Methods

        public virtual void Delete(FileViewModel file)
        {
            if (Files.Contains(file))
            {
                Files.Remove(file);

                if (!file.IsNew)
                    deletedFiles.Add(file);
            }
        }

        public virtual string Update()
        {
            foreach (FileViewModel file in deletedFiles)
                file.DeleteFile();

            string files = "";
            foreach (FileViewModel file in Files)
            {
                if (file.IsNew)
                {
                    string count = "";
                    if (Files.Count > 1)
                        count = "_" + (Files.IndexOf(file) + 1).ToString();

                    string newName = FileName() + count + '.' + file.Extension;

                    file.CopyTo(Path + @"\" + newName);
                    files += newName + ';';
                }
                else
                    files += file.FileName + ';';
            }
            return files;
        }

        #endregion

        #region NewCommand

        public CommandViewModel New { get; private set; }

        public virtual void OnGetFile()
        {
            string[] files = Views.MainWindow.OpenFileDialog(Filter, null, true);

            if (files != null)
                foreach (string file in files)
                    AddFile(file);
        }

        public virtual bool CanGetFile()
            => true;

        public virtual void AddFile(string filepath)
        {
            if (!string.IsNullOrEmpty(filepath))
            {
                if (FileTypes.Count > 0 && !FileTypes.Contains(GetFileExtension(filepath)))
                {
                    string filetypes = "";
                    foreach (string type in FileTypes)
                    {
                        if (type != FileTypes[0])
                            filetypes += ", ";

                        filetypes += type.Remove(0, 1);
                    }

                    Views.MainWindow.Message("Tiedostotyyppi " + GetFileExtension(filepath).Remove(0, 1) + " ei ole kelvollinen.\n\n" +
                        "Tuetut tiedostotyypit: " + filetypes, "Virhe", System.Windows.MessageBoxImage.Error);
                    return;
                }

                FileViewModel file = new FileViewModel(filepath, this);
                file.IsNew = true;
                Files.Add(file);
            }
        }

        public static string GetFileExtension(string file)
            => file.Remove(0, file.LastIndexOf('.'));


        #endregion
    }
}
