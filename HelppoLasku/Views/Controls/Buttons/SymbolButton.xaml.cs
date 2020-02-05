using System.Windows;
using System.Windows.Controls;


namespace HelppoLasku.Views.Controls
{
    public partial class SymbolButton : Button
    {
        public SymbolButton()
        {
            InitializeComponent();
        }

        public enum ArrowDirection { Up, Down };

        public ArrowDirection Direction
        {
            get { return (ArrowDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(ArrowDirection), typeof(SymbolButton), null);


    }
}
