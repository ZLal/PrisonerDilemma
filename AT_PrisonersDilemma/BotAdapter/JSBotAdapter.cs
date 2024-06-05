using AT_PrisonersDilemma.ScriptLoader;

namespace AT_PrisonersDilemma.BotAdapter
{
    public class JSBotAdapter : IBot
    {
        private readonly JSScriptLoader ScriptLoader;
        public JSBotAdapter(string name, JSScriptLoader scriptLoader)
        {
            Name = name;
            ScriptLoader = scriptLoader;
        }

        public static IBot CreateBot(string name, string script)
        {
            JSScriptLoader scriptLoader = new();
            scriptLoader.LoadAssembly(script);
            return new JSBotAdapter(name, scriptLoader);
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
            object[] parameters = { player == BotAction.Cooperate ? 0 : 1, opponent == BotAction.Cooperate ? 0 : 1 };
            _ = ScriptLoader.ExecuteMethod($"{nameof(SetResult)}", parameters);
        }
    }
}
