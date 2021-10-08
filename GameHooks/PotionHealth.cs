
using System;
using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(IndicatorMapItem), "UpdateHealth")]
    public static class PotionHealth
    {
        public static EventHandler<UpdateHealthEventArgs> OnUpdateHealth;

        static bool Postfix(IndicatorMapItem __instance)
        {
            var e = new UpdateHealthEventArgs();
            OnUpdateHealth?.Invoke(null, e);
            if (e.Health.HasValue)
            {
                Reflection.SetPrivateField(__instance, "health", e.Health);
                if (e.Health <= 0)
                {
                    KillPotion();
                }
                return false;
            }

            return true;
        }

        public static void KillPotion()
        {
            Reflection.CallPrivateMethod(Managers.RecipeMap.indicator, "OnIndicatorRuined");
        }
    }

    public class UpdateHealthEventArgs : EventArgs
    {
        public float? Health { get; set; }
    }
}
