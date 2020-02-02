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
    public partial class TextUnitInputControl : UserControl
    {
        public TextUnitInputControl()
        {
            InitializeComponent();
        }

        public HorizontalAlignment TextAligment
        {
            get { return (HorizontalAlignment)GetValue(TextAligmentProperty); }
            set { SetValue(TextAligmentProperty, value); }
        }

        public static readonly DependencyProperty TextAligmentProperty =
            DependencyProperty.Register("TextAligment", typeof(HorizontalAlignment), typeof(TextUnitInputControl), new PropertyMetadata(HorizontalAlignment.Right));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TextUnitInputControl), new PropertyMetadata(""));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextUnitInputControl), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(TextUnitInputControl), new PropertyMetadata(""));

        public bool IsDefault
        {
            get { return (bool)GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDefault.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDefaultProperty =
            DependencyProperty.Register("IsDefault", typeof(bool), typeof(TextUnitInputControl), new PropertyMetadata(false));
    }
}
