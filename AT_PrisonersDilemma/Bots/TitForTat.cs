﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT_PrisonersDilemma.Bots
{
    public class TitForTat : IBot
    {
        private BotAction previousOpponentAction = BotAction.Cooperate;
        public string Name => "Tit for Tat";

        public BotAction NextIteration() => previousOpponentAction;

        public void Reset() { previousOpponentAction = BotAction.Cooperate; }
        public void SetResult(BotAction player, BotAction opponent) { previousOpponentAction = opponent; }
    }
}
