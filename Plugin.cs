using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    [BepInPlugin("net.catgocrihxpmods.PotionCraft.HardMode", "Potion Craft Hard Mode", "1.0")]
    public class TemplatePlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> useConfig;

        void Awake()
        {
            Debug.Log(@"[HardMode]: Loaded");
            var harmony = new Harmony("net.catgocrihxpmods.PotionCraft.Template");
            harmony.PatchAll();

            //Don't actually turn this off. Will likely break the game c:
            useConfig = Config.Bind("Hardmode main settings", "useConfig", true, "Whether to use the rest of this config. Turning this off will cause Hardmode to do nothing unless used by other mods.");

            if (useConfig.Value)
            {
                LoadFromConfig(Config);
            }
        }

        void LoadFromConfig(ConfigFile config)
        {
            HardModeModifier tutorialModifier = new NoStartTutorialModifier();
            NoStartTutorialModifier.instance = tutorialModifier;
            NoStartTutorialModifier.instance.LoadFromBindings(config);

            HardModeModifier highlanderModifier = new HighlanderModifier();
            HighlanderModifier.instance = highlanderModifier;
            HighlanderModifier.instance.LoadFromBindings(config);

            HardModeModifier priceModifier = new PriceModifier();
            PriceModifier.instance = priceModifier;
            PriceModifier.instance.LoadFromBindings(config);

            HardModeModifier taxModifier = new TaxModifier();
            TaxModifier.instance = taxModifier;
            TaxModifier.instance.LoadFromBindings(config);
        }
    }
}
