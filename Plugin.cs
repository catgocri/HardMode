using BepInEx;
using BepInEx.Configuration;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using HarmonyLib;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    [BepInPlugin("net.catgocrihxpmods.PotionCraft.HardMode", "Potion Craft Hard Mode", "1.0")]
    public class HardModePlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> useConfig;

        void Awake()
        {
            Debug.Log(@"[HardMode]: Loaded");
            var harmony = new Harmony("net.catgocrihxpmods.PotionCraft.HardMode");
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
            PotionHealth.boneDamage = -Config.Bind("Hardmode main settings", "boneDamage", 0.8f, "The amount of damage you take while moving a full unit touching bones. Vanilla default is 0.4.").Value;

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

            /*HardModeModifier dangerZoneModifier = new DangerZoneModifier();
            DangerZoneModifier.instance = dangerZoneModifier;
            DangerZoneModifier.instance.LoadFromBindings(config);*/

            HardModeModifier deteriorationModifer = new DeteriorationModifier();
            DeteriorationModifier.instance = deteriorationModifer;
            DeteriorationModifier.instance.LoadFromBindings(config);

            HardModeModifier waterhealsModifer = new PouringWaterHealsModifier();
            PouringWaterHealsModifier.instance = waterhealsModifer;
            PouringWaterHealsModifier.instance.LoadFromBindings(config);

            HardModeModifier healthaffectstierModifer = new HealthAffectsPotionTier();
            HealthAffectsPotionTier.instance = healthaffectstierModifer;
            HealthAffectsPotionTier.instance.LoadFromBindings(config);

            HardModeModifier gardenModifier = new GardenModifier();
            GardenModifier.instance = gardenModifier;
            GardenModifier.instance.LoadFromBindings(config);
        }
    }
}
