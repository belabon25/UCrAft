using Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Data
{
    /// <summary>
    /// Classe permettant d'avoir un persistance des données de l'application. Ecrite avec l'utilisation de LinQToXML revisitée
    /// </summary>
    public class PersXMLLinQ : IPersistanceManager
    {
        private string CheminFichier => Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "..//XML"), "objetsSauvegardes.xml");

        /// <summary>
        /// Méthode appelée pour charger les données du fichier écrit au chemin définit par cheminFichier.
        /// </summary>
        /// <returns>Retourne la liste des Objets présents dans le fichier.</returns>
        public List<Objet> Charge()
        {
            List<Objet> listeRetournee = new List<Objet>();
            XDocument arbreLu = XDocument.Load(CheminFichier);

            // Créé une liste d'item (sans crafts) à partir du fichier
            var items = arbreLu.Descendants("item").Select(itemLu => new Item(itemLu.Attribute("nom").Value,
                                                                              itemLu.Attribute("Image").Value,
                                                                              itemLu.Attribute("DescriptionGenerale").Value,
                                                                              XmlConvert.ToInt32(itemLu.Attribute("Efficacite").Value),
                                                                              XmlConvert.ToInt32(itemLu.Attribute("Importance").Value)
                                                                              )).ToArray();


            // Créé une liste de matériaux à partir du fichier
            var materiaux = arbreLu.Descendants("materiau").Select(materiauLu => new Materiau(materiauLu.Attribute("nom").Value,
                                                                  materiauLu.Attribute("Image").Value,
                                                                  materiauLu.Attribute("DescriptionGenerale").Value,
                                                                  materiauLu.Attribute("MethodeRecolte").Value,
                                                                  new Localisation(XmlConvert.ToInt32(materiauLu.Attribute("CoucheMin").Value),
                                                                                   XmlConvert.ToInt32(materiauLu.Attribute("CoucheMax").Value),
                                                                                   (EBiomes)XmlConvert.ToUInt32(materiauLu.Attribute("Biomes").Value))
                                                                  )).ToArray();


            listeRetournee.AddRange(materiaux);
            listeRetournee.AddRange(items);

            //Pour chaque craft, on cherche le seul item dont le nom correspond au nom stocké dans le fichier, puis on ajoute le craft à cet item.
            foreach (XElement craft in arbreLu.Descendants("craft"))
            {
                items.SingleOrDefault(i => i.Nom.Equals(craft.Attribute("nomItem").Value)).AddCraft(CreateurCraft(craft, listeRetournee));
            }

            return listeRetournee;
        }


        /// <summary>
        /// Découpe la donnée sérializée lue, et construit un craft à partir de celle-ci.
        /// </summary>
        /// <param name="craftLu">Le XElement lu par la méthode Charge()</param>
        /// <param name="tousLesObjets">Liste de tous les objets lus par la méthode Charge()</param>
        /// <returns>Retourne le craft créé</returns>
        private static Craft CreateurCraft(XElement craftLu, List<Objet> tousLesObjets)
        {
            Dictionary<EPosition, Objet> pattern = new Dictionary<EPosition, Objet>();
            string craftStr = craftLu.Attribute("craftItem").Value; // { [HautGauche] = bon, [MilieuMilieu] = Indéfini,  } Exemple typique de string reçu, évolution au fil du découpage.
            uint nbCreation = XmlConvert.ToUInt32(craftLu.Attribute("nbCraftes").Value);

            craftStr = craftStr.Trim(',', ' ', '{', '}'); // [HautGauche] = bon, [MilieuMilieu] = Indéfini
            foreach (string s in craftStr.Split(',')) // Découpe en 2 strings : "[HautGauche] = bon" et " [MilieuMilieu] = Indéfini"
            {
                IEnumerable<string> sortie = s.Split('=').Select(str => str.Trim('[', ']', ' ')); // IEnumerable de deux strings : HautGauche, bon

                pattern.Add(Enum.Parse<EPosition>(sortie.First()), tousLesObjets.Find(o => o.Nom.Equals(sortie.Last()))); // Création et ajout au pattern d'une EPosition, composée d'une Localisation et d'un Objet.
            }

            return new Craft(pattern, nbCreation);
        }

        /// <summary>
        /// Sert à éviter une erreur lorsque le XAttribute voulant être écrit est vide.
        /// </summary>
        /// <param name="nom">Nom passé pour le XAttribute</param>
        /// <param name="value">Valeur passée pour le XAttribute</param>
        /// <returns>Le XAttribute si la valeur n'est pas vide,sinon pas de XAttribute du tout</returns>
        private static XAttribute IfXAttributeIsNull(string nom, object value)
        {
            if (value is null)
            {
                return null;
            }
            else
            {
                return new XAttribute(nom, value);
            }
        }

        /// <summary>
        /// Permet la sauvegarde d'une Collection d'objets donnée en paramètre.
        /// La structure du fichier est la suivante :
        ///  <Data>
        ///     <Items>
        ///         <item/>
        ///     </Items>
        ///     <Materiaux>
        ///         <materiau/>
        ///     </Materiaux>
        ///     <Crafts>
        ///         <craft/>
        ///     </Crafts>
        ///  </Data>
        /// </summary>
        /// <param name="listeObjets"> La Collection d'objet voulant être sauvegardée</param>
        public void Sauvegarde(ReadOnlyCollection<Objet> listeObjets)
        {
            XDocument fichierSauvegarde = new XDocument();

            // On sépare les items des matériaux
            var listeItems = listeObjets.Where(o => o is Item).Select(o => o as Item);
            var listeMateriaux = listeObjets.Where(o => o is Materiau).Select(o => o as Materiau);

            //On transforme chaque item de la liste en XElement et on transforme ses attributs en XAttribute (sauf le craft).
            var itemSauv = listeItems.Select(item => new XElement("item",
                                                          IfXAttributeIsNull("nom", item.Nom),
                                                          IfXAttributeIsNull("Image", item.ImageChemin),
                                                          IfXAttributeIsNull("DescriptionGenerale", item.DescriptionGenerale),
                                                          IfXAttributeIsNull("Efficacite", item.Efficacite),
                                                          IfXAttributeIsNull("Importance", item.Importance)
                                                          ));

            //On transforme chaque matériau de la liste en XElement et on transforme ses attributs en XAttribute.
            var materiauxSauv = listeMateriaux.Select(materiau => new XElement("materiau",
                                                                      IfXAttributeIsNull("nom", materiau.Nom),
                                                                      IfXAttributeIsNull("Image", materiau.ImageChemin),
                                                                      IfXAttributeIsNull("DescriptionGenerale", materiau.DescriptionGenerale),
                                                                      IfXAttributeIsNull("CoucheMin", materiau.LocalisationMateriau.CoucheMin),
                                                                      IfXAttributeIsNull("CoucheMax", materiau.LocalisationMateriau.CoucheMax),
                    /*Le biome sera reconverti à la lecture*/         IfXAttributeIsNull("Biomes", (uint)materiau.LocalisationMateriau.Biomes),
                                                                      IfXAttributeIsNull("MethodeRecolte", materiau.MethodeDeRecolte)
                                                                      ));

            // Le craft doit être stocké à part car il est impossible de le lire si il est passé en XAttribute d'un item. 
            var craftsSauv = listeItems.Select(item => new XElement("craft",
                                              IfXAttributeIsNull("nomItem", item.Nom),
                                              IfXAttributeIsNull("craftItem", item.CraftItem),
                                              IfXAttributeIsNull("nbCraftes", item.CraftItem.NombreLorsDeLaCreation)
                                              ));

            // Ajout des données dans le XDocument.
            fichierSauvegarde.Add(new XElement("Data",
                new XElement("Items", itemSauv),
                new XElement("Materiaux", materiauxSauv),
                new XElement("Crafts", craftsSauv)
                ));

            //Activation de l'indentation pour faciliter la lecture humaine
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            //Création du dossier au cas ou il n'existerait pas
            if (!Directory.Exists("..//XML"))
                Directory.CreateDirectory("..//XML");

            // Utilisation d'un TextWriter et du XmlWriter pour écrire le XDocument  
            using (TextWriter tw = File.CreateText(CheminFichier))
            using (XmlWriter writer = XmlWriter.Create(tw, settings))
            {
                fichierSauvegarde.Save(writer);
            }
            Debug.WriteLine("Sauvegarde Effectuée !");
        }
    }
}
