using Data;
using Modele;
using System;
using System.Collections.Generic;

namespace TestsFonctionnels
{
    class Program
    {
        static void Main(string[] args)
        {
            ManagerObjets conteneurObjet = new ManagerObjets(new Stub().Charge().ToArray());


            AffichageListeObjets(conteneurObjet);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            TestSuppressionObjet(conteneurObjet);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            AjouterObjet(conteneurObjet);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            RechercheConteneurObjet(conteneurObjet);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            ModificationObjet(conteneurObjet);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            ConsulterObjet(conteneurObjet);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            TestItemsCraftablesAvec(conteneurObjet);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            TestSauvegarde(conteneurObjet);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            TestLecture();
        }

        //Cas d'utilisation : Ajouter un objet
        static void AjouterObjet(ManagerObjets conteneurObjet)
        {
            Materiau materiau = new Materiau("pierre", "image/pierre.png", "Un des premiers materiaux", "à la pioche",
                new Localisation(0, 0, EBiomes.Plaine | EBiomes.Foret));

            Console.WriteLine($"Ajout de l'objet : {materiau}");

            conteneurObjet.AjouterObjet(materiau);

            AffichageListeObjets(conteneurObjet);
        }

        //Cas d'utilisation : Rechercher un Objet
        static void RechercheConteneurObjet(ManagerObjets conteneurObjet)
        {
            var resRecherche = conteneurObjet.RechercherObjets("a");
            Console.WriteLine("Objet dont le nom contient 'a'");

            foreach (Objet o in resRecherche)
            {
                Console.WriteLine(o);
            }
        }

        //Cas d'utilisation : Supprimer un objet
        static void TestSuppressionObjet(ManagerObjets conteneurObjet)
        {
            Console.WriteLine("Suppression de l'objet azer");
            conteneurObjet.SupprimerObjet("azer");

            AffichageListeObjets(conteneurObjet);

        }

        //Cas d'utilisation : Affichage de la liste des objets
        static void AffichageListeObjets(ManagerObjets conteneurObjet)
        {
            Console.WriteLine(conteneurObjet.Objets.Count);
            foreach (Objet o in conteneurObjet.Objets)
            {
                Console.WriteLine(o);
            }
        }

        //Cas d'utilisation : Modification d'un objet
        static void ModificationObjet(ManagerObjets conteneurObjet)
        {
            Console.WriteLine("AVANT MODIFICATION de azfsbonqf:");
            AffichageListeObjets(conteneurObjet);
            conteneurObjet.ModifierObjet(new Materiau("azfsbonqf", "Nouveau chemin d'image", "nouveau", "nouveau", new Localisation(0, 50, EBiomes.Desert)));
            Console.WriteLine();
            Console.WriteLine("APRES MODIFICATION :");
            AffichageListeObjets(conteneurObjet);
        }

        //Cas d'utilisation : Consulter un objet
        static void ConsulterObjet(ManagerObjets conteneurObjet)
        {
            string nomObjet = "azfsbonqf";
            Console.WriteLine($"Consultation de {nomObjet}");
            var resRecherche1 = conteneurObjet.RechercherObjets(nomObjet); //Consultation Matériau
            foreach (Objet o in resRecherche1)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            nomObjet = "item1";
            Console.WriteLine($"Consultation de {nomObjet}");
            var resRecherche2 = conteneurObjet.RechercherObjets(nomObjet);  // Consultation Item
            foreach (Objet o in resRecherche2)
            {
                Console.WriteLine(o);
            }
        }

        static void TestBiome()
        {
            EBiomes biome = EBiomes.Ender | EBiomes.Foret;
            Console.WriteLine(biome);
            biome |= EBiomes.Foret;
            Console.WriteLine(biome);
            biome |= EBiomes.Desert;
            Console.WriteLine(biome);
        }

        static void TestItemsCraftablesAvec(ManagerObjets conteneurObjet)
        {
            foreach (Objet obj in conteneurObjet.Objets[6].ItemCraftableAvec)
            {
                Console.WriteLine(obj);
            }
            foreach (Objet obj in conteneurObjet.ItemCraftableAvec("item   SWAG"))
            {
                Console.WriteLine(obj);
            }

        }

        // Méthode du ultra bled, mais elle marche. Le chemin d'accès au fichier est dans UCrAft/TestsFonctionnels/bin/Debug/XML
        static void TestSauvegarde(ManagerObjets conteneurObjet)
        {
            IPersistanceManager saver = new PersXMLLinQ();

            saver.Sauvegarde(conteneurObjet.Objets);
        }

        static void TestLecture()
        {
            IPersistanceManager saver = new PersXMLLinQ();
            List<Objet> objetsLus = saver.Charge();
            foreach (Objet o in objetsLus)
            {
                Console.WriteLine(o);
            }
        }
    }
}
