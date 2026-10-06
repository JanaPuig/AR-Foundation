public enum MonsterType
{
    Planta,
    Fuego,
    Agua,
    Electrico,
    Volador
}

public enum RoundResult
{
    Win,
    Lose,
    Draw
}

public static class CombatRules
{
    public static RoundResult Evaluate(MonsterType player, MonsterType enemy)
    {
        if (player == enemy)
            return RoundResult.Draw;

        switch (player)
        {
            case MonsterType.Planta:

                if (enemy == MonsterType.Agua || enemy == MonsterType.Volador)
                {
                    return RoundResult.Win;
                }
                else
                {
                    return RoundResult.Lose;
                }

            case MonsterType.Fuego:

                if (enemy == MonsterType.Planta ||enemy == MonsterType.Electrico)
                {
                    return RoundResult.Win;
                }
                else
                {
                    return RoundResult.Lose;
                }

            case MonsterType.Agua:

                if (enemy == MonsterType.Fuego || enemy == MonsterType.Electrico)
                {
                    return RoundResult.Win;
                }
                else
                {
                    return RoundResult.Lose;
                }

            case MonsterType.Electrico:

                if (enemy == MonsterType.Planta || enemy == MonsterType.Volador)
                {
                    return RoundResult.Win;
                }
                else
                {
                    return RoundResult.Lose;
                }

            case MonsterType.Volador:

                if (enemy == MonsterType.Agua ||enemy == MonsterType.Fuego)
                {
                    return RoundResult.Win;
                }
                else
                {
                    return RoundResult.Lose;
                }

            default:
                return RoundResult.Draw;
        }
    }
}