using System;

namespace Modele
{
    /// <summary>
    /// Represente les items du jeu Minecraft ainsi que des informations proposées par l'application
    /// </summary>
    public class Item : Objet
    {
        private int efficacite;
        private int importance;
        private Craft craftItem;

        /// <summary>
        /// Indique l'efficacité d'un Item
        /// </summary>
        public int Efficacite
        {
            get => efficacite;
            set
            {
                efficacite = value;
                OnPropertyChanged(nameof(Valide));
                OnPropertyChanged(nameof(Efficacite));
            }
        }

        /// <summary>
        /// Principalement pour les débutants, indique si un item est utile
        /// </summary>
        public int Importance
        {
            get => importance;
            set
            {
                importance = value;
                OnPropertyChanged(nameof(Valide));
                OnPropertyChanged(nameof(Importance));
            }
        }

        /// <summary>
        /// Le craft pour créer l'item
        /// </summary>
        public Craft CraftItem
        {
            get => craftItem;
            private set
            {
                craftItem = value;

                //Mise en place de la transmission des notifications de modifications
                craftItem.PropertyChanged += (s, a) => OnPropertyChanged(nameof(Valide));
                craftItem.Pattern.CollectionChanged += (s, a) => OnPropertyChanged(nameof(Valide));
                craftItem.PropertyChanged += (s, a) => OnPropertyChanged(nameof(CraftItem));
                craftItem.Pattern.CollectionChanged += (s, a) => OnPropertyChanged(nameof(CraftItem));
            }
        }

        public Item(string nom, string imageChemin, string descriptionGenerale, Craft craftItem, int efficacite, int importance)
            : base(nom, imageChemin, descriptionGenerale)
        {
            CraftItem = craftItem;
            Efficacite = efficacite;
            Importance = importance;
        }

        /// <summary>
        /// Constructeur sans aucun craft. Destiné à n'être utilisé que lors de la déserialization.
        /// </summary>
        public Item(string nom, string imageChemin, string descriptionGenerale, int efficacite, int importance) : base(nom, imageChemin, descriptionGenerale)
        {
            Efficacite = efficacite;
            Importance = importance;
        }
        /// <summary>
        /// Permet d'ajouter un craft à un item qui n'en a pas. Destiné à n'être utilisé que lors de la déserialization.
        /// </summary>
        /// <param name="craft"></param>
        public void AddCraft(Craft craft)
        {
            if (CraftItem != null) throw new InvalidOperationException();
            CraftItem = craft;
        }

        /// <summary>
        ///Permet de vérifier la validité d'un item.
        ///Un item est valide si : 
        ///    -Les propriétés de base sont valides
        ///    -Importance et efficacité >0 et <=5 
        ///    -Le craft est valide
        /// </summary>
        /// <returns> True si le Materiau est valide</returns>
        public override bool IsValide()
        {
            if (!base.IsValide()) return false;

            if (Importance < 0 || Importance > 5) return false;

            if (CraftItem is null) return false;

            if (!CraftItem.IsValide()) return false;

            if (CraftItem.Pattern.ContainsValue(this)) return false;


            return Efficacite >= 0 && Efficacite <= 5;
        }

        public override string ToString()
        {
            return $"{base.ToString()} {CraftItem}";
        }
    }
}
