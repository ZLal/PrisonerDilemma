using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT_PrisonersDilemma.Bots
{
    public class RandomBot : IBot
    {
        private readonly Random random;
        public RandomBot(Random random)
        {
            this.random = random;
        }

        public string Name => "Random";

        public BotAction NextIteration()
        {
            return random.Next(1, 10) <= 5 ? BotAction.Cooperate : BotAction.Defect;
        }

        public void Reset() { }
        public void SetResult(BotAction player, BotAction opponent) { }
    }
}
