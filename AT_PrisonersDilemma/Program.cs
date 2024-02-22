// See https://aka.ms/new-console-template for more information
using AT_PrisonersDilemma;
using AT_PrisonersDilemma.Bots;

Console.WriteLine("Loading █");

Random random = new();
Game game = new(random)
{
    CustomIteration = 10
};

game.AddBot(new AlwaysCooperate());
game.AddBot(new AlwaysDefect());
game.AddBot(new RandomBot(random));
game.AddBot(new TitForTat());

game.PlayGame();
var resultData = game.GetResults();

Console.WriteLine();
Console.WriteLine("Results");
Console.WriteLine("-------------------");

var defaultForeColor = Console.ForegroundColor;
var defaultBackColor = Console.BackgroundColor;
foreach (var result in resultData.Results)
{
    Console.ForegroundColor = defaultForeColor;
    Console.BackgroundColor = defaultBackColor;

    Console.WriteLine($"{result.Player1} vs {result.Player2} (Score {result.ScoreList.Sum(x => x.Item3)} vs {result.ScoreList.Sum(x => x.Item4)})");
    foreach (var item in result.ScoreList)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = item.Item1 == BotAction.Cooperate ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write(item.Item3);
        Console.BackgroundColor = item.Item2 == BotAction.Cooperate ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write(item.Item4);

        //Console.ForegroundColor = defaultForeColor;
        Console.BackgroundColor = defaultBackColor;
        Console.Write(" ");
    }
    Console.WriteLine();
    Console.WriteLine();
}

Console.ForegroundColor = defaultForeColor;
Console.BackgroundColor = defaultBackColor;

Console.WriteLine("Leader board");
Console.WriteLine("-------------------");
resultData.LeaderBoard.ForEach(x => Console.WriteLine(x.Key + " - " + x.Value));

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("Press enter to exit");
Console.ReadLine();
