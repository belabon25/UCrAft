using Modele;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Data
{
    public class Stub : IPersistanceManager
    {
        public List<Objet> Charge()
        {
            Objet m = new Materiau("bon", "IconeRechercher.png", "bon", "bon", new Localisation(0, 10, EBiomes.Desert));
            Objet m1 = new Materiau("azer", "PlaceHolder.png", "bon", "bon", new Localisation(0, 10, EBiomes.Desert));
            Objet m2 = new Materiau("bonazer", "PlaceHolder.png", "bon", "bon", new Localisation(0, 10, EBiomes.Desert));
            Objet m3 = new Materiau("azfsbon", "ImageVide.png", "bon", "bon", new Localisation(0, 10, EBiomes.Ender));
            Objet m4 = new Materiau("azfsbonqf", "PlaceHolder.png", "bon", "bon", new Localisation(0, 10, EBiomes.Desert | EBiomes.Champignon));
            Objet i1 = new Item("item1", "PlaceHolder.png", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = m, [EPosition.MilieuMilieu] = m1 }, 1), 2, 3);
            Objet i2 = new Item("item2", "PlaceHolder.png", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = m, [EPosition.MilieuMilieu] = i1 }, 1), 2, 3);
            Objet i3 = new Item("item   SWAG", "ImageVide.png", "Description", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = i1, [EPosition.MilieuMilieu] = i1 }, 1), 2, 3);
            Objet i4 = new Item("itemHey", "PlaceHolder.png", "ItemHey vous dit bonjour", new Craft(new Dictionary<EPosition, Objet> { [EPosition.HautGauche] = m, [EPosition.HautDroite] = m, [EPosition.BasMilieu] = m }, 1), 5, 1);

            List<Objet> listeObjet = new List<Objet>();
            listeObjet.Add(m);
            listeObjet.Add(m1);
            listeObjet.Add(m2);
            listeObjet.Add(m3);
            listeObjet.Add(m4);
            listeObjet.Add(i1);
            listeObjet.Add(i2);
            listeObjet.Add(i3);
            listeObjet.Add(i4);
            return listeObjet;
        }

        public void Sauvegarde(ReadOnlyCollection<Objet> l) => Debug.WriteLine("Sauvegarde appellée pour le stub.");
    }
}
