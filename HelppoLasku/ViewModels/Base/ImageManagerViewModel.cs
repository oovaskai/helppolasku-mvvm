using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace HelppoLasku.ViewModels
{
    public class ImageManagerViewModel : FileManagerViewModel
    {
        public ImageManagerViewModel(string path, string file) : base(path, file)
        {
        }

        #region Properties

        public string DefaultFileName { get; set; }

        public FileViewModel Image
        {
            get
            {
                if (Files.Count > 0)
                    return Files[0];
                return null;
            }
        }

        public Visibility DragTextVisibility => Files.Count > 0 ? Visibility.Collapsed : Visibility.Visible;

        public override void AddFile(string filepath)
        {
            if (Files.Count > 0)
                Delete(Files[0]);

            base.AddFile(filepath);
            RaisePropertyChanged("Image");
            RaisePropertyChanged("DragTextVisibility");
        }

        #endregion

        #region Methods

        public override void Delete(FileViewModel file)
        {
            base.Delete(file);
            RaisePropertyChanged("Image");
            RaisePropertyChanged("DragTextVisibility");
        }

        public override string Update()
        {
            if (Files.Count <= 0)
                return null;

            string file = Files[0].FileName;

            if (!string.IsNullOrEmpty(DefaultFileName))
                file = DefaultFileName + "." + Files[0].Extension;

            string newFilePath = Path + @"\" + file;

            if (Files[0].IsNew)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                try
                {
                    File.Copy(Files[0].FilePath, newFilePath, true);
                }
                catch (Exception e)
                {
                    Views.MainWindow.Message(e.Message, "Virhe", MessageBoxImage.Error);
                }
            }
            return file;
        }

        #endregion
    }
}
