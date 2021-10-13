using BepInEx.Configuration;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using Utils.Extensions;

namespace catgocrihxpmods.HardMode.PotionCraft
{

    //Should be loaded after DangerZoneModifier ( I think)

    public class DeteriorationModifier : HardModeModifier
    {
        public new string name = "Deterioration";
        public float rate = 0.01f;
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "damagePotionOnMove", true, "Makes the potion take damage when it moves.").Value);
            rate = config.Bind(name + " Settings", "damageRate", 0.006f, "The rate potion takes to deteriorate.").Value;
            if (this.active)
            {

                MoveIndicatorAlongPathEvent.OnMoveIndicatorAlongPath += (sender, e) =>
                {

                    Transform indicatorContainer = Managers.RecipeMap.recipeMapObject.indicatorContainer;
                    Vector2 vector = indicatorContainer.localPosition;
                    Vector2 v = e.TargetPosition - vector;
                    if (v.magnitude != 0)
                    {
                        PotionHealth.SetHealth(PotionHealth.health - (v.magnitude * rate));
                    }


                };
            }

        }
    }

    /*
    [HarmonyPatch(typeof(IndicatorMapItem), "UpdateHealth")]
    public class UpdateHealthOverride
    {
        public static bool Prefix(IndicatorMapItem __instance)
        {
            //This creates a new DeteriorationModifier each frame :(
            var detModInstance = new DeteriorationModifier();

            //We cab access the currently loaded one with this c:
            var detMod = DeteriorationModifier.instance;

            if (detModInstance.active)
            {
                var x = Reflection.GetPrivateField<float>(__instance, "health");
                var y = Reflection.GetPrivateField<float>(__instance, "visualHealth");
                if (Managers.RecipeMap.indicator.afterLoadFixedUpdatesCount < 2)

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
                if (((float)visualHealth1) == ((float)visualHealth2))
                {
                    Managers.RecipeMap.indicator.liquidLevel.OnVisualHealthChanged();
                }

                //Make sure we don't let the other code run
                return false;

            }

            //we still want vanilla behaviour here
            return true;
        }        
    }*/
}