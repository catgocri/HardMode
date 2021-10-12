using BepInEx.Configuration;
using System.Linq;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using static Potion;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    class HighlanderModifier : HardModeModifier
    {
        public int maxIngredientAmount;
        public new string name = "Highlander Mode";

        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "highlanderActive", true, "If active, potions will fail when adding more than the amount detailed below of a single ingredient").Value);
            maxIngredientAmount = config.Bind(name + " Settings", "maxDuplicateIngredients", 1, "1 = True Highlander.").Value;

            if (this.active)
            {
                IngredientAddedEvent.OnIngredientAdded += OnIngredientAdded;
            }
        }

        private void OnIngredientAdded(object sender, IngredientAddedEventArgs e)
        {
            var ingredient = e.Ingredient;

            var components = from comp in Managers.Potion.usedComponents
                             where comp.componentType == UsedComponent.ComponentType.InventoryItem
                             let componentItem = comp.componentObject as InventoryItem
                             where componentItem?.IsSame(ingredient) == true
                             select comp;

            if (components.Any(x => x.amount > maxIngredientAmount))
            {
                PotionHealth.KillPotion();
            }
        }
    }
}
