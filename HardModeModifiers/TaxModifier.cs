using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class TaxModifier : HardModeModifier
    {
        public new string name = "Tax";
        public static float taxPercent;
        public static int taxThreshold;
        public static bool deathAndTaxes; //This config will end the game if a player would pay taxes but can't afford them.

        //I think we could add an escalation mode that increases the taxPercentage over time.

        public override void LoadFromBindings(ConfigFile config)
        {
			SetActive(config.Bind(name + " Settings", "doTaxes", true, "If the game should subtract money from you each day.").Value);
            taxPercent = config.Bind(name + " Settings", "taxPercentage", 0.05f, "How much, in percentage, you lose in taxes each day").Value;
            taxThreshold = config.Bind(name + " Settings", "taxThreshold", 500, "The minimum amount of Gold you must have before being charged taxes.").Value;

        }
    }
    [HarmonyPatch(typeof(DayManager), "StartNewDay")]
    public static class TaxModifierPatch
    {
        static void Postfix()
        {
            if(!TaxModifier.instance.active)
            {
                return;
            }
            if(Managers.Player.Gold >= TaxModifier.taxThreshold)
            {
                var value = Mathf.RoundToInt(-TaxModifier.taxPercent * Managers.Player.Gold);
                Debug.Log("Lost " + value + " gold.");

                Managers.Player.AddGold(value);
            }
        }
    }
}
