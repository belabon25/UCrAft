using Data;
using Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vues.Utilitaire;

namespace Vues
{
    /// <summary>
    /// Classe permettant de gérer l'affichage des vues, sans avoir à tout faire dans le code behind des UserControls
    /// </summary>
    public class ManagerVue
    {
        /// <summary>
        /// Enum permettant de connaître ce que fait la fenêtre d'affichage actuellement
        /// </summary>
        private enum EEtatApplication
        {
            None,
            AfficheObjet,
            ModifieObjet,
            AjouteObjet,
        }


        /// <summary>
        /// Attribut contenant l'état de l'application, son nom comporte encapsulé pour prévenir son usage
        /// </summary>
        private EEtatApplication etatApplicationEncapsulé = EEtatApplication.None;

        /// <summary>
        /// Encapusule etatApplicationEncapsulé en mettant à zéro les éléments utilisé par l'état passé
        /// </summary>
        private EEtatApplication EtatApplication
        {
            get => etatApplicationEncapsulé;
            set
            {
                //mise à zero des elements utilisées
                switch (etatApplicationEncapsulé)
                {
                    case EEtatApplication.AjouteObjet:
                    case EEtatApplication.ModifieObjet:
                        objetEnCoursDeModification = null;
                        modificateurObjetUC.DataContext = null;
                        break;
                    case EEtatApplication.AfficheObjet:
                        afficheurObjetUC.DataContext = null;
                        break;
                }

                etatApplicationEncapsulé = value;
            }
        }

        /// <summary>
        /// La classe utilisée pour la persistance
        /// </summary>
        private IPersistanceManager managerPersistance = new PersXMLLinQ();

        /// <summary>
        /// Le ManagerObjet servant de passerelle vers le modèle
        /// </summary>
        private ManagerObjets managerObjets;

        private App ApplicationCourrante => Application.Current as App;

        /// <summary>
        /// Une référence vers la fenètre principale
        /// </summary>
        private MainWindow mainWindow;

        /// <summary>
        /// UC s'occupant de l'affichage d'objet
        /// </summary>
        private DetailObjetUC afficheurObjetUC;

        /// <summary>
        /// UC s'occupant de la modification et de l'ajout d'objet
        /// </summary>
        private ModifierAjouterObjetUC modificateurObjetUC;

        /// <summary>
        /// Référence vers l'objet séléctionné lors de la création/modification d'objet
        /// </summary>
        public Objet ObjetSelectionnéPourCreerLesCrafts { get; private set; }

        /// <summary>
        /// Objet servant de tampon pour la création/modification
        /// </summary>
        private Objet objetEnCoursDeModification;

        /// <summary>
        /// Initialise les différents éléments au lancement de l'application
        /// </summary>
        /// <param name="mainWindow"></param>
        internal void Setup(MainWindow mainWindow)
        {
            //Initialisation modele
            managerObjets = new ManagerObjets(managerPersistance.Charge().ToArray());

            //Afficheur objet
            afficheurObjetUC = new DetailObjetUC();
            afficheurObjetUC.ListViewItemsCraftableAvec.SelectionChanged += EventDemandeChangementObjetAffiché;
            afficheurObjetUC.CommandBindingNew.Executed += CommandeAjoutObjet;
            afficheurObjetUC.CommandBindingDelete.Executed += CommandeSuppressionObjet;
            afficheurObjetUC.CommandBindingModifier.Executed += CommandeModificationObjet;

            //Modificateur objet
            modificateurObjetUC = new ModifierAjouterObjetUC();
            modificateurObjetUC.CommandBindingSave.Executed += ValidationCommande;
            modificateurObjetUC.CommandBindingStop.Executed += AnnulationCommande;

            //MainWindow
            this.mainWindow = mainWindow;
            this.mainWindow.MasterList.SelectionChanged += MasterChangedSelectedObjet;
            EtatApplication = EEtatApplication.AfficheObjet;
            SetMasterSelectedObjet(managerObjets.Objets[0]);

            this.mainWindow.ColonneRecherche.DataContext = managerObjets;
            this.mainWindow.MasterList.DataContext = managerObjets;
        }

        /// <summary>
        /// Permet la sauvegarde des objets
        /// </summary>
        internal void Sauvegarde()
        {
            managerPersistance.Sauvegarde(managerObjets.Objets);
        }

        /// <summary>
        /// Est appelée lorsque la liste d'objet principale voit son objet selectionné changer.
        /// Cette méthode s'appuit sur l'état de l'application pour agir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterChangedSelectedObjet(object sender, SelectionChangedEventArgs e)
        {
            Objet objetSelectionné = mainWindow.MasterList.SelectedItem as Objet;
            if (objetSelectionné is null) return;
            switch (EtatApplication)
            {
                case EEtatApplication.AfficheObjet:
                    AfficherObjet(objetSelectionné);
                    break;
                case EEtatApplication.AjouteObjet:
                case EEtatApplication.ModifieObjet:
                    ObjetSelectionnéPourCreerLesCrafts = objetSelectionné;
                    break;
            }
        }


        private void SetMasterSelectedObjet(Objet nouvelObjetSelectionné)
        {
            mainWindow.MasterList.SelectedItem = nouvelObjetSelectionné;
        }

        /// <summary>
        /// Met l'application en place pour l'affichage de l'objet passé en paramètre
        /// </summary>
        /// <param name="objetAAfficher"></param>
        private void AfficherObjet(Objet objetAAfficher)
        {
            EtatApplication = EEtatApplication.AfficheObjet;
            if (!objetAAfficher.Equals(mainWindow.MasterList.SelectedItem))
            {
                SetMasterSelectedObjet(null);
            }
            mainWindow.CCDetail.Content = afficheurObjetUC;

            afficheurObjetUC.DataContext = objetAAfficher;
        }

        /// <summary>
        /// Met en place l'application pour la modification ou l'ajout de l'objet passé en paramètre
        /// </summary>
        /// <param name="objetAModifier"></param>
        private void SetUpModificationAjout(Objet objetAModifier)
        {
            objetEnCoursDeModification = objetAModifier;
            mainWindow.CCDetail.Content = modificateurObjetUC;
            modificateurObjetUC.DataContext = objetEnCoursDeModification;
        }

        /// <summary>
        /// Initie l'ajout d'un Item
        /// </summary>
        private void AjouterItem()
        {
            EtatApplication = EEtatApplication.AjouteObjet;
            SetUpModificationAjout(new Item("Nom", "", "Description générale", new Craft(new Dictionary<EPosition, Objet>(), 0), 0, 0));
            modificateurObjetUC.TextBoxNom.IsEnabled = true;
        }

        /// <summary>
        /// Initie l'ajout d'un matériau
        /// </summary>
        private void AjouterMateriau()
        {
            EtatApplication = EEtatApplication.AjouteObjet;
            SetUpModificationAjout(new Materiau("Nom", "", "Description générale", "Méthode de récolte", new Localisation(0, 100, 0)));
            modificateurObjetUC.TextBoxNom.IsEnabled = true;
        }

        /// <summary>
        /// Initie la modification de l'objet passé en paramètre
        /// </summary>
        /// <param name="objetAModifier"></param>
        private void ModifierObjet(Objet objetAModifier)
        {
            EtatApplication = EEtatApplication.ModifieObjet;
            SetUpModificationAjout(objetAModifier.Copy());
            modificateurObjetUC.TextBoxNom.IsEnabled = false;
        }

        /// <summary>
        /// Supprime l'objet passé en paramètre. Si d'autres objets en sont dépendants, un message est affiché et l'objet n'est pas supprimé
        /// </summary>
        /// <param name="objetASupprimer"></param>
        private void SupprimerObjet(Objet objetASupprimer)
        {
            if (objetASupprimer is null || !managerObjets.Objets.Contains(objetASupprimer)) return;

            var dependances = managerObjets.ItemCraftableAvec(objetASupprimer.Nom);

            if (dependances.Count() > 0)
            {
                string depandantsString = "";
                foreach (var d in dependances)
                {
                    depandantsString += $"{d.Nom}, ";
                }
                MessageBox.Show($"Imposible de supprimer {objetASupprimer.Nom}\n Des Items dépandent de lui.\n{depandantsString.Trim(' ', ',')}", "Imposible de supprimer l'objet");
                return;
            }

            DialogBox dialogBox = new DialogBox($"Suppression de {objetASupprimer.Nom}", $"Etes vous sûr de vouloir supprimer {objetASupprimer.Nom} ?", "Oui", "Non");

            if (dialogBox.ShowDialog() == true)
            {
                if (dialogBox.choix == 2) return;

                managerObjets.SupprimerObjet(objetASupprimer.Nom);

                AfficherObjet(managerObjets.Objets.First());
            }
        }

        /// <summary>
        /// Répond à la demande d'ajout d'un objet
        /// </summary>
        internal void DemandeAjoutObjet()
        {
            DialogBox itemOuMateriau = new DialogBox("Item ou Matériau", "Voulez-vous créer un item ou un matériau ?", "Item", "Matériau");

            Nullable<bool> result = itemOuMateriau.ShowDialog();

            if (result == true)
            {
                if (itemOuMateriau.choix == 1)
                {
                    AjouterItem();
                }
                else if (itemOuMateriau.choix == 2)
                {
                    AjouterMateriau();
                }
            }
        }

        /// <summary>
        /// Répond à la demande de modification de l'objet passé en paramètre
        /// </summary>
        /// <param name="objetAModifier"></param>
        internal void DemandeModificationObjet(Objet objetAModifier)
        {
            if (objetAModifier is null) return;
            ModifierObjet(objetAModifier);
        }

        /// <summary>
        /// Répond à la demande d'affichage de l'objet passé en paramètre
        /// </summary>
        /// <param name="objetAAfficher"></param>
        internal void DemandeChangementObjetAffiché(Objet objetAAfficher)
        {
            if (objetAAfficher is null) return;
            AfficherObjet(objetAAfficher);
        }

        /// <summary>
        /// Appelé lors d'une validation par un des UCs (principalement le modifieur)
        /// Valide l'action en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ValidationCommande(object sender, ExecutedRoutedEventArgs e)
        {
            switch (EtatApplication)
            {
                case EEtatApplication.AjouteObjet:
                    try
                    {
                        managerObjets.AjouterObjet(objetEnCoursDeModification);
                        AfficherObjet(objetEnCoursDeModification);
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("Le nom est déjà utilisé.", "Erreur objet existant");
                    }
                    break;
                case EEtatApplication.ModifieObjet:
                    AfficherObjet(managerObjets.ModifierObjet(objetEnCoursDeModification));
                    break;
            }
        }

        /// <summary>
        /// Appelé lors d'une validation par un des UCs (principalement le modifieur)
        /// Annule l'action en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AnnulationCommande(object sender, ExecutedRoutedEventArgs e)
        {
            if (EtatApplication != EEtatApplication.AfficheObjet)
            {
                AfficherObjet(managerObjets.Objets[0]);
            }
        }

        /// <summary>
        /// Evenement lié à DemandeChangementObjetAffiché
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void EventDemandeChangementObjetAffiché(object sender, SelectionChangedEventArgs e)
        {
            if (sender == afficheurObjetUC.ListViewItemsCraftableAvec)
            {
                DemandeChangementObjetAffiché(afficheurObjetUC.ListViewItemsCraftableAvec.SelectedItem as Objet);
            }
        }

        /// <summary>
        /// Commande liée à DemandeAjoutObjet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void CommandeAjoutObjet(object sender, ExecutedRoutedEventArgs e)
        {
            DemandeAjoutObjet();
        }

        /// <summary>
        /// Commande liée à SupprimerObjet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void CommandeSuppressionObjet(object sender, ExecutedRoutedEventArgs e)
        {
            if (EtatApplication == EEtatApplication.AfficheObjet)
                SupprimerObjet(afficheurObjetUC.DataContext as Objet);
        }

        /// <summary>
        /// Commande liée à DemandeModificationObjet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandeModificationObjet(object sender, ExecutedRoutedEventArgs e)
        {
            DemandeModificationObjet(afficheurObjetUC.DataContext as Objet);
        }
    }
}
