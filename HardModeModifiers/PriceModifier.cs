using BepInEx.Configuration;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    //DONE: Descriptive Patch Name
    public class PriceModifier : HardModeModifier
    {
        public new string name = "Markup Prices";
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "modifyPrices", true, "Makes merchants always sell at markup.").Value);
            if (this.active)
            {
                ItemDiscount.OnCalculateItemDiscount += (_, e) => e.CostMultiplier = 2;
            }
        }
    }
}
