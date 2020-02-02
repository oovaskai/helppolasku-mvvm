using System;
using System.ComponentModel;

namespace HelppoLasku.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase()
        {
            IsEnabled = true;
        }

        #region Properties

        string displayName;

        public virtual string DisplayName
        {
            get { return displayName; }
            set
            {
                if (displayName != value)
                {
                    displayName = value;
                    RaisePropertyChanged("DisplayName");
                }
            }
        }

        bool isEnabled;

        public virtual bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    RaisePropertyChanged("IsEnabled");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {

        }

        #endregion
    }
}
