using Modele;
using NUnit.Framework;

namespace TestUnitaire
{
    class TestLocalisation
    {
        [Test]
        public void ContrainteValidite()
        {
            Localisation localisation = new Localisation(0, 21, EBiomes.ExtremeHills);
            Assert.True(localisation.IsValide()); //Normal

            localisation = new Localisation(-12, 0, EBiomes.ExtremeHills);
            Assert.False(localisation.IsValide());//couche minimum inferieur à zero

            localisation = new Localisation(0, 1000, EBiomes.ExtremeHills);
            Assert.False(localisation.IsValide());//couche maximum supperieur à 128

            localisation = new Localisation(0, 21, EBiomes.Indefini);
            Assert.False(localisation.IsValide());//Biome indefini

            localisation = new Localisation(100, 21, EBiomes.Grottes);
            Assert.False(localisation.IsValide());//couche minimum superieur a la couche maximum
        }

    }
}
