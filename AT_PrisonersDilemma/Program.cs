// See https://aka.ms/new-console-template for more information
using AT_PrisonersDilemma;

Console.WriteLine("Loading █");

Game game = new()
{
    StartIteration = 20
};

// Enable http in BotFactory
//Console.WriteLine("Waiting for webserver startup");
//Thread.Sleep(10_000);

Console.WriteLine("Playing");
BotFactory botFactory = new();
List<IBot> bots = botFactory.GetBots();
bots.ForEach(bot => game.AddBot(bot));

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
