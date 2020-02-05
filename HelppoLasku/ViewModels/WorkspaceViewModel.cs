using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HelppoLasku.ViewModels
{
    public class WorkspaceViewModel : ViewModelBase
    {
        WorkspaceControlViewModel control;

        public WorkspaceViewModel(WorkspaceControlViewModel control, ViewModelBase content)
        {
            this.control = control;

            if (content.GetType() == typeof(DataViewModel) || content.GetType().IsSubclassOf(typeof(DataViewModel)))
            {
                content.PropertyChanged += OnContentPropertyChanged;
            }

            Content = content;
        }

        public ViewModelBase Content { get; private set; }

        public EventHandler OnClose;

        public CommandViewModel Close => new CommandViewModel("Sulje", () =>
        {
            OnClose?.Invoke(this, EventArgs.Empty);
            Dispose();
            control.Remove(this);  
        });

        void OnContentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EditEnabled" && (Content as DataViewModel).EditEnabled == false)
                Close.Command.Execute(null);
        }

        protected override void OnDispose()
        {
            if (Content != null)
                Content.PropertyChanged -= OnContentPropertyChanged;

            base.OnDispose();
        }

    }
}
