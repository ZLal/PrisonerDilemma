
namespace AT_PrisonersDilemma
{
    public class Game
    {
        public int RemainingIteration { get; private set; } = 0;
        /// <summary>
        /// If greater than 0 it will be used as iteration count
        /// </summary>
        public int StartIteration { get; set; } = 10;

        /// <summary>
        /// Allow bot to play with itself
        /// </summary>
        public bool AllowBotToPlayItself { get; set; } = true;

        private Dictionary<string, IBot> Bots = new();

        private List<StateItem> GameStates = new();

        public void AddBot(IBot bot) => Bots.Add(bot.Name, bot);

        private List<StateItem> CreateState()
        {
            int i, j;
            List<IBot> botList = Bots.Values.ToList();
            List<StateItem> stateItems = new();
            for (i = 0; i < botList.Count; i++)
            {
                j = AllowBotToPlayItself ? i : i + 1;
                for (; j < botList.Count; j++)
                {
                    stateItems.Add(new StateItem()
                    {
                        Player1 = botList[i],
                        Player2 = botList[j],
                        Results = new List<KeyValuePair<BotAction, BotAction>>()
                    });
                }
            }
            return stateItems;
        }

        private void PlayStateGame(StateItem item)
        {
            for (int i = 0; i < RemainingIteration; i++)
            {
                var kv = new KeyValuePair<BotAction, BotAction>(item.Player1.NextIteration(), item.Player2.NextIteration());
                item.Results.Add(kv);
                item.Player1.SetResult(kv.Key, kv.Value);
                item.Player2.SetResult(kv.Value, kv.Key);
            }
            item.Player1.Reset();
            item.Player2.Reset();
        }

        public void PlayGame()
        {
            Reset();
            GameStates = CreateState();
            RemainingIteration = StartIteration;
            foreach (var state in GameStates)
            {
                PlayStateGame(state);
            }
        }

        private static Tuple<BotAction, BotAction, int, int> CalculateScore(KeyValuePair<BotAction, BotAction> keyValue)
        {
            var score = keyValue switch
            {
                { Key: BotAction.Cooperate, Value: BotAction.Cooperate } => new KeyValuePair<int, int>(3, 3),
                { Key: BotAction.Defect, Value: BotAction.Defect } => new KeyValuePair<int, int>(1, 1),
                { Key: BotAction.Cooperate, Value: BotAction.Defect } => new KeyValuePair<int, int>(0, 5),
                { Key: BotAction.Defect, Value: BotAction.Cooperate } => new KeyValuePair<int, int>(5, 0),
                _ => throw new Exception("Invalid Action")
            };
            return new Tuple<BotAction, BotAction, int, int>(keyValue.Key, keyValue.Value, score.Key, score.Value);
        }

        public ResultData GetResults()
        {
            ResultData result;
            List<ResultItem> resultItems;
            resultItems = GameStates.Select(x => new ResultItem()
            {
                Player1 = x.Player1.Name,
                Player2 = x.Player2.Name,
                ScoreList = x.Results.Select(y => CalculateScore(y)).ToList()
            }).ToList();

            List<KeyValuePair<string, int>> leaderBoard = new();
            leaderBoard.AddRange(resultItems.Select(x => new KeyValuePair<string, int>(x.Player1, x.ScoreList.Sum(y => y.Item3))));
            leaderBoard.AddRange(resultItems.Select(x => new KeyValuePair<string, int>(x.Player2, x.ScoreList.Sum(y => y.Item4))));
            leaderBoard = leaderBoard.GroupBy(x => x.Key)
                .Select(x => new KeyValuePair<string, int>(x.Key, x.Sum(y => y.Value)))
                .OrderByDescending(x => x.Value)
                .ToList();
            
            result = new ResultData()
            {
                LeaderBoard = leaderBoard,
                Results = resultItems
            };
            return result;
        }

        public void Reset()
        {
            RemainingIteration = 0;
            GameStates.Clear();
        }
    }
}
