using System;
using System.Collections.Generic;
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
    public bool CanFinish() => GameState == GameState.Round2
}

public record Player(string Name, PlayCard Card);

public record Board(int WhiteDice1, int WhiteDice2, int RedDice, int YellowDice, int GreenDice, int BlueDice)
{
    private static Random rnd = new Random();
    private static int shakeDice() => rnd.Next(1, 6);
    public static Board Shake() => new Board(shakeDice(), shakeDice(), shakeDice(), shakeDice(), shakeDice(), shakeDice());
}

public record PlayCard()
{
    public IEnumerable<RowCell> RedRow { get; init; } = Enumerable.Range(2, 12).Select(n => new RowCell(false, n));
    public IEnumerable<RowCell> YellowRow { get; init; } = Enumerable.Range(2, 12).Select(n => new RowCell(false, n));
    public IEnumerable<RowCell> GreenRow { get; init; } = Enumerable.Range(12, 2).Select(n => new RowCell(false, n));
    public IEnumerable<RowCell> BlueRow { get; init; } = Enumerable.Range(12, 2).Select(n => new RowCell(false, n));

    public int MissedChanges { get; init; } = 0;
}

public record RowCell(bool signed, int number);

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
        [.., RowCell(_, var n)] when n == cellNumber => row.Count(cell => cell.signed) >= 5,       
        _ => row.SkipWhile(cell => cell.number != cellNumber).All(cell => cell.signed is false),
    };
}