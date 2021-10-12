using BepInEx.Configuration;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using Utils.Extensions;

namespace catgocrihxpmods.HardMode.PotionCraft
{


    public class HealthAffectsPotionTier : HardModeModifier
    {
        public new string name = "Potion Tier";
        public float rate = 0.3f;
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "active", true, "Potion health affects tier").Value);
            rate = config.Bind(name + " Settings", "rate", 0.3f, "The amount health contributes to tier").Value;

            if (this.active)
            {

                GetPotionEffectTier.OnGetPotionEffectTier += (sender, e) =>
                {
                    e.handled = true;

                    Transform indicatorContainer = Managers.RecipeMap.recipeMapObject.indicatorContainer;
                    Transform transform = e.RMM.currentPotionEffectMapItem.transform;
                    float time = Vector2.Distance(indicatorContainer.localPosition, transform.localPosition);
                    float time2 = Mathf.Abs(Mathf.DeltaAngle(Managers.RecipeMap.indicatorRotation.Value, transform.eulerAngles.z));
                    float num = Managers.RecipeMap.potionEffectsSettings.effectPowerDistanceDependence.Evaluate(time);
                    float num2 = Managers.RecipeMap.potionEffectsSettings.effectPowerAngleDistanceDependence.Evaluate(time2);

                    float num4 = (PotionHealth.health - 0.95f)*rate;

                    float num3 = Mathf.Clamp01(num + num2 + num4);
                    if (!(num3 < Managers.RecipeMap.potionEffectsSettings.middleEffectPowerPosition))
                    {
                        if (!num3.Is(1f))
                        {
                            e.potionTier = 2;
                            return;
                        }
                        e.potionTier = 3;
                        return;
                    }
                    e.potionTier = 1;
                    return;
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