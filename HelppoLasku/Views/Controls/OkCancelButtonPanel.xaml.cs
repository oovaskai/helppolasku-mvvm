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
    /// Interaction logic for OkCancelButtonPanel.xaml
    /// </summary>
    public partial class OkCancelButtonPanel : UserControl
    {
        public OkCancelButtonPanel()
        {
            InitializeComponent();
        }

        public bool IsOkDefault
        {
            get { return (bool)GetValue(IsOkDefaultProperty); }
            set { SetValue(IsOkDefaultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOkDefault.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOkDefaultProperty =
            DependencyProperty.Register("IsOkDefault", typeof(bool), typeof(OkCancelButtonPanel), new PropertyMetadata(true));



        public new bool Focusable
        {
            get { return (bool)GetValue(FocusableProperty); }
            set { SetValue(FocusableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Focusable.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty FocusableProperty =
            DependencyProperty.Register("Focusable", typeof(bool), typeof(OkCancelButtonPanel), new PropertyMetadata(true));


    }
}
