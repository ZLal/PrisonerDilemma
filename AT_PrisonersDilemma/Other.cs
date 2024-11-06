
namespace AT_PrisonersDilemma
{
    public class StateItem
    {
        public IBot Player1 { get; set; }
        public IBot Player2 { get; set; }

        public List<KeyValuePair<BotAction, BotAction>> Results { get; set; }
    }

    public class ResultData
    {
        public List<KeyValuePair<string, int>> LeaderBoard { get; set; }
        public List<ResultItem> Results { get; set; }
    }

    public class ResultItem
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }

        public List<Tuple<BotAction, BotAction, int, int>> ScoreList { get; set; }
    }

    public enum BotAction
    {
        Cooperate,
        Defect
    }

    public interface IBot
    {
        public string Name { get; }

        BotAction NextIteration();
        void SetResult(BotAction player, BotAction opponent);
        void Reset();
    }
}
