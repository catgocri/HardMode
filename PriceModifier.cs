using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    [HarmonyPatch(typeof(TradeManager), "GetDiscountForItem")]
    //TODO: Update with descriptive patch name
    public static class MyPatch3 
    {
        static bool Prefix(ref float __result)
        {
            __result = 2f;
            return false;
        }
    }
}
