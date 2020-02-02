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

namespace HelppoLasku.Views.Controls
{
    /// <summary>
    /// Interaction logic for InputTextBox.xaml
    /// </summary>
    public partial class InputTextBox : TextBox
    {
        public InputTextBox()
        {
            InitializeComponent();
        }

        public enum FocusAction { SelectAll, Start, End, None }

        public FocusAction OnFocus
        {
            get => (FocusAction)GetValue(OnFocusProperty);
            set => SetValue(OnFocusProperty, value);
        }

        public static readonly DependencyProperty OnFocusProperty =
            DependencyProperty.Register("OnFocus", typeof(FocusAction), typeof(InputTextBox), new PropertyMetadata(FocusAction.None));

        public bool IsDefault
        {
            get { return (bool)GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        public static readonly DependencyProperty IsDefaultProperty =
            DependencyProperty.Register("IsDefault", typeof(bool), typeof(InputTextBox), new PropertyMetadata(false));

        private void InputTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsDefault)
            {
                inputTextBox.Focus();
            }
        }

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (OnFocus == FocusAction.SelectAll)
                inputTextBox.SelectAll();

            if (OnFocus == FocusAction.Start)
                inputTextBox.Select(0, 0);

            if (OnFocus == FocusAction.End)
                inputTextBox.Select(Text.Length, 0);
        }
    }
}
