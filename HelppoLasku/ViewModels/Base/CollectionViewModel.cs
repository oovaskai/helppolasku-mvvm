using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using HelppoLasku.DataAccess;

namespace HelppoLasku.ViewModels
{
    public abstract class CollectionViewModel : ViewModelBase
    {
        ObservableCollection<DataModel> sourceCollection;

        protected CollectionViewModel() { }

        protected CollectionViewModel(List<DataModel> models)
        {
            LoadItems(models);
        }

        protected CollectionViewModel(ObservableCollection<DataModel> source)
        {
            sourceCollection = source;
            sourceCollection.CollectionChanged += OnSourceCollectionChanged;
            LoadItems(sourceCollection);
        }

        #region Items

        ObservableCollection<DataViewModel> items;

        public ObservableCollection<DataViewModel> Items
        {
            get
            {
                if (items == null)
                    items = new ObservableCollection<DataViewModel>();
                return items;
            }
        }

        public virtual void LoadItems(ObservableCollection<DataModel> source)
        {
            LoadItems(source.ToList());
        }

        public virtual void LoadItems(List<DataModel> models)
        {
            List<DataViewModel> viewmodels = (from model in models select NewItem(model)).ToList();
            items = new ObservableCollection<DataViewModel>(viewmodels);
        }

        public abstract DataViewModel NewItem(DataModel model);

        public virtual void RemoveItem(DataViewModel viewmodel)
        {
            if (SelectedItem == viewmodel)
                SelectedItem = null;

            Items.Remove(viewmodel);
        }

        public virtual void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
                foreach (DataModel model in e.NewItems)
                    Items.Add(NewItem(model));

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (DataModel model in e.OldItems)
                {
                    DataViewModel viewmodel = FindByID(model.ID);
                    if (viewmodel != null)
                        RemoveItem(viewmodel);
                }
            }
        }

        public virtual DataViewModel FindByID(string id)
        {
            foreach (DataViewModel item in Items)
                if (item.Model.ID == id)
                    return item;
            return null;
        }

        #endregion

        #region SelectedItem

        DataViewModel selectedItem;

        public virtual DataViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {   
                ViewModelBase oldSelection = selectedItem;
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
                RaisePropertyChanged("SelectedIndex");
                SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(selectedItem, oldSelection));
            }
        }

        public virtual int SelectedIndex => selectedItem == null ? -1 : Items.IndexOf(selectedItem);

        public EventHandler<SelectionChangedEventArgs> SelectionChanged;

        #endregion

        #region Commands

        CollectionCommandsViewModel commands;
        public CollectionCommandsViewModel Commands
        {
            get
            {
                if (commands == null)
                    commands = new CollectionCommandsViewModel(OnNew, CanNew, OnEdit, CanEdit, OnCopy, CanCopy, OnDelete, CanDelete);
                return commands;
            }
        }

        public virtual void OnNew() { }

        public virtual bool CanNew()
            => true;

        public virtual void OnEdit() { }

        public virtual bool CanEdit()
            => SelectedItem != null;

        public virtual void OnCopy() { }

        public virtual bool CanCopy()
            => SelectedItem != null;

        public virtual void OnDelete() { }

        public virtual bool CanDelete()
            => SelectedItem != null;

        #endregion

        #region IDispose

        protected override void OnDispose()
        {
            if (sourceCollection != null)
                sourceCollection.CollectionChanged -= OnSourceCollectionChanged;

            if (Items != null)
            {
                foreach (ViewModelBase item in Items)
                    item.Dispose();

                Items.Clear();
            }
            base.OnDispose();
        }

        #endregion
    }

    #region SelectionChangedEventArgs

    public class SelectionChangedEventArgs : EventArgs
    {
        public SelectionChangedEventArgs(ViewModelBase newSelection, ViewModelBase oldSelection)
        {
            NewSelection = newSelection;
            OldSelection = oldSelection;
        }

        public ViewModelBase NewSelection { get; private set; }
        public ViewModelBase OldSelection { get; private set; }
    }

    #endregion
}
