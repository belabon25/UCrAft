using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Modele
{
    /// <summary>
    /// Represente le craft d'un item dans le jeu
    /// </summary>
    public class Craft : INotifyPropertyChanged
    {

        /// <summary>
        /// Les differents Objets necessaires au craft attaché à leurs positions sur la grille
        /// </summary>
        public ObservableDictionary<EPosition, Objet> Pattern { get; private set; }

        /// <summary>
        /// Nombre d'exemplaires créés à la fois
        /// </summary>
        public uint NombreLorsDeLaCreation
        {
            get => nombreLorsDeLaCreation;
            set
            {
                nombreLorsDeLaCreation = value;
                OnPropertyChanged(nameof(NombreLorsDeLaCreation));
            }
        }
        private uint nombreLorsDeLaCreation;

        /// <summary>
        /// Facilite l'utilisation de PropertyChanged
        /// </summary>
        /// <param name="nomPropriété">Nom de la propriété pour laquelle la notification sera lancée</param>
        private void OnPropertyChanged(string nomPropriété)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriété));
        }

        public Craft(Dictionary<EPosition, Objet> pattern, uint nombreLorsDeLaCreation)
        {
            Pattern = new ObservableDictionary<EPosition, Objet>(pattern);
            NombreLorsDeLaCreation = nombreLorsDeLaCreation;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Indique si le craft est valide
        /// -Le nombre d'entrées de pattern et compris entre 1 et 9 bornes comprises
        /// -Le nombre à la création est non null
        /// </summary>
        /// <returns>True si le craft est valide</returns>
        public bool IsValide()
        {
            if (Pattern.Count <= 0 || Pattern.Count > 9 || NombreLorsDeLaCreation == 0)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder returnValue = new StringBuilder("{ ");
            foreach (var pPO in Pattern) //pPO pour pairePositionObjet
            {
                returnValue.Append($"[{pPO.Key}] = {pPO.Value.Nom}, ");
            }
            return returnValue.Append(" }").ToString();
        }
    }
}
