using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace HelppoLasku.Views.Controls
{
    public partial class SortableListView : ListView
    {
        GridViewColumnHeader _lastHeaderClicked = null;

        public SortableListView()
        {
            InitializeComponent();
        }

        private void ListViewColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked == null)
                return;

            if (headerClicked.Role == GridViewColumnHeaderRole.Padding)
                return;

            var sortingColumn = (headerClicked.Column.DisplayMemberBinding as Binding)?.Path?.Path 
                ?? (headerClicked.Column.CellTemplate.LoadContent() as TextBlock).GetBindingExpression(TextBlock.TextProperty).ParentBinding?.Path?.Path;

            if (sortingColumn == null)
                return;

            var direction = ApplySort(Items, sortingColumn);

            if (direction == ListSortDirection.Ascending)
            {
                headerClicked.Column.HeaderTemplate =
                    Resources["HeaderTemplateArrowUp"] as DataTemplate;
            }
            else
            {
                headerClicked.Column.HeaderTemplate =
                    Resources["HeaderTemplateArrowDown"] as DataTemplate;
            }

            // Remove arrow from previously sorted header
            if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
            {
                _lastHeaderClicked.Column.HeaderTemplate =
                    Resources["HeaderTemplateDefault"] as DataTemplate;
            }

            _lastHeaderClicked = headerClicked;
        }


        public static ListSortDirection ApplySort(ICollectionView view, string propertyName)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                SortDescription currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    if (currentSort.Direction == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }
                view.SortDescriptions.Clear();
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }
            return direction;
        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource as TextBlock) != null)
            {
                ViewModels.CollectionViewModel collection = (sender as ListView).DataContext as ViewModels.CollectionViewModel;
                collection.SelectedItem = (e.OriginalSource as TextBlock).DataContext as ViewModels.DataViewModel;

                if (collection.SelectedItem != null)
                    collection.OnEdit();
            }           
        }
    }

}
