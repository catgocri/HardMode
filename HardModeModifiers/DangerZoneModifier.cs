using BepInEx.Configuration;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class DangerZoneModifier : HardModeModifier
    {
        public new string name = "Bone Zone";
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "dangerZoneInstaKill", true, "If bone zones should instantly kill you.").Value);

            if (this.active)
            {
                PotionHealth.OnUpdateHealth += (sender, e) =>
                {
                    if (Managers.RecipeMap.indicator.dangerZoneIds.Count > 0)
                    {
                        // PotionHealth will apply the potion failure effect for us.
                        e.Health = 0;
                    }
                };
            }
        }
    }
}