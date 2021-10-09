using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class GardenModifier : HardModeModifier
    {
        public new string name = "Garden";
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "doGardenModifier", true, "If the garden's harvest count should be modified.").Value);

            if (this.active)
            {
                GardenEvent.onGardenEvent += (sender, e) =>
                {
                    // Sets to (1, 0)
                    e.ingredientAmount = Vector2Int.right;
                };
            }
        }
    }
}
