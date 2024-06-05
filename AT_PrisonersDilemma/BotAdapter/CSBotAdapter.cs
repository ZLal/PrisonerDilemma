using AT_PrisonersDilemma.ScriptLoader;

namespace AT_PrisonersDilemma.BotAdapter
{
    public class CSBotAdapter : IBot
    {
        private readonly CSScriptLoader ScriptLoader;
        public CSBotAdapter(string name, CSScriptLoader scriptLoader)
        {
            Name = name;
            ScriptLoader = scriptLoader;
        }

        public static IBot CreateBot(string name, string script)
        {
            CSScriptLoader scriptLoader = new();
            scriptLoader.LoadAssembly(script);
            return new CSBotAdapter(name, scriptLoader);
        }

        public string Name { get; init; }

        public BotAction NextIteration()
        {
            object? objAction = ScriptLoader.ExecuteMethod($"AT_PrisonersDilemma.BotScripts.{Name}.{nameof(NextIteration)}", null)
                ?? throw new InvalidOperationException($"null result in {Name}.{nameof(NextIteration)}");
            int action = Convert.ToInt32(objAction);
            return action == 0 ? BotAction.Cooperate : BotAction.Defect;
        }

        public void Reset()
        {
            _ = ScriptLoader.ExecuteMethod($"AT_PrisonersDilemma.BotScripts.{Name}.{nameof(Reset)}", null);
        }

        public void SetResult(BotAction player, BotAction opponent)
        {
            object[] parameters = { player == BotAction.Cooperate ? 0 : 1, opponent == BotAction.Cooperate ? 0 : 1 };
            _ = ScriptLoader.ExecuteMethod($"AT_PrisonersDilemma.BotScripts.{Name}.{nameof(SetResult)}", parameters);
        }
    }
}
