using System.IO;
using AT_PrisonersDilemma.BotAdapter;
using AT_PrisonersDilemma.Bots;
using AT_PrisonersDilemma.ScriptLoader;

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
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string fileExtension = Path.GetExtension(file);
                string content = File.ReadAllText(file);
                if (fileExtension == ".cs")
                {
                    CSScriptLoader scriptLoader = new CSScriptLoader();
                    scriptLoader.LoadAssembly(content);
                    result.Add(new CSBotAdapter(fileName, scriptLoader));
                }
                else if (fileExtension == ".py")
                {
                    PYScriptLoader scriptLoader = new PYScriptLoader();
                    scriptLoader.LoadAssembly(content);
                    result.Add(new PYBotAdapter(fileName, scriptLoader));
                }
            }
            return result;
        }
    }
}
