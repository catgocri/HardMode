
using System;
using BasicMod.Utility;
using HarmonyLib;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(ObjectBased.Garden.GrowingSpot.SpotPlant), "GatherIngredient")]
    public static class GardenEvent
    {
        public static EventHandler<GardenEventEventArgs> onGardenEvent;

        static bool Prefix(GameObject __instance)
        {
            var e = new GardenEventEventArgs();
            onGardenEvent?.Invoke(null, e);
            if (e.ingredientAmount.HasValue)
            {
                Reflection.SetPrivateField(__instance, "ingredientAmount", e.ingredientAmount);
            }
            return true;
        }
    }

    public class GardenEventEventArgs : EventArgs
    {
        public Vector2Int? ingredientAmount { get; set; }
    }
}
