using Data;
using Modele;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestUnitaire
{
    class TestConteneurObjet
    {
        private IPersistanceManager chargeur = new Stub();

        [SetUp]
        public void Setup()
        {
        }


        [Test] // J'ai essayé de faire avec TestCase mais je me suis heurté à une chaine de problemes
        public void AjoutObjets()
        {
            ManagerObjets conteneurObjet = new ManagerObjets();
            Objet m = new Materiau("bon", "bon", "bon", "bon", new Localisation(0, 10, EBiomes.Desert));

            Assert.AreEqual(0, conteneurObjet.Objets.Count);

            Assert.DoesNotThrow(() => conteneurObjet.AjouterObjet(m));

            Assert.AreEqual(1, conteneurObjet.Objets.Count);

            m = new Materiau("Mauvais", "", "bon", "bon", new Localisation(0, 10, EBiomes.Desert));

            Assert.Throws<MissingFieldException>(() => conteneurObjet.AjouterObjet(m));

            Assert.AreEqual(1, conteneurObjet.Objets.Count);

            Objet mPresent = new Materiau("bon", "present", "present", "present", new Localisation(0, 10, EBiomes.Desert));
            Assert.Throws<ArgumentException>(() => conteneurObjet.AjouterObjet(mPresent));
        }

        [Test]
        public void SupprimerObjets()
        {
            ManagerObjets conteneurObjet = new ManagerObjets(chargeur.Charge().ToArray());

            int nombreObjetDepart = conteneurObjet.Objets.Count;
            if (nombreObjetDepart == 0)
            {
                Assert.Fail("Aucun objet load par le stub");
            }

            Objet o = conteneurObjet.Objets[0];
            conteneurObjet.SupprimerObjet(o.Nom);
            Assert.AreEqual(nombreObjetDepart - 1, conteneurObjet.Objets.Count);

            Assert.Throws<ArgumentException>(() => conteneurObjet.SupprimerObjet(""));
        }

        [Test]
        public void RechercherObjet()
        {
            ManagerObjets conteneurObjet = new ManagerObjets();

            Objet o1 = new Materiau("materiau1", "CheminInconnu", "un materiau", "à la main", new Localisation(0, 5, EBiomes.Ender));
            Objet o2 = new Materiau("materiau2", "CheminInconnu", "un materiau", "à la main", new Localisation(0, 5, EBiomes.Ender));
            Objet o3 = new Materiau("materiau3", "CheminInconnu", "un materiau", "à la main", new Localisation(0, 5, EBiomes.Ender));

            conteneurObjet.AjouterObjet(o1, o2, o3);

            IEnumerable<Objet> resRecherche = conteneurObjet.RechercherObjets("materiau");

            CollectionAssert.AreEquivalent(resRecherche, conteneurObjet.Objets);

            resRecherche = conteneurObjet.RechercherObjets("1");
            uint nbObjetTrouve = 0;
            foreach (var o in resRecherche)
            {
                Assert.AreEqual(o1, o);
                nbObjetTrouve++;
            }
            Assert.AreEqual(1, nbObjetTrouve);
        }

        [Test]
        public void ItemCraftableAvec()
        {
            Objet m1 = new Materiau("materiau1", "CheminInconnu", "un materiau", "à la main", new Localisation(0, 5, EBiomes.Ender));
            Objet m2 = new Materiau("materiau2", "CheminInconnu", "un materiau", "à la main", new Localisation(0, 5, EBiomes.Ender));
            Objet m3 = new Materiau("materiau3", "CheminInconnu", "un materiau", "à la main", new Localisation(0, 5, EBiomes.Ender));
            Item i1 = new Item("item1", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = m1, [EPosition.MilieuMilieu] = m2 }, 1), 2, 3);
            Item i2 = new Item("item2", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = m1 }, 1), 2, 3);

            ManagerObjets conteneurObjet = new ManagerObjets();

            conteneurObjet.AjouterObjet(m1, m2, m3, i1, i2);

            Assert.Throws<ArgumentException>(() => conteneurObjet.ItemCraftableAvec("mat"));

            List<Item> resultatRecherche = new List<Item>(conteneurObjet.ItemCraftableAvec("materiau1"));
            Assert.True(resultatRecherche.Contains(i1));
            Assert.True(resultatRecherche.Contains(i2));

            resultatRecherche.Clear();
            resultatRecherche.AddRange(conteneurObjet.ItemCraftableAvec("materiau2"));
            Assert.True(resultatRecherche.Contains(i1));
            Assert.False(resultatRecherche.Contains(i2));

            resultatRecherche.Clear();
            resultatRecherche.AddRange(conteneurObjet.ItemCraftableAvec("materiau3"));
            Assert.False(resultatRecherche.Contains(i1));
            Assert.False(resultatRecherche.Contains(i2));
        }
    }
}
