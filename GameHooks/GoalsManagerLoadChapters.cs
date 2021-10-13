using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlayerManager;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(GoalsManager), "LoadChapters")]
    public static class GoalsManagerLoadChaptersEvent
    {
        public static EventHandler<GoalsManagerLoadChaptersEventArgs> onGoalsManagerLoadChaptersEvent;

        public static void Postfix()
        {
            var e = new GoalsManagerLoadChaptersEventArgs();
            onGoalsManagerLoadChaptersEvent?.Invoke(null, e);
            
        }
    }

    public class GoalsManagerLoadChaptersEventArgs : EventArgs
    {

    }
}
