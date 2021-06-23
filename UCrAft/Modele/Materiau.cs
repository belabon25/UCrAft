namespace Modele
{
    /// <summary>
    /// Represente les matériaux du jeu Minecraft 
    /// </summary>
    public class Materiau : Objet
    {
        private string methodeDeRecolte;
        private Localisation localisationMateriau;

        /// <summary>
        /// Par exemple : avec une pioche en fer
        /// </summary>
        public string MethodeDeRecolte
        {
            get => methodeDeRecolte;
            set
            {
                methodeDeRecolte = value;
                OnPropertyChanged(nameof(MethodeDeRecolte));
            }
        }

        /// <summary>
        /// Spécifie où il est possible de trouver le matériau
        /// </summary>
        public Localisation LocalisationMateriau
        {
            get => localisationMateriau;
            private set
            {
                localisationMateriau = value;
                localisationMateriau.PropertyChanged += (s, a) => OnPropertyChanged(nameof(Valide));
                localisationMateriau.PropertyChanged += (s, a) => OnPropertyChanged(nameof(LocalisationMateriau));
            }
        }

        public Materiau(string nom, string imageChemin, string descriptionGenerale, string methodeDeRecolte, Localisation localisationMateriau)
            : base(nom, imageChemin, descriptionGenerale)
        {
            MethodeDeRecolte = methodeDeRecolte;
            LocalisationMateriau = localisationMateriau;
        }

        /// <summary>
        ///         Permet de vérifier la validité d'un matériau.
        ///Un matériau est valide si : 
        ///    -Les propriétés de base sont valides
        ///    -La localisation est valide
        ///    -Texte méthode de récolte non vide.
        /// </summary>
        /// <returns> True si le Materiau est valide</returns>
        public override bool IsValide()
        {
            if (!base.IsValide()) return false;

            if (!LocalisationMateriau.IsValide()) return false;

            return !string.IsNullOrWhiteSpace(MethodeDeRecolte);
        }

        public override string ToString()
        {
            return base.ToString() + LocalisationMateriau.ToString();
        }
    }
}
