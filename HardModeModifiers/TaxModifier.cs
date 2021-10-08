using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class TaxModifier : HardModeModifier
    {
        public new string name = "Markup Prices";
        public override void LoadFromBindings(ConfigFile config)
        {
			SetActive(config.Bind(name + " Settings", "doTaxes", true, "If the game should subtract money from you each day.").Value);
        }
    }
    [HarmonyPatch(typeof(DayManager), "StartNewDay")]
    public static class TaxModifierPatch
    {
        static bool Prefix(DayManager __instance)
        {
            if(!TaxModifier.instance.active)
            {
				return true;
            }
            if(Managers.Player.Gold >= 1000)
            {
                Managers.Player.AddGold(Mathf.Max(- Managers.Player.Gold / 20));
                return false;
            }
            return false;
        }
    }
}
