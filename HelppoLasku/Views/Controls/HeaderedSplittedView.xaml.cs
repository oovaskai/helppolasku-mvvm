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
    public partial class HeaderedSplittedView : Grid
    {
        public HeaderedSplittedView()
        {
            InitializeComponent();
        }

        public object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(object), typeof(HeaderedSplittedView), new PropertyMetadata(null));

        public object RightContent
        {
            get { return (object)GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(HeaderedSplittedView), new PropertyMetadata(null));

        public object LeftContent
        {
            get { return (object)GetValue(LeftContentProperty); }
            set { SetValue(LeftContentProperty, value); }
        }

        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.Register("LeftContent", typeof(object), typeof(HeaderedSplittedView), new PropertyMetadata(null));

        public double LeftWidth
        {
            get { return (double)GetValue(LeftWidthProperty); }
            set { SetValue(LeftWidthProperty, value); }
        }

        public static readonly DependencyProperty LeftWidthProperty =
            DependencyProperty.Register("LeftWidth", typeof(double), typeof(HeaderedSplittedView), new FrameworkPropertyMetadata(782.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }
}
