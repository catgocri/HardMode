
using System;
using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(RecipeMapManager), "GetEffectTier")]
    public static class GetPotionEffectTier
    {
        public static EventHandler<GetPotionEffectTierArgs> OnGetPotionEffectTier;

        static bool Prefix(ref int __result, RecipeMapManager __instance)
        {
            var e = new GetPotionEffectTierArgs(__instance);
            OnGetPotionEffectTier?.Invoke(null, e);
            __result = e.potionTier;
            return !e.handled;
        }
    }

    public class GetPotionEffectTierArgs : EventArgs
    {
        public int potionTier;
        public RecipeMapManager RMM;
        public bool handled = false;

        public GetPotionEffectTierArgs(RecipeMapManager rMM)
        {
            RMM = rMM;
        }
    }
}
