using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void onPotionUpdated()
        {
            foreach (UsedComponent c in Managers.Potion.usedComponents)
            {
                Debug.Log(c.componentObject);
                Debug.Log(c.componentType);
            }
        }

    }



    [HarmonyPatch(typeof(PotionCraftPanel.PotionCraftPanel))]
    [HarmonyPatch("Awake")]
    class HighlanderPatch
    {
        static void Postfix()
        {
            var mod = HighlanderModifier.instance as HighlanderModifier;
            Managers.Potion.potionCraftPanel.onPotionUpdated.AddListener(mod.onPotionUpdated);
        }
    }
}
