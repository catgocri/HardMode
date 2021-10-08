using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Potion;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    class HighlanderModifier : HardModeModifier
    {
        public int maxIngredientAmount;
        public new string name = "Highlander Mode";

        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "active", true, "If active, potions will fail when adding more than the amount detailed below of a single ingredient").Value);
            maxIngredientAmount = config.Bind(name + " Settings", "maxDuplicateIngredients", 1, "1 = True Highlander.").Value;
        }



        public void onIngredientAdded(Ingredient ingredient)
        {
            IEnumerable<UsedComponent> component = (from comp in Managers.Potion.usedComponents
                                                    where comp.componentType == UsedComponent.ComponentType.InventoryItem && (comp.componentObject as InventoryItem).IsSame(ingredient)
                                                    select comp);
            //We should only get one here
            foreach (UsedComponent comp in component)
            {
                 //Where the magic happens
                if(comp.amount > maxIngredientAmount)
                {
                    //Debug.Log("There can only be one!");
                    var indicator = Managers.RecipeMap.indicator;
                    indicator.GetType().GetTypeInfo().GetDeclaredMethod("OnIndicatorRuined").Invoke(indicator, null);
                }
            }
           

        }


    }


    /*
    [HarmonyPatch(typeof(PotionCraftPanel.PotionCraftPanel))]
    [HarmonyPatch("Awake")]
    class OnPotionUpdatePatch
    {
        static void Postfix()
        {
            var mod = HighlanderModifier.instance as HighlanderModifier;
            Managers.Potion.potionCraftPanel.onPotionUpdated.AddListener(mod.onPotionUpdated);
        }
    }
    */

    [HarmonyPatch(typeof(ObjectBased.Stack.Stack))]
    [HarmonyPatch("AddIngredientPathToMapPath")]
    class OnIngredientsAddPatch
    {
        static void Postfix(ObjectBased.Stack.Stack __instance)
        {
            if (!HighlanderModifier.instance.active)
            {
                return;
            }

            Ingredient ingredient = (Ingredient)__instance.inventoryItem;
            var mod = HighlanderModifier.instance as HighlanderModifier;

            mod.onIngredientAdded(ingredient);
        }
    }
}
