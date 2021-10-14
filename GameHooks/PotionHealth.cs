
using System;
using System.Reflection;
using HarmonyLib;
using ObjectBased.RecipeMap.RecipeMapItem.IndicatorMapItem;
using TMPro;
using UnityEngine;
using Utils.Extensions;
using static ProgressState;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{



    public static class PotionHealth
    {
        public static EventHandler<UpdateHealthEventArgs> OnUpdateHealth;
        public static float health = 1.0f;
        public static float visualHealth = 1.0f;
		public static float boneDamage = 0.4f;//Vanilla
		public static TextMeshPro TMPHealth;


		public static void Start()
        {
			AddHealthText();
			OnUpdateHealth += (sender, e) =>
			{
				TMPHealth.text = "HardMode\nHealth: " + Mathf.FloorToInt(health * 100f).ToString();
			};
        }

		public static void AddHealthText()
		{
			var textHolder = new GameObject();
			textHolder.name = "PotionHealthTextHolder";
			textHolder.transform.Translate(HardModePlugin.logoPos+ new Vector3(0,-1f));
			textHolder.layer = 5;

			TMPHealth = textHolder.AddComponent<TextMeshPro>();
			TMPHealth.alignment = TextAlignmentOptions.Center;
			TMPHealth.enableAutoSizing = true;
			TMPHealth.sortingLayerID = -1650695527;
			TMPHealth.sortingOrder = 100;
			TMPHealth.fontSize = 3;
			TMPHealth.fontSizeMin = 3;
			TMPHealth.fontSizeMax = 3;
			TMPHealth.color = Color.black;
			TMPHealth.text = "Health";

			GameObject panel = GameObject.Find("Room Lab/RecipeMap In Room/UI");
			if (panel is not null)
			{
				textHolder.transform.SetParent(panel.transform);
			}
		}


		public static void SetHealth(float newHealth)
		{


			float value = visualHealth;
			float value2 = visualHealth + (newHealth - health);
			health = Mathf.Clamp01(newHealth);
			visualHealth = Mathf.Clamp01(value2);
			if (!value.Is(visualHealth))
			{
				Managers.RecipeMap.indicator.liquidLevel.OnVisualHealthChanged();
			}
		}

		public static void KillPotion()
        {
            BasicMod.Utility.Reflection.InvokePrivateMethod(Managers.RecipeMap.indicator, "OnIndicatorRuined");
        }
    }

	public class UpdateHealthEventArgs : EventArgs
	{
		public bool? Handled { get; set; } = false;
		public IndicatorMapItem indicator;
	}

	//Health system overhaul begin

	[HarmonyPatch(typeof(IndicatorMapItem))]
	[HarmonyPatch("UpdateHealth")]
	class RemoveVanillaUpdateHealth
	{
		static bool Prefix(IndicatorMapItem __instance)
		{

			var e = new UpdateHealthEventArgs();
			e.indicator = __instance;
			PotionHealth.OnUpdateHealth?.Invoke(null, e);


			//Do danger zone stuff


			//End danger zone stuff
			if (__instance.afterLoadFixedUpdatesCount >= 2)
			{
				RecipeMapManagerIndicatorSettings indicatorSettings = Managers.RecipeMap.indicatorSettings;
				float value = PotionHealth.visualHealth;
				if (__instance.dangerZoneIds.Count > 0)
				{
					Transform indicatorContainer = Managers.RecipeMap.recipeMapObject.indicatorContainer;
					RecipeMapManagerTeleportationSettings teleportationSettings = Managers.RecipeMap.teleportationSettings;
					float magnitude = (indicatorContainer.localPosition - __instance.previousPosition).magnitude;
					float num = PotionHealth.boneDamage * magnitude;
					float num2 = (__instance.teleportationAnimator.isAnimating ? (__instance.teleportationAnimator.AnimationStatus - __instance.previousTeleportationStatus) : 0f);
					float num3 = (__instance.teleportationAnimator.isFadingOut ? teleportationSettings.totalIndicatorDamageOnFadeOut : teleportationSettings.totalIndicatorDamageOnFadeIn);
					float num4 = (0f - num2) * num3;
					PotionHealth.SetHealth(Mathf.Clamp01(PotionHealth.health + num + num4));
					//visualHealth = health;
				}
			}



				//Kill potion stuff
				if (PotionHealth.health.Is(0f))
			{
				PotionHealth.KillPotion();
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(IndicatorMapItem))]
	[HarmonyPatch("Update")]
	class HardCoreModePatch
	{
		static bool Prefix(IndicatorMapItem __instance)
		{
			return true;
		}
	}


	[HarmonyPatch(typeof(PotionManager))]
	[HarmonyPatch("ResetPotion")]
	class ResetPotionPatch
	{
		static void Prefix()
		{
			PotionHealth.health = 1.0f;
			PotionHealth.visualHealth = 1.0f;
		}
	}

	[HarmonyPatch(typeof(IndicatorMapItem))]
	[HarmonyPatch("AddHealthBySalt")]
	class AddHealthBySaltPatch
	{
		static bool Prefix(IndicatorMapItem __instance, float healthToAdd)
		{
			float value = PotionHealth.visualHealth;
			PotionHealth.health = Mathf.Clamp01(PotionHealth.health + healthToAdd);
			PotionHealth.visualHealth = Mathf.Clamp01(PotionHealth.visualHealth + healthToAdd);
			if (!value.Is(PotionHealth.visualHealth))
			{
				__instance.liquidLevel.OnVisualHealthChanged();
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(SerializedPath))]
	[HarmonyPatch("ApplyPathToCurrentPotion")]
	class SetHealthOnApplyPathPatch
	{
		static void Prefix(SerializedPath __instance)
		{
			PotionHealth.health = __instance.health;
			PotionHealth.visualHealth = __instance.health;
			//Managers.RecipeMap.indicator.liquidLevel.OnVisualHealthChanged();
		}
	}

	[HarmonyPatch(typeof(SerializedPath))]
	[HarmonyPatch("GetPathFromCurrentPotion")]
	class SaveHealthToPathPatch
	{
		static void Postfix(ref SerializedPath __result)
		{
			if (__result != null)
			{
				__result.health = PotionHealth.health;
			}
		}
	}

	[HarmonyPatch(typeof(LiquidLevel))]
	[HarmonyPatch("OnVisualHealthChanged")]
	class VisualHealthChangedPatch
	{
		static bool Prefix()
		{

			var shaderOffsetY = 0.5f;
			float value = PotionHealth.visualHealth.Map(1f, 0f, 0f, shaderOffsetY);
			int shPropIndicatorLiquidLevelOffsetY = Shader.PropertyToID("_LiquidLevelOffsetY");

			Shader.SetGlobalFloat(shPropIndicatorLiquidLevelOffsetY, value);
			return false;
		}
	}

}
