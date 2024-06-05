using AT_PrisonersDilemma.BotAdapter;
using AT_PrisonersDilemma.Bots;

namespace AT_PrisonersDilemma
{
    public class BotFactory
    {
        private readonly Random random;
        public BotFactory(Random random)
        {
            this.random = random;
        }

        public List<IBot> GetBots()
        {
            List<IBot> result = GetDefaultBots();
            result.AddRange(GetScriptedBots());
            return result;
        }

        public List<IBot> GetDefaultBots()
        {
            return new List<IBot>
            {
                new AlwaysCooperate(),
                new AlwaysDefect(),
                new RandomBot(random),
                new TitForTat()
            };
        }

        public List<IBot> GetScriptedBots()
        {
            List<IBot> result = new();
            List<string> files = Directory.EnumerateFiles("BotScripts").ToList();
            Dictionary<string, Func<string, string, IBot>> adapterLookup = new()
                {
                    { ".cs", CSBotAdapter.CreateBot },
                    { ".py", PYBotAdapter.CreateBot },
                    { ".js", JSBotAdapter.CreateBot },
                    { ".http", HttpBotAdapter.CreateBot },
                };
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string fileExtension = Path.GetExtension(file);
                string content;
                if (adapterLookup.TryGetValue(fileExtension, out var createBot))
                {
                    content = File.ReadAllText(file);
                    result.Add(createBot(fileName, content));
                }
            }
            return result;
        }
    }
}
