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
                    //Sorry! It kept crashing my client while I was working on it :(
                };
            }
        }
    }
}