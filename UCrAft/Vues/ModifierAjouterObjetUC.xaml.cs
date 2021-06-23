using Modele;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vues.UCPetitsElements;
using Path = System.IO.Path;

namespace Vues
{
    /// <summary>
    /// Logique d'interaction pour ModifierAjouterObjetUC.xaml
    /// </summary>
    public partial class ModifierAjouterObjetUC : UserControl
    {

        private readonly GrilleCraftUC grilleModification = new GrilleCraftUC(true);
        private readonly SelecteurBiome selecteurBiome = new SelecteurBiome();
        private readonly ModifDetailSpeItem modifDetailSpeItem = new ModifDetailSpeItem();
        private readonly ModifDetailSpeMateriau modifDetailSpeMateriau = new ModifDetailSpeMateriau();

        public ModifierAjouterObjetUC()
        {
            InitializeComponent();
            Style styleBouton = FindResource("BoutonsApp") as Style;
            BoutonValider.Style = styleBouton;
            BoutonAnnuler.Style = styleBouton;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is null) return;

            IconImporter.Visibility = String.IsNullOrWhiteSpace((DataContext as Objet).ImageChemin) ? Visibility.Visible : Visibility.Collapsed;

            if (DataContext is Item)
            {
                CCGrilleModifItem.Content = grilleModification;
                CCModifDetailItemMateriau.Content = modifDetailSpeItem;
            }
            else if (DataContext is Materiau)
            {
                CCGrilleModifItem.Content = selecteurBiome;
                CCModifDetailItemMateriau.Content = modifDetailSpeMateriau;
            }
        }

        private void BoutonAjouterUneImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            dialog.FileName = "Image Item";
            dialog.DefaultExt = ".jpg | .png";
            dialog.Filter = "All Images files (*.jpg, *.png)| *.jpg; *.png; | JPG files (*.jpg) | *.jpg | PNG files (*.png) | *.png";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string cheminDossierImages = Path.Combine(Directory.GetCurrentDirectory(), "../Images/");
                if (!Directory.Exists(cheminDossierImages))
                {
                    Directory.CreateDirectory(cheminDossierImages);
                }

                string newFileName = Path.Combine(cheminDossierImages, Path.GetFileName(dialog.FileName));
                if (!File.Exists(newFileName))
                {
                    File.Copy(dialog.FileName, newFileName);
                }

                (DataContext as Objet).ImageChemin = Path.GetFileName(newFileName);
                IconImporter.Visibility = Visibility.Collapsed;
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
