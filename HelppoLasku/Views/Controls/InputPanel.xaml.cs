using System.Windows;
using System.Windows.Controls;


namespace HelppoLasku.Views.Controls
{
    public partial class InputPanel : StackPanel
    {
        public InputPanel()
        {
            InitializeComponent();
            Grid.SetIsSharedSizeScope(this, true);
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(InputPanel), new PropertyMetadata(""));
    }
}
