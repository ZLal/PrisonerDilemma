using AT_PrisonersDilemma.ScriptLoader;

namespace AT_PrisonersDilemma.BotAdapter
{
    public class HttpBotAdapter : IBot
    {
        private readonly HttpScriptLoader ScriptLoader;
        public HttpBotAdapter(string name, HttpScriptLoader scriptLoader)
        {
            Name = name;
            ScriptLoader = scriptLoader;
        }

        public static IBot CreateBot(string name, string script)
        {
            HttpScriptLoader scriptLoader = new();
            scriptLoader.LoadAssembly(script);
            return new HttpBotAdapter(name, scriptLoader);
        }

        public string Name { get; init; }

        public BotAction NextIteration()
        {
            object? objAction = ScriptLoader.ExecuteMethod($"{nameof(NextIteration)}", null)
                ?? throw new InvalidOperationException($"null result in {Name}.{nameof(NextIteration)}");
            int action = Convert.ToInt32(objAction);
            return action == 0 ? BotAction.Cooperate : BotAction.Defect;
        }

        public void Reset()
        {
            _ = ScriptLoader.ExecuteMethod($"{nameof(Reset)}", null);
        }

        public void SetResult(BotAction player, BotAction opponent)
        {
            Dictionary<string, string> parameters = new()
            {
                { "Player", player == BotAction.Cooperate ? "0" : "1" },
                { "Opponent", opponent == BotAction.Cooperate ? "0" : "1" }
            };
            _ = ScriptLoader.ExecuteMethod($"{nameof(SetResult)}", parameters);
        }
    }
}
