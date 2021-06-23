using Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Vues.UCPetitsElements
{
    /// <summary>
    /// Logique d'interaction pour SelecteurBiome.xaml
    /// </summary>
    public partial class SelecteurBiome : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Dictionary<EBiomes, bool> BiomesSelectionnés { get; set; }

        public SelecteurBiome()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is null)
            {
                return;
            }
            BiomesSelectionnés = new Dictionary<EBiomes, bool>();

            EBiomes biomesDataContext = (DataContext as Materiau).LocalisationMateriau.Biomes;
            //On ajoute chaque biome sauf indefini
            foreach (EBiomes biome in Enum.GetValues(typeof(EBiomes)))
            {
                if (biome != EBiomes.Indefini)
                {
                    //Le biome est selectionné si il est contenu dans biomeDataContext
                    BiomesSelectionnés[biome] = (biome & biomesDataContext) == biome;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BiomesSelectionnés)));
        }

        private void Biome_Checked(object sender, RoutedEventArgs e)
        {
            EBiomes biome = ((KeyValuePair<EBiomes, bool>)((sender as CheckBox).DataContext)).Key;

            (DataContext as Materiau).LocalisationMateriau.Biomes |= biome;
        }

        private void Biome_Unchecked(object sender, RoutedEventArgs e)
        {
            EBiomes biome = ((KeyValuePair<EBiomes, bool>)((sender as CheckBox).DataContext)).Key;

            (DataContext as Materiau).LocalisationMateriau.Biomes &= ~biome;

            /* 1 1 1 0 1 1
             * 0 1 0 0 0 0 Celui que je veux enlever
             * 1 0 1 1 1 1 On inverse la ligne 2
             * 1 0 1 0 1 1 On fait un & entre la ligne 1 et la ligne 3 */
        }
    }
}
