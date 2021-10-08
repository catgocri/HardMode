
using System;
using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(ObjectBased.Stack.Stack), "AddIngredientPathToMapPath")]
    public static class IngredientAddedEvent
    {
        public static EventHandler<IngredientAddedEventArgs> OnIngredientAdded;

        static void Postfix(ObjectBased.Stack.Stack __instance)
        {
            var ingredient = (Ingredient)__instance.inventoryItem;
            OnIngredientAdded?.Invoke(null, new IngredientAddedEventArgs(ingredient));
        }
    }

    public class IngredientAddedEventArgs : EventArgs
    {
        public IngredientAddedEventArgs(Ingredient ingredient)
        {
            this.Ingredient = ingredient;
        }

        public Ingredient Ingredient { get; }
    }
}