using Modele;
using NUnit.Framework;
using System.Collections.Generic;


namespace TestUnitaire
{
    class TestCraft
    {
        [Test]
        public void Valide()
        {
            Materiau mat = new Materiau("materiau", "Chemin", "Description", "Methode de recolte", new Localisation(0, 0, EBiomes.Desert));
            Assert.True(new Craft(new Dictionary<EPosition, Objet> { [EPosition.BasGauche] = mat }, 2).IsValide());//Cas normal

            Assert.True(new Craft(new Dictionary<EPosition, Objet>
            {
                [EPosition.BasGauche] = mat,
                [EPosition.HautDroite] = mat,
            }, 2).IsValide());//Cas normal

            Assert.False(new Craft(new Dictionary<EPosition, Objet>
            {
                [EPosition.BasGauche] = mat,
                [EPosition.HautDroite] = mat,
            }, 0).IsValide());//nombre d'objets à la création nul

            Assert.False(new Craft(new Dictionary<EPosition, Objet>
            {
            }, 2).IsValide());//nombre d'objets utilisés pour le craft nul
        }
    }
}
