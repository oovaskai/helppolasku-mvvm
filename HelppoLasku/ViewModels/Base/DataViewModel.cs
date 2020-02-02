using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using HelppoLasku.DataAccess;
using HelppoLasku.Validation;

namespace HelppoLasku.ViewModels
{
    public abstract class DataViewModel : ViewModelBase, IDataErrorInfo
    {
        protected DataViewModel(DataModel dataModel)
        {
            Model = dataModel ?? throw new ArgumentNullException("dataModel");
            Model.ModelChanged += OnModelChanged;
        }

        public DataModel Model { get; set; }

        public virtual void OnModelChanged(object sender, ModelChangedEventArgs e)
        {
            if (sender.Equals(Model))
            {
                if (e.Type == ModelChangedEventArgs.EventType.Update && e.Properties != null)
                {
                    foreach (string property in e.Properties)
                        RaisePropertyChanged(property);
                }

                else if (e.Type == ModelChangedEventArgs.EventType.Delete)
                {
                    Dispose();
                }
            }
        }

        bool editEnabled;

        public virtual bool EditEnabled
        {
            get => editEnabled;
            set
            {
                if (editEnabled != value)
                {
                    editEnabled = value;
                    RaisePropertyChanged("EditEnabled");
                }
            }
        }

        EditCommandsViewModel commands;

        public EditCommandsViewModel Commands
        {
            get
            {
                if (commands == null)
                    commands = new EditCommandsViewModel(OnSave, CanSave, OnCancel, CanCancel);
                return commands;
            }
        }

        public virtual void OnSave()
        {
            if (Error != null)
                throw new InvalidOperationException("Can not save invalid data");

            Model.Save();
            EditEnabled = false;
        }

        public virtual bool CanSave()
        {
            RaisePropertyChanged("Error");

            if (Error == null)
                return true;

            return false;
        }
            
        public virtual void OnCancel()
        {
            EditEnabled = false;
        }

        public virtual bool CanCancel()
            => true;

        protected override void OnDispose()
        {
            Model.ModelChanged -= OnModelChanged;
            base.OnDispose();
        }

        public DataValidator Validator { get; protected set; }

        public virtual string Error => Validator?.GetError();

        public virtual string this[string property] => Validator?.Validate(property);
    }
}
