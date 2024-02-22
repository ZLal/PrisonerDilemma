using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT_PrisonersDilemma.Bots
{
    public class AlwaysDefect : IBot
    {
        public string Name => "Always Defect";

        public BotAction NextIteration() => BotAction.Defect;

        public void Reset() { }
        public void SetResult(BotAction player, BotAction opponent) { }
    }
}
