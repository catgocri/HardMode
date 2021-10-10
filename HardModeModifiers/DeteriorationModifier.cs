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
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "damagePotionOnMove", true, "Makes the potion take damage when it moves.").Value);

            if (this.active)
            {
               

                PotionHealth.OnUpdateHealth += (sender, e) =>
                {
                    /// Do not gain health idly
                    e.Handled = true;

                    IndicatorMapItem indicator = Managers.RecipeMap.indicator;

                    if (indicator.afterLoadFixedUpdatesCount >= 2)
                    {

                        RecipeMapManagerIndicatorSettings indicatorSettings = Managers.RecipeMap.indicatorSettings;
                        float value = e.VisualHealth.Value;

                        if (indicator.dangerZoneIds.Count > 0)
                        {
                            Transform indicatorContainer = Managers.RecipeMap.recipeMapObject.indicatorContainer;
                            RecipeMapManagerTeleportationSettings teleportationSettings = Managers.RecipeMap.teleportationSettings;
                            float magnitude = (indicatorContainer.localPosition - indicator.previousPosition).magnitude;
                            float num = indicatorSettings.indicatorHealthDecreasingCoefficient * magnitude;
                            float num2 = (indicator.teleportationAnimator.isAnimating ? (indicator.teleportationAnimator.AnimationStatus - indicator.previousTeleportationStatus) : 0f);
                            float num3 = (indicator.teleportationAnimator.isFadingOut ? teleportationSettings.totalIndicatorDamageOnFadeOut : teleportationSettings.totalIndicatorDamageOnFadeIn);
                            float num4 = (0f - num2) * num3;
                            e.Health = Mathf.Clamp01((float)e.Health + num + num4);
                            e.VisualHealth = e.Health;
                        }
                        else
                        {
                            e.VisualHealth = Mathf.Clamp01((float)(e.VisualHealth + Time.deltaTime / indicatorSettings.indicatorVisualHealthRegenerationTime));
                        }
                        if (((float)e.Health).Is(0f))
                        {
                            Reflection.CallPrivateMethod(indicator, "OnIndicatorRuined");
                        }
                        if (!value.Is((float)e.VisualHealth))
                        {
                            indicator.liquidLevel.OnVisualHealthChanged();
                        }

                    }
                   
                };

                MoveIndicatorAlongPathEvent.OnMoveIndicatorAlongPath += (sender, e) =>
                {
                    IndicatorMapItem indicator = Managers.RecipeMap.indicator;
                    
                    float rate = 0.01f;
                    Transform indicatorContainer = Managers.RecipeMap.recipeMapObject.indicatorContainer;
                    Vector2 vector = indicatorContainer.localPosition;
                    Vector2 v = e.TargetPosition - vector;
                    if (v != Vector2.zero){
                        Debug.Log(v);
                    }
                    if (v.magnitude != 0)
                    {
                        float health = Reflection.GetPrivateField<float>(indicator, "health");
                        float value = (v.magnitude * rate);
                        Debug.Log(value);
                        Reflection.SetPrivateField<float>(indicator, "health", health - value);
                    }
                    //AINE.SetHealth(AINE.health - 0.01f);
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