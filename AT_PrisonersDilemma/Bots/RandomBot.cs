
namespace AT_PrisonersDilemma.Bots
{
    public class RandomBot : IBot
    {
        public string Name => "Random";

        public BotAction NextIteration()
        {
            return Random.Shared.Next(1, 10) <= 5 ? BotAction.Cooperate : BotAction.Defect;
        }

        public void Reset() { }
        public void SetResult(BotAction player, BotAction opponent) { }
    }
}