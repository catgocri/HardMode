
using System;
using HarmonyLib;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(IndicatorMapItem), "UpdateHealth")]
    public static class PotionHealth
    {
        public static EventHandler<UpdateHealthEventArgs> OnUpdateHealth;

        static bool Prefix(IndicatorMapItem __instance)
        {
            var e = new UpdateHealthEventArgs();
            e.Health = Reflection.GetPrivateField<float>(__instance, "health");
            e.VisualHealth = Reflection.GetPrivateField<float>(__instance, "visualHealth");
            
            OnUpdateHealth?.Invoke(null, e);

            Reflection.SetPrivateField<float>(__instance, "health", (float)e.Health );
            Reflection.SetPrivateField<float>(__instance, "visualHealth", (float)e.VisualHealth);

            if (e.Health <= 0)
            {
                KillPotion();
            }

            return !(bool)e.Handled;
        }

        public static void KillPotion()
        {
            Reflection.CallPrivateMethod(Managers.RecipeMap.indicator, "OnIndicatorRuined");
        }
    }

    public class UpdateHealthEventArgs : EventArgs
    {
        public float? Health { get; set; }
        public float? VisualHealth { get; set; }
        public bool? Handled { get; set; } = false;
    }
}
