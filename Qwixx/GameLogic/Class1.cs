using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace GameLogic;

public record Game(IEnumerable<Player> Players )
{
    public GameState GameState { get; set; } = GameState.Shaking;
    public int CurrentPlayer { get; set; } = 0;
    public Game NextPlayer() => this with { 
        CurrentPlayer = (CurrentPlayer + 1) % Players.Count(),
        GameState = GameState.Shaking
    };
    public bool CanFinishTurn() => GameState == GameState.Round2;
}

public record Player(string Name, PlayCard Card);

public record Board(int WhiteDice1, int WhiteDice2, int RedDice, int YellowDice, int GreenDice, int BlueDice)
{
    private static Random rnd = new Random();
    private static int ShakeDice() => rnd.Next(1, 6);
    public static Board Shake() => new Board(ShakeDice(), ShakeDice(), ShakeDice(), ShakeDice(), ShakeDice(), ShakeDice());
}

public record PlayCard()
{
    public IReadOnlyList<RowCell> RedRow { get; init; } = Enumerable.Range(2, 12).Select(n => new RowCell(false, n)).ToImmutableList();
    public IReadOnlyList<RowCell> YellowRow { get; init; } = Enumerable.Range(2, 12).Select(n => new RowCell(false, n)).ToImmutableList();
    public IReadOnlyList<RowCell> GreenRow { get; init; } = Enumerable.Range(12, 2).Select(n => new RowCell(false, n)).ToImmutableList();
    public IReadOnlyList<RowCell> BlueRow { get; init; } = Enumerable.Range(12, 2).Select(n => new RowCell(false, n)).ToImmutableList();

    public int MissedChanges { get; init; } = 0;
}

public record RowCell(bool Signed, int Number);

public enum GameState
{
    Shaking,
    Round1,
    Round2
}


public static class Actions
{
    public static bool CanSignCell(IReadOnlyList<RowCell> row, int cellNumber) => row switch
    {
        // it is the last cell
        [.., RowCell(_, var n)] when n == cellNumber => row.Count((Func<RowCell, bool>)(cell => (bool)cell.Signed)) >= 5,       
        _ => row.SkipWhile(cell => cell.Number != cellNumber).All((Func<RowCell, bool>)(cell => cell.Signed is false)),
    };

    public static IReadOnlyList<RowCell> SignCell(IReadOnlyList<RowCell> row, int cellNumber)
    {
        return row.Select(cell => cell.Number == cellNumber ? cell with { Signed = true } : cell).ToImmutableList();
    }
}