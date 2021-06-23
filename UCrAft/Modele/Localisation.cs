using System.ComponentModel;

namespace Modele
{
    /// <summary>
    /// Represente une zone (ou plusieurs) dans le jeu Minecraft
    /// </summary>
    public class Localisation : INotifyPropertyChanged
    {
        private int coucheMin;
        private int coucheMax;
        private EBiomes biomes;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Rend l'utilisation de PropertyChanged plus facile
        /// </summary>
        /// <param name="propertyName">nom de la propriété pour laquelle lancer la notification</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// La couche la plus basse de la zone décrite
        /// </summary>
        public int CoucheMin
        {
            get => coucheMin;
            set
            {
                coucheMin = value;
                OnPropertyChanged(nameof(CoucheMin));
            }
        }

        /// <summary>
        /// La couche la plus haute de la zone décrite
        /// </summary>
        public int CoucheMax
        {
            get => coucheMax;
            set
            {
                coucheMax = value;
                OnPropertyChanged(nameof(CoucheMax));
            }
        }

        /// <summary>
        /// Les biomes possible pour les zones decrite
        /// </summary>
        public EBiomes Biomes
        {
            get => biomes;
            set
            {
                biomes = value;
                OnPropertyChanged(nameof(Biomes));
            }
        }

        public Localisation(int coucheMin, int coucheMax, EBiomes biomes)
        {
            CoucheMin = coucheMin;
            CoucheMax = coucheMax;
            Biomes = biomes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Si la localisation est valide</returns>
        public bool IsValide()
        {
            return Biomes != EBiomes.Indefini && CoucheMin >= 0 && CoucheMax < 128 && CoucheMax >= CoucheMin;
        }

        public override string ToString()
        {
            return $" Couche Maximale : {CoucheMax} \n Couche Minimale : {CoucheMin} \n Biome : {Biomes}\n";
        }
    }
}