using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Modele
{
    /// <summary>
    /// Contient et gère les objets. Sert de passerelle vers l'extérieur du Modèle
    /// </summary>
    public class ManagerObjets : INotifyPropertyChanged
    {
        /// <summary>
        /// Tous les objets enregistrés
        /// </summary>
        public readonly ReadOnlyCollection<Objet> Objets;
        private readonly List<Objet> objets = new List<Objet>();

        /// <summary>
        /// Un filtre offrant la possibilité de sélectionner une partie des Objets à afficher avec ObjetsFiltres
        /// </summary>
        public string Filtre
        {
            get
            {
                return filtre;
            }
            set
            {
                filtre = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ObjetsFiltres"));
            }
        }
        private string filtre = "";

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Propriété calculée pour filtrer en fonction du string Filtre
        /// </summary>
        public IEnumerable<Objet> ObjetsFiltres
        {
            get
            {
                return Objets.Where(o => o.Nom.ToLower().Contains(Filtre.ToLower()));
            }
        }

        /// <summary>
        /// Constructeur de ManagerObjets, peut prendre une serie d'Objet en paramètre 
        /// /// </summary>
        /// <param name="objetsFournis">Des objets fourni à la construction si il y en a</param>
        public ManagerObjets(params Objet[] objetsFournis)
        {
            Objets = new ReadOnlyCollection<Objet>(objets);
            foreach (Objet o in objetsFournis)
            {
                AjouterObjet(o);
            }
        }
        /// <summary>
        /// Référence et renvoie une liste de tous les items craftables avec un matériau dont le nom est donné en paramètre
        /// </summary>
        /// <param name="nomObjet"> le nom d'un objet </param>
        /// <returns> Liste d'items craftable avec l'objet dont le nom est passé en argument </returns>
        public IEnumerable<Item> ItemCraftableAvec(string nomObjet)
        {
            Objet objetPourCraft = objets.Find(o => o.Nom.Equals(nomObjet));
            if (objetPourCraft is null)
            {
                throw new ArgumentException("Objet inconnu");
            }

            //Retourne,dans la liste "Objets", la liste des Items dont le pattern de craft contient en valeur l'objet indiqué.
            return Objets.Where(o => o is Item).Select(o => o as Item).Where(o => o.CraftItem.Pattern.ContainsValue(objetPourCraft));
        }

        /// <summary>
        /// Permet d'ajouter un objet à la liste d'objets du conteneur.
        /// </summary>
        /// <param name="objet"> Objet voulant être ajouté à la liste</param>
        /// <exception cref="MissingFieldException"> Objet invalide </exception>
        /// <exception cref="ArgumentException"> Objet déjà présent dans la liste </exception>
        public void AjouterObjet(Objet objet)
        {
            if (!objet.IsValide())
            {
                throw new MissingFieldException();
            }
            if (Objets.Contains(objet))
            {
                throw new ArgumentException();
            }
            objets.Add(objet);
            objet.RecuperateurItemCraftableAvec += ItemCraftableAvec; //Injection de la dépendance

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ObjetsFiltres"));
        }

        /// <summary>
        /// Permet d'ajouter une liste d'objets donnée en paramètre
        /// Facilite l'ajout multiple d'objets
        /// </summary>
        /// <param name="objets"> Liste d'objets voulant être ajoutée </param>
        public void AjouterObjet(params Objet[] objets)
        {
            foreach (Objet o in objets)
            {
                AjouterObjet(o);
            }
        }

        /// <summary>
        /// Permet de modifier un objet dans la liste d'objets
        /// </summary>
        /// <param name="objet"> Objet voulant être modifié </param>
        /// <exception cref="ArgumentException"> Si l'objet n'existe pas dans la liste ou si il est invalide</exception>
        public Objet ModifierObjet(Objet objetModifié)
        {
            if (!objetModifié.IsValide())
            {
                throw new ArgumentException("L'objet modifié n'est pas valide");
            }

            Objet objetAModifier = objets.SingleOrDefault(o => o.Nom.Equals(objetModifié.Nom));
            if (objetAModifier is null)
            {
                throw new ArgumentOutOfRangeException("Objet non trouvé");
            }

            Type typeObjet = objetAModifier.GetType();
            var propriétésAModifier = typeObjet.GetProperties().Where(p => p.CanWrite);
            //On réécrit tout l'objet avec les propriétés de l'objet modifié.
            foreach (var prop in propriétésAModifier)
            {
                prop.SetValue(objetAModifier, prop.GetValue(objetModifié));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ObjetsFiltres"));
            return objetAModifier;
        }


        /// <summary>
        /// Supprime un objet dont le nom est donné en paramètre.
        /// Ne supprime pas les dépendances.
        /// </summary>
        /// <param name="nomObjet">Le nom de l'objet à supprimer</param>
        /// <exception cref="ArgumentException"> Objet introuvable </exception>
        public void SupprimerObjet(string nomObjet)
        {
            try
            {
                Objet objetASupprimer = objets.Find(o => o.Nom.Equals(nomObjet));
                if (objetASupprimer is null)
                {
                    throw new ArgumentException("L'objet à supprimer n'a pas été trouvé !");
                }
                objets.Remove(objetASupprimer);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ObjetsFiltres"));
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine($"{e.Message}\nNe doit pas se produire.");
            }
        }

        /// <summary>
        /// Recherche et retourne une liste d'objets dont le nom contient le string passé en paramètre.
        /// </summary>
        /// <param name="partieNomObjet"> string contenant le nom ou une partie du nom de l'objet recherché </param>
        /// <returns> IEnumerable d'objets correspondants  </returns>
        public IEnumerable<Objet> RechercherObjets(string partieNomObjet)
        {
            return Objets.Where(o => o.Nom.Contains(partieNomObjet));
        }
    }
}
