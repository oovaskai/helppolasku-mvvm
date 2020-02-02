using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class ProductGroupListViewModel : CollectionViewModel
    {
        public ProductGroupListViewModel(ObservableCollection<DataModel> source) : base(source)
        {
            DisplayName = "Tuoteryhmät";
        }

        CollectionView View; 

        ProductGroupViewModel editGroup;
        public ProductGroupViewModel EditGroup
        {
            get { return editGroup; }
            set
            {
                if (editGroup != value)
                {
                    editGroup = value;
                    RaisePropertyChanged("EditGroup");
                }
            }
        }

        public override void LoadItems(ObservableCollection<DataModel> source)
        {
            base.LoadItems(source);

            ProductGroupViewModel allGroup = NewItem(ProductGroup.All) as ProductGroupViewModel;
            Items.Insert(0, allGroup);
            SelectedItem = allGroup;

            View = (CollectionView)CollectionViewSource.GetDefaultView(Items);
        }

        public override DataViewModel NewItem(DataModel model)
        {
            ProductGroupViewModel viewmodel = new ProductGroupViewModel(model as ProductGroup);
            return viewmodel;
        }

        public new ProductGroupViewModel SelectedItem
        {
            get => base.SelectedItem as ProductGroupViewModel;
            set => base.SelectedItem = value;
        }

        public new ProductGroupViewModel FindByID(string id) => base.FindByID(id) as ProductGroupViewModel;

        #region Commands

        #region CollectionCommands

        public override void OnNew()
        {
            EditGroup = new ProductGroupViewModel(new ProductGroup { Index = Items.Count });
            EditGroup.DisplayName = "Uusi tuoteryhmä";
            EditGroup.EditEnabled = true;
        }

        public override void OnEdit()
        {
            EditGroup = new ProductGroupViewModel(new ProductGroup(SelectedItem.Model));
            EditGroup.DisplayName = "Nimeä uudelleen";
            EditGroup.EditEnabled = true;
        }

        public override bool CanEdit()
            => SelectedItem != null && SelectedItem.Model != ProductGroup.All;

        public override void OnDelete()
        {
            if (Views.MainWindow.ConfirmMessage("Haluatko varmasti poistaa tuoteryhmän " + SelectedItem.Name + " ?", "Varoitus", System.Windows.MessageBoxImage.Question))
            {
                int index = SelectedItem.Index;
                SelectedItem.Model.Delete();
                SelectedItem = Items[0] as ProductGroupViewModel;
                foreach (ProductGroupViewModel group in Items)
                {
                    if(group.Index > index)
                        group.Index--;
                }
            }
        }

        public override bool CanDelete()
            => SelectedItem != null && SelectedItem.Model != ProductGroup.All;

        #endregion

        #region UpCommand

        public CommandViewModel UpCommand => new CommandViewModel("Ylös", "Järjestys ylös", OnMoveUp, CanMoveUp);

        void OnMoveUp()
            => UpdateGroups(SelectedItem.Index, SelectedItem.Index - 1);


        bool CanMoveUp()
            => SelectedItem != null && SelectedItem.Index != 0 ? SelectedItem.Index > 1 : false;

        #endregion

        #region DownCommand

        public CommandViewModel DownCommand => new CommandViewModel("Alas", "Järjestys alas", OnMoveDown, CanMoveDown);

        void OnMoveDown()
            => UpdateGroups(SelectedItem.Index, SelectedItem.Index + 1);

        bool CanMoveDown()
            => SelectedItem != null && SelectedItem.Index != 0 ? SelectedItem.Index < Items.Count -1 : false;

        #endregion

        #region Methods

        void MoveGroup(int steps)
        {
            int oldIndex = SelectedItem.Index;
            ProductGroupViewModel group = SelectedItem;
            int index = oldIndex + steps;

            if (index < 1)
                index = 1;
            if (index > Items.Count - 1)
                index = Items.Count - 1;

            //Views.MainWindow.Message(SelectedItem.Name + " " + oldIndex + " " + index);

            UpdateGroups(oldIndex, index);  
        }

        void UpdateGroups(int oldIndex, int newIndex)
        {
            foreach(ProductGroupViewModel group in Items)
            {
                if (group.Index == newIndex)
                    group.Index = oldIndex;

                else if (group.Index == oldIndex)
                    group.Index = newIndex;

                //Views.MainWindow.Message(group.Name + " " + group.Index);

                View.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));
                View.Refresh();
                group.Model.Save();
            }
        }

        #endregion // Methods

        #endregion // // Commands
    }
}
