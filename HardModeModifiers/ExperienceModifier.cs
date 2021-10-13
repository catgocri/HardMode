using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class ExperienceModifier : HardModeModifier
    {
        public new string name = "Experience";
        public static float modifier = 1;
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "doExperienceModifier", true, "If completing hard mode goals should add an experience multiplier.").Value);

            if (this.active)
            {
                AddExperienceEvent.onAddExperienceEvent += (sender, e) =>
                {
                    //Debug.Log(e.amount);
                    e.amount *= modifier;
                };
            }
        }

        public static void IncreaseModifier(float amount)
        {
            modifier += amount;
        }
    }
}
