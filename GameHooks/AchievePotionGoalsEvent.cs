using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlayerManager;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(GoalsManager), "AchievePotionGoals")]
    public static class AchievePotionGoalsEvent
    {
        public static EventHandler<AchievePotionGoalsEventArgs> onAchievePotionGoalsEvent;

        public static void Prefix(Potion potion)
        {
            var e = new AchievePotionGoalsEventArgs(potion);
            onAchievePotionGoalsEvent?.Invoke(null, e);
            
        }
    }

    public class AchievePotionGoalsEventArgs : EventArgs
    {
        public Potion potion;

        public AchievePotionGoalsEventArgs(Potion _potion)
        {
            this.potion = _potion;
        }
    }
}
