using System;

namespace Modele
{
    /// <summary>
    /// Represente les biomes du jeu
    /// Peut être combiné (utile pour la déserialization)
    /// </summary>
    [Flags]
    public enum EBiomes
    {
        Indefini = 0b0000000000000000,
        Marais = 0b0000000000000001,
        Foret = 0b0000000000000010,
        Taiga = 0b0000000000000100,
        Desert = 0b0000000000001000,
        Plaine = 0b0000000000010000,
        Toundra = 0b0000000000100000,
        Champignon = 0b0000000001000000,
        Jungle = 0b0000000010000000,
        Ocean = 0b0000000100000000,
        ExtremeHills = 0b0000001000000000,
        Nether = 0b0000010000000000,
        Ender = 0b0000100000000000,
        Grottes = 0b0001000000000000,
    }
}
