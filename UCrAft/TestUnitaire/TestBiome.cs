using Modele;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestUnitaire
{
    public class TestBiome
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ToutLesBiomesSontDifferents()
        {
            List<int> valeursPrecedentes = new List<int>();
            foreach (int newVal in Enum.GetValues(typeof(EBiomes)))
            {
                foreach (EBiomes biomesPrecedent in valeursPrecedentes)
                {
                    Assert.AreNotEqual(biomesPrecedent, newVal);
                }
                valeursPrecedentes.Add(newVal);
            }
        }
    }
}
