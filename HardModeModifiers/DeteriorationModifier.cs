using BepInEx.Configuration;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class DeteriorationModifier : HardModeModifier
    {
        public new string name = "Deterioration";
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "damagePotionOnMove", true, "Makes the potion take damage when it moves.").Value);

            if (this.active)
            {
                PotionHealth.OnUpdateHealth += (sender, e) =>
                {
                    float rate = 0.01f;
                    Transform indicatorContainer = Managers.RecipeMap.recipeMapObject.indicatorContainer;
                    Vector2 vector = indicatorContainer.localPosition;
                    Vector2 v = Managers.RecipeMap.indicator.targetPosition - vector;
                    if (v.magnitude != 0)
                    {
                        Debug.Log("Ran thing successfully.");
                        e.Health -= (v.magnitude*rate);
                    }
                };
            }
		}
    }
    [HarmonyPatch(typeof(IndicatorMapItem), "UpdateHealth")]
    public class UpdateHealthOverride
    {
        public static void Prefix(IndicatorMapItem __instance)
        {
            var detModInstance = new DeteriorationModifier();
            if (detModInstance.active)
            {
                var x = Reflection.GetPrivateField<float>(__instance, "health");
                var y = Reflection.GetPrivateField<float>(__instance, "visualHealth");
                if (Managers.RecipeMap.indicator.afterLoadFixedUpdatesCount < 2)
                    return;
                RecipeMapManagerIndicatorSettings indicatorSettings = Managers.RecipeMap.indicatorSettings;
                double visualHealth1 = (double) y;
                if (Managers.RecipeMap.indicator.dangerZoneIds.Count > 0)
                {
                    Transform indicatorContainer = Managers.RecipeMap.recipeMapObject.indicatorContainer;
                    RecipeMapManagerTeleportationSettings teleportationSettings = Managers.RecipeMap.teleportationSettings;
                    float magnitude = (indicatorContainer.localPosition - Managers.RecipeMap.indicator.previousPosition).magnitude;
                    x = Mathf.Clamp01(x + indicatorSettings.indicatorHealthDecreasingCoefficient * magnitude + (float) -(Managers.RecipeMap.indicator.teleportationAnimator.isAnimating ? (double) Managers.RecipeMap.indicator.teleportationAnimator.AnimationStatus - (double) Managers.RecipeMap.indicator.previousTeleportationStatus : 0.0) * (Managers.RecipeMap.indicator.teleportationAnimator.isFadingOut ? teleportationSettings.totalIndicatorDamageOnFadeOut : teleportationSettings.totalIndicatorDamageOnFadeIn));
                    y = x;
                }
                else
                {
                    y = Mathf.Clamp01(y + Time.deltaTime / indicatorSettings.indicatorVisualHealthRegenerationTime);
                }
                if (x == 0.0f)
                    Reflection.CallPrivateMethod(__instance, "OnIndicatorRuined");
                double visualHealth2 = (double) y;
                if (((float) visualHealth1) == ((float) visualHealth2))
                    return;
                Managers.RecipeMap.indicator.liquidLevel.OnVisualHealthChanged();
            }
        }        
    }
}