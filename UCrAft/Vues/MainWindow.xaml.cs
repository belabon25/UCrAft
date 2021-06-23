using System.Windows;
using System.Windows.Input;

namespace Vues
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool PanneauRechercheDeplie
        {
            get
            {
                return panneauRechercheDeplie;
            }
            set
            {
                if (value)
                {//On déplie le panneau
                    TextBoxRecherche.Visibility = Visibility.Visible;
                    ColonneItems.Width = new GridLength(200, GridUnitType.Pixel);
                    StackIconeBarre.HorizontalAlignment = HorizontalAlignment.Left;
                    Keyboard.Focus(TextBoxRecherche);
                }
                else
                {//On replie le panneau
                    TextBoxRecherche.Visibility = Visibility.Collapsed;
                    ColonneItems.Width = new GridLength(50, GridUnitType.Pixel);
                    StackIconeBarre.HorizontalAlignment = HorizontalAlignment.Center;
                }
                panneauRechercheDeplie = value;
            }
        }


        private bool panneauRechercheDeplie = false;

        public ManagerVue managerVue => (Application.Current as App).managerVue;

        public MainWindow()
        {
            InitializeComponent();
            BoutonRecherche.DataContext = this;
            PanneauRechercheDeplie = false;
            managerVue.Setup(this);
            CommandBindingStop.Executed += managerVue.AnnulationCommande;
            CommandBindingDelete.Executed += managerVue.CommandeSuppressionObjet;
        }

        private void RechercheGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            PanneauRechercheDeplie = true;
        }

        private void GridItemCraft_MouseDown_Stop_Recherche(object sender, MouseButtonEventArgs e)
        {
            PanneauRechercheDeplie = false;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBindingFind_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PanneauRechercheDeplie = true;
        }

        private void CommandBindingStop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PanneauRechercheDeplie = false;
        }
    }
}
