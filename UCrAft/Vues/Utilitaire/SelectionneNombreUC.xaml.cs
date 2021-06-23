using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vues.Utilitaire
{
    /// <summary>
    /// Logique d'interaction pour SelectionneNombreUC.xaml
    /// </summary>
    public partial class SelectionneNombreUC : UserControl
    {

        public int? ValeurMinimum
        {
            get { return (int?)GetValue(ValeurMinimumProperty); }
            set { SetValue(ValeurMinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValeurMinimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValeurMinimumProperty =
            DependencyProperty.Register(nameof(ValeurMinimum), typeof(int?), typeof(SelectionneNombreUC), new PropertyMetadata(null));



        public int? ValeurMaximum
        {
            get { return (int?)GetValue(ValeurMaximumProperty); }
            set { SetValue(ValeurMaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValeurMaximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValeurMaximumProperty =
            DependencyProperty.Register(nameof(ValeurMaximum), typeof(int?), typeof(SelectionneNombreUC), new PropertyMetadata(null));


        public int Valeur
        {
            get { return (int)GetValue(ValeurProperty); }
            set
            {
               
                SetValue(ValeurProperty, value);


                if ((ValeurMaximum != null && Valeur > ValeurMaximum) || (ValeurMinimum != null && Valeur < ValeurMinimum))
                {
                    TextBoxNombre.Foreground = Brushes.Red;
                }
                else
                {
                    TextBoxNombre.Foreground = Brushes.Black;
                }
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValeurProperty =
            DependencyProperty.Register(nameof(Valeur), typeof(int), typeof(SelectionneNombreUC), new PropertyMetadata(0));


        public SelectionneNombreUC()
        {
            InitializeComponent();
        }

        private void Click_Minus(object sender, RoutedEventArgs e)
        {
            Valeur--;
        }

        private void Click_Plus(object sender, RoutedEventArgs e)
        {
            Valeur++;
        }
    }
}
