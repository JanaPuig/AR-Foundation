using System.Collections.Generic;
using UnityEngine;

//Los 5 tipos de monstruo del juego.
public enum ElementType
{
    Fuego,      
    Planta,    
    Agua,       
    Electrico,  
    Volador     
}

//Resultado de una ronda visto desde el jugador.
public enum RoundResult
{
    Victoria,
    Derrota,
    Empate
}


// Sistema de tipos: cada tipo gana a 2 y pierde contra 2




public static class ElementSystem
{
    // Clave = tipo atacante, valor = los 2 tipos a los que gana.
    private static readonly Dictionary<ElementType, ElementType[]> WinsAgainst =
        new Dictionary<ElementType, ElementType[]>
        {
            { ElementType.Fuego,     new[] { ElementType.Planta,    ElementType.Electrico } },
            { ElementType.Planta,    new[] { ElementType.Agua,      ElementType.Volador   } },
            { ElementType.Agua,      new[] { ElementType.Fuego,     ElementType.Electrico } },
            { ElementType.Electrico, new[] { ElementType.Planta,    ElementType.Volador   } },
            { ElementType.Volador,   new[] { ElementType.Fuego,     ElementType.Agua      } },
        };

    private static readonly ElementType[] AllTypes =
        (ElementType[])System.Enum.GetValues(typeof(ElementType));

   
    public static bool Beats(ElementType attacker, ElementType defender)
    {
        return System.Array.IndexOf(WinsAgainst[attacker], defender) >= 0;
    }

   
    public static RoundResult Resolve(ElementType player, ElementType rival)
    {
        if (player == rival) return RoundResult.Empate;
        return Beats(player, rival) ? RoundResult.Victoria : RoundResult.Derrota;
    }

 
    public static ElementType[] GetStrengths(ElementType type)
    {
        return (ElementType[])WinsAgainst[type].Clone();
    }

   
    public static ElementType[] GetWeaknesses(ElementType type)
    {
        var result = new List<ElementType>(2);
        foreach (ElementType other in AllTypes)
        {
            if (Beats(other, type)) result.Add(other);
        }
        return result.ToArray();
    }

   
    public static ElementType RandomType()
    {
        return AllTypes[Random.Range(0, AllTypes.Length)];
    }

    
    public static ElementType RandomTypeExcept(ElementType excluded)
    {
        ElementType type;
        do { type = RandomType(); } while (type == excluded);
        return type;
    }
}
