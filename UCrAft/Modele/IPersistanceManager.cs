using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Modele
{
    /// <summary>
    /// Indique quelles méthodes sont utilisées par le modèle pour sérializer les données.
    /// </summary>
    public interface IPersistanceManager
    {
        public void Sauvegarde(ReadOnlyCollection<Objet> co);
        public List<Objet> Charge();
    }
}
