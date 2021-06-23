using Modele;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestUnitaire
{
    class TestObjet
    {
        [Test]
        public void EqualsObjets()
        {
            Materiau mat = new Materiau("materiau", "Chemin", "Description", "Methode de recolte", new Localisation(0, 0, EBiomes.Desert));
            Item i = new Item("item1", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 2, 3);

            Assert.AreNotEqual(mat, i);

            Item i2 = i;

            Assert.AreEqual(i, i2);

            Item i3 = new Item("item1", "Chemin252", "Description42", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 2), 2, 3);
            Assert.AreEqual(i3, i);

            Item i4 = new Item("item2", "Chemin252", "Description42", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 2), 2, 3);
            Assert.AreNotEqual(i3, i4);

            Assert.AreNotEqual(i4, null);

            ManagerObjets c = new ManagerObjets();
            Assert.AreNotEqual(i4, c);

        }

        [Test]
        public void ContraintesObjet()
        {
            Materiau mat = new Materiau("materiau", "Chemin", "Description", "Methode de recolte", new Localisation(0, 0, EBiomes.Desert));
            Assert.True(mat.IsValide());//normale
            Assert.True(mat.Valide);//normale

            Item i = new Item("", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 2, 3);
            Assert.False(i.IsValide());//nom vide

            i = new Item("item2", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 2, 3);
            Assert.True(i.IsValide());//normale

            i = new Item("item2", "", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 2, 3);
            Assert.False(i.IsValide());//Chemin image vide

            i = new Item("item2", "   ", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 2, 3);
            Assert.False(i.IsValide());//Chemin image blanc

            i = new Item("item2", "  z ", " ", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 2, 3);
            Assert.False(i.IsValide());//Description blanche
            Assert.False(i.Valide);//Description blanche
        }

        [Test]
        public void ContraintesItem()
        {
            Materiau mat = new Materiau("materiau", "Chemin", "Description", "Methode de recolte", new Localisation(0, 0, EBiomes.Desert));
            Item i = new Item("item1", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 2, 3);
            Assert.True(i.IsValide()); //Normale
            Assert.True(i.Valide); //Normale

            Item i2 = new Item("item1", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = i }, 1), 2, 3);
            Assert.False(i2.IsValide());//reference à lui meme via i

            Item i3 = new Item("item1", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 5, 6);
            Assert.False(i3.IsValide());//Importance > 5

            Item i4 = new Item("item1", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 5, -1);
            Assert.False(i3.IsValide());//Importance < 0

            Item i5 = new Item("item1", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 6, 2);
            Assert.False(i3.IsValide());//Efficacité > 5

            Item i6 = new Item("item1", "Chemin", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = mat }, 1), 3, -1);
            Assert.False(i3.IsValide());//Efficacité < 0
            Assert.False(i3.Valide);//Efficacité < 0
        }

        [Test]
        public void ContrainteMateriau()
        {
            Materiau mat = new Materiau("materiau", "Chemin", "Description", "Methode de recolte", new Localisation(0, 0, EBiomes.Desert));
            Assert.True(mat.IsValide());//normale

            mat = new Materiau("materiau", "Chemin", "Description", "  ", new Localisation(0, 0, EBiomes.Desert));
            Assert.False(mat.IsValide()); //methode de recolte vide

            //Les tests pour la localisation sont dans un fichiers specifique
        }



    }
}
