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
    public partial class SearchTextBox : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SearchTextBox), null);

        public static readonly DependencyProperty ItemsSourceProperty = 
            DependencyProperty.Register("ItemsSource", typeof(CollectionView), typeof(SearchTextBox), null);

        public static readonly DependencyProperty DisplayItemPathProperty =
            DependencyProperty.Register("DisplayItemPath", typeof(string), typeof(SearchTextBox), null);

        public static readonly DependencyProperty SortItemPathProperty =
            DependencyProperty.Register("SortItemPath", typeof(string), typeof(SearchTextBox), null);

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(SearchTextBox), null);

        public static readonly DependencyProperty IsDefaultProperty =
            DependencyProperty.Register("IsDefault", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false));

        public SearchTextBox()
        {
            InitializeComponent();
        }

        #region Properties

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool IsDefault
        {
            get { return (bool)GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        public string DisplayItemPath
        {
            get { return (string)GetValue(DisplayItemPathProperty); }
            set { SetValue(DisplayItemPathProperty, value); }
        }

        public string SortItemPath
        {
            get { return (string)GetValue(SortItemPathProperty); }
            set { SetValue(SortItemPathProperty, value); }
        }

        public CollectionView ItemsSource
        {
            get { return (CollectionView)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private object selectedItem;

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        #endregion

        #region Event Handling

        public event PropertyChangedEventHandler PropertyChanged;

        private void SearchTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (ItemsSource != null)
                ItemsSource.Filter = ItemFilter;

            if (SortItemPath != null && ItemsSource != null)
                ItemsSource.SortDescriptions.Add(new SortDescription(SortItemPath, ListSortDirection.Ascending));
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            popUp.IsOpen = string.IsNullOrEmpty(Text) ? false : true;

            if (ItemsSource != null)
                ItemsSource.Refresh();
        }

        private void ItemListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
                selectedItem = e.AddedItems[0];
            else
            {
                selectedItem = null;
                SelectedItem = null;
            }
        }

        private bool ItemFilter(object item)
        {
            string value = item.GetType().GetProperty(DisplayItemPath).GetValue(item).ToString();
            if (string.IsNullOrEmpty(Text) || (value != null && value.IndexOf(Text, StringComparison.OrdinalIgnoreCase) >= 0))
                return true;

            return false;
        }

        #endregion

        #region User Input

        private void FilterTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                popUp.IsOpen = true;
                itemListBox.Focus();
            }

            if ((e.Key == Key.Escape || e.Key == Key.Tab) && popUp.IsOpen)
            {
                popUp.IsOpen = false;
            }
        }

        private void ItemListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && itemListBox.SelectedIndex == 0)
            {
                selectedItem = null;
                SelectedItem = null;
                filterTextBox.Focus();
                filterTextBox.SelectAll();
            }
                

            if (e.Key == Key.Tab || e.Key == Key.Return)
            {
                if (selectedItem == null)
                    popUp.IsOpen = false;
                else
                    SelectItem();
            }

            if (e.Key == Key.Escape)
            {
                selectedItem = null;
                SelectedItem = null;
                popUp.IsOpen = false;
                filterTextBox.Focus();
            }
        }

        private void PopUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!popUp.IsOpen)
                popUp.IsOpen = true;
        }

        private void ItemListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectItem();
        }

        private void SelectItem()
        {
            SelectedItem = selectedItem;
            filterTextBox.Focus();
            selectedItem = null;
            popUp.IsOpen = false;
        }

        #endregion
    }
}
