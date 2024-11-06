
namespace AT_PrisonersDilemma.Bots
{
    public class AlwaysCooperate : IBot
    {
        public string Name => "Always Cooperate";

        public BotAction NextIteration() => BotAction.Cooperate;

        public void Reset() { }
        public void SetResult(BotAction player, BotAction opponent) { }
    }
}
