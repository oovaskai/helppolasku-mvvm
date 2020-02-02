using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HelppoLasku.ViewModels
{
    public class FileViewModel : ViewModelBase
    {
        FileManagerViewModel fileManager;

        public FileViewModel(string filePath)
        {
            FilePath = filePath;

            OpenFile = new CommandViewModel("Avaa tiedosto", OnOpenFile, CanOpenFile);
            OpenFolder = new CommandViewModel("Avaa tiedostosijainti", OnOpenFolder, CanOpenFolder);
            Copy = new CommandViewModel("Kopioi leikepöydälle", OnCopyFile, CanCopyFile);
            Delete = new CommandViewModel("Poista", OnDelete, CanDelete);
        }

        public FileViewModel(string filePath, FileManagerViewModel manager) : this(filePath)
        {
            fileManager = manager;
        }

        #region Properties

        string filepath;

        public string FilePath
        {
            get => string.IsNullOrEmpty(filepath) ? "Ei asetettu" : filepath;
            private set
            {
                if (filepath != value)
                {
                    filepath = value;
                    RaisePropertyChanged("FilePath");
                    RaisePropertyChanged("File");
                    RaisePropertyChanged("Path");
                    RaisePropertyChanged("Extension");
                }
            }
        }

        public string Path => string.IsNullOrEmpty(filepath) ? "Ei asetettu" : filepath.Remove(filepath.LastIndexOf(@"\"));

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(filepath))
                    return "Ei asetettu";

                string file = filepath.Remove(0, filepath.LastIndexOf(@"\") + 1);

                return file;
            }
        }

        public string Extension => string.IsNullOrEmpty(filepath) ? "" : filepath.Remove(0, filepath.LastIndexOf('.') + 1);

        public bool IsNew { get; set; }

        public override string DisplayName
        {
            get => IsNew ? FileName + "*" : FileName;
        }


        #endregion

        #region Methods

        public void DeleteFile()
        {
            Views.MainWindow.Message("Poistetaan: " + FilePath);

            try
            {
                Dispose();
                File.Delete(FilePath);
            }
            catch (Exception e)
            {
                Views.MainWindow.Message(e.Message);
            }
        }

        public void CopyTo(string target)
        {
            string path = target.Remove(target.LastIndexOf(@"\"));
            Directory.CreateDirectory(path);

            if (File.Exists(target))
            {
                string filename = target.Remove(target.LastIndexOf('.'));
                string ex = target.Remove(0, target.LastIndexOf('.'));

                int i = 1;

                while (File.Exists(filename + "_" + i + ex))
                {
                    i++;
                }

                target = filename + "_" + i + ex;
            }
            File.Copy(FilePath, target);

            Views.MainWindow.Message("Kopioidaan: " + FilePath + "-> kohteeseen: " + target);
        }

        #endregion

        #region OpenFileCommand

        public CommandViewModel OpenFile { get; private set; }

        public virtual void OnOpenFile()
        {
            try
            {
                System.Diagnostics.Process.Start(FilePath);
            }
            catch (Exception e)
            {
                Views.MainWindow.Message(e.Message);
            }
        }

        public virtual bool CanOpenFile()
            => !string.IsNullOrEmpty(filepath);

        #endregion

        #region OpenFolderCommand

        public CommandViewModel OpenFolder { get; private set; }

        public virtual void OnOpenFolder()
        {
            string argument = "/select, \"" + FilePath + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        public virtual bool CanOpenFolder()
            => CanOpenFile();

        #endregion

        #region CopyFileCommand

        public CommandViewModel Copy { get; private set; }

        public virtual void OnCopyFile()
        {
            System.Windows.Clipboard.SetFileDropList(new System.Collections.Specialized.StringCollection { FilePath });
        }

        public virtual bool CanCopyFile()
            => CanOpenFile();

        #endregion

        #region DeleteFileCommand

        public CommandViewModel Delete { get; private set; }

        public virtual void OnDelete()
        {
            if (fileManager != null)
                fileManager.Delete(this);
        }

        public virtual bool CanDelete()
            => CanOpenFile();

        #endregion
    }
}
