using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modele
{
    /// <summary>
    /// Comporte les propriétés et methodes communes aux items et aux matériaux
    /// Offre la possibilité de polymorphisme entre Item et Materiau
    /// </summary>
    public abstract class Objet : IEquatable<Objet>, INotifyPropertyChanged
    {
        public string Nom
        {
            get => nom;
            set
            {
                nom = value;
                OnPropertyChanged(nameof(Nom));
                OnPropertyChanged(nameof(Valide));
            }
        }
        public string ImageChemin
        {
            get => imageChemin;
            set
            {
                imageChemin = value;
                OnPropertyChanged(nameof(Valide));
                OnPropertyChanged(nameof(ImageChemin));
            }
        }
        public string DescriptionGenerale
        {
            get => descriptionGenerale;
            set
            {
                descriptionGenerale = value;
                OnPropertyChanged(nameof(Valide));
                OnPropertyChanged(nameof(DescriptionGenerale));
            }
        }

        /// <summary>
        /// Depandance injecté par le ConteneurObjet lors de l'ajout de l'objet
        /// Offre la possibilité de connaitre les items craftables avec cet objet à partir de lui même (pratique pour le binding)
        /// </summary>
        public Func<string, IEnumerable<Item>> RecuperateurItemCraftableAvec;

        /// <summary>
        /// Propriété utilisant RecuperateurItemCraftableAvec
        /// </summary>
        public IEnumerable<Item> ItemCraftableAvec => RecuperateurItemCraftableAvec?.Invoke(Nom);

        private string nom;
        private string imageChemin;
        private string descriptionGenerale;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Rend l'utilisation de PropertyChanged plus facile
        /// </summary>
        /// <param name="propertyName">nom de la propriété pour laquelle lancer la notification</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Objet(string nom, string imageChemin, string descriptionGenerale)
        {
            Nom = nom;
            ImageChemin = imageChemin;
            DescriptionGenerale = descriptionGenerale;
        }

        public override string ToString()
        {
            return $"{Nom};{ImageChemin};{DescriptionGenerale}";
        }

        /// <summary>
        /// Permet le binding sur la validité d'un objet
        /// </summary>
        public virtual bool Valide => IsValide();

        /// <summary>
        /// Permet de vérifier la validité d'un objet
        /// Un objet est valide si 
        ///     -ImageChemin , nom et Description non vide
        /// </summary>
        /// <returns> True si l'Objet est valide </returns>
        public virtual bool IsValide()
        {
            if (String.IsNullOrWhiteSpace(DescriptionGenerale) || String.IsNullOrWhiteSpace(ImageChemin) || String.IsNullOrWhiteSpace(Nom))
            {
                return false;
            }
            return true;
        }

        public bool Equals(Objet other)
        {
            return Nom.Equals(other.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (!(obj is Objet)) return false;
            return Equals(obj as Objet);
        }

        public override int GetHashCode()
        {
            return Nom.GetHashCode();
        }
    }

    public static class ExtensionObjet
    {
        public static Objet Copy(this Objet objetSource)
        {
            if (objetSource is Item)
            {
                Item itemSource = objetSource as Item;
                Item copie = new Item(itemSource.Nom, itemSource.ImageChemin, itemSource.DescriptionGenerale, itemSource.Efficacite, itemSource.Importance);
                if (!(itemSource.CraftItem is null))
                {
                    Dictionary<EPosition, Objet> pattern = new Dictionary<EPosition, Objet>(itemSource.CraftItem.Pattern);//copié les refererences des Objet n'est pas grave et EPosition est un enum
                    copie.AddCraft(new Craft(pattern, itemSource.CraftItem.NombreLorsDeLaCreation));
                }
                return copie;
            }
            else if (objetSource is Materiau)
            {
                Materiau materiauSource = objetSource as Materiau;

                return new Materiau(materiauSource.Nom,
                                    materiauSource.ImageChemin,
                                    materiauSource.DescriptionGenerale,
                                    materiauSource.MethodeDeRecolte,
                                    new Localisation(materiauSource.LocalisationMateriau.CoucheMin,
                                                     materiauSource.LocalisationMateriau.CoucheMax,
                                                     materiauSource.LocalisationMateriau.Biomes));
            }
            throw new ArgumentException();
        }
    }
}
