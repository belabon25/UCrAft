using Modele;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vues
{
    /// <summary>
    /// Logique d'interaction pour DetailObjetUC.xaml
    /// </summary>
    public partial class DetailObjetUC : UserControl
    {
        private readonly GrilleCraftUC grilleCraft = new GrilleCraftUC();
        private readonly UCPetitsElements.DetailSpecifiqueItemUC detailSpecifiqueItem = new UCPetitsElements.DetailSpecifiqueItemUC();
        private readonly UCPetitsElements.DetailSpecifiqueMateriauUC detailSpecifiqueMateriau = new UCPetitsElements.DetailSpecifiqueMateriauUC();

        public DetailObjetUC()
        {
            InitializeComponent();
            Style styleBouton = FindResource("BoutonsApp") as Style;
            BoutonAjouter.Style = styleBouton;
            BoutonModifier.Style = styleBouton;
            BoutonSupprimer.Style = styleBouton;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (DataContext is null) return;

           // ListViewItemsCraftableAvec = new ListView();

            if (DataContext is Item)
            {
                //Mise en place de la liste des items craftable avec
                //ListViewItemsCraftableAvec.ItemsSource = (DataContext as Item).ItemCraftableAvec;
                ListViewItemsCraftableAvec.ItemTemplate = FindResource("objetReduit") as DataTemplate;
                ListViewItemsCraftableAvec.ItemsPanel = FindResource("itemTemplateHorizontal") as ItemsPanelTemplate;

                //Mise en place affichage spécifique au items
                CCGrilleCraft.Content = grilleCraft;
                CCDetailSpecifique.Content = detailSpecifiqueItem;
            }
            else if (DataContext is Materiau)
            {
                //Mise en place de la liste des items craftable avec
                //ListViewItemsCraftableAvec.ItemsSource = (DataContext as Materiau).ItemCraftableAvec;
                ListViewItemsCraftableAvec.ItemTemplate = FindResource("itemCraftableDetaillé") as DataTemplate;
                ListViewItemsCraftableAvec.ItemsPanel = FindResource("itemTemplateVertical") as ItemsPanelTemplate;

                //Mise en place affichage spécifique au materiaux
                CCGrilleCraft.Content = null;
                CCDetailSpecifique.Content = detailSpecifiqueMateriau;
            }
            Thread.Sleep(100); // Pour éviter qu'un clique permette de naviguer dans plusieurs items en quelques milisecondes.
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
