using System;

namespace AT_PrisonersDilemma.BotScripts
{
    public static class AlternateDouble
    {
        // Cooperate = 0 & Defect = 1

        private static int counter = 0;
        public static int NextIteration()
        {
            if (counter >= 4) counter = 0;
            counter++;
            return counter == 1 || counter == 2 ? 0 : 1;
        }

        public static void Reset() { counter = 0; }
        public static void SetResult(int player, int opponent) { }
    }
}
