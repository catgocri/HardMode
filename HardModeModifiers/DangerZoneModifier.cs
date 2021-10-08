using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;

namespace catgocrihxpmods.HardMode.PotionCraft
{
   public class DangerZoneModifier : HardModeModifier
    {
        public new string name = "Bone Zone";
        public override void LoadFromBindings(ConfigFile config)
        {
			SetActive(config.Bind(name + " Settings", "dangerZoneInstaKill", true, "If bone zones should instantly kill you.").Value);
        }
    }
    [HarmonyPatch(typeof(IndicatorMapItem), "UpdateHealth")]
    public static class DangerZoneModifierPatch
    {
        static bool Prefix(IndicatorMapItem __instance)
        {
            if(!DangerZoneModifier.instance.active)
            {
				return true;
            }
            if (__instance.dangerZoneIds.Count > 0)
            {
                var indicator = Managers.RecipeMap.indicator;
                indicator.GetType().GetTypeInfo().GetDeclaredMethod("OnIndicatorRuined").Invoke(indicator, null);
                return false;
            }
            return false;
        }
    } 
}