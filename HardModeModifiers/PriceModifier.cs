using HarmonyLib;
using BepInEx.Configuration;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    //DONE: Descriptive Patch Name
    public class PriceModifier : HardModeModifier
    {
        public new string name = "Markup Prices";
        public override void LoadFromBindings(ConfigFile config)
        {
			SetActive(config.Bind(name + " Settings", "modifyPrices", true, "Makes merchants always sell at markup.").Value);
        }
    }
    [HarmonyPatch(typeof(TradeManager), "GetDiscountForItem")]
    public static class PriceModifierPatch
    {
        static bool Prefix(ref float __result)
        {
            if(!PriceModifier.instance.active)
            {
				return true;
            }
            __result = 2f;
            return false;
        }
    }
}
