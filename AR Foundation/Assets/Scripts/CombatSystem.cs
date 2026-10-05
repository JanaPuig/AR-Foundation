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
        if (player == enemy) return RoundResult.Draw;

        switch (player)
        {
            case MonsterType.Planta:
                return (enemy == MonsterType.Agua || enemy == MonsterType.Electrico) ? RoundResult.Win : RoundResult.Lose;

            case MonsterType.Fuego:
                return (enemy == MonsterType.Planta) ? RoundResult.Win : RoundResult.Lose;

            case MonsterType.Agua:
                return (enemy == MonsterType.Fuego) ? RoundResult.Win : RoundResult.Lose;

            case MonsterType.Electrico:
                return (enemy == MonsterType.Agua || enemy == MonsterType.Volador) ? RoundResult.Win : RoundResult.Lose;

            case MonsterType.Volador:
                return (enemy == MonsterType.Planta) ? RoundResult.Win : RoundResult.Lose;

            default:
                return RoundResult.Draw;
        }
    }
}
