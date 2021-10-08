using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    [HarmonyPatch(typeof(TradeManager), "GetDiscountForItem")]
    //DONE: Descriptive Patch Name
    public static class PriceModifierPatch
    {
        static bool Prefix(ref float __result)
        {
            __result = 2f;
            return false;
        }
    }
}
