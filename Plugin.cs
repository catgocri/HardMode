using BasicMod;
using BasicMod.Factories;
using BepInEx;
using BepInEx.Configuration;
using Books.GoalsBook;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using catgocrihxpmods.HardMode.PotionCraft.Talents;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    [BepInPlugin("net.catgocrihxpmods.PotionCraft.HardMode", "Potion Craft Hard Mode", "1.0")]
	[BepInDependency("potioncraft.basicmod")]

    public class HardModePlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> useConfig;
        public static ConfigEntry<bool> infHaggle;
        //public static ConfigEntry<bool> bonusGoals;
        public static ConfigEntry<bool> showWatermark;

        public static Vector3 logoPos = new Vector3(4.5f, -0.7f, 0.0f);

        void Awake()
        {
            Debug.Log(@"[HardMode]: Loaded");
            var harmony = new Harmony("net.catgocrihxpmods.PotionCraft.HardMode");
            harmony.PatchAll();

            //Don't actually turn this off. Will likely break the game c:
            useConfig = Config.Bind("Hardmode main settings", "useConfig", true, "Whether to use the rest of this config. Turning this off will cause Hardmode to do nothing unless used by other mods.");
            showWatermark = Config.Bind("Hardmode main settings", "showWatermark", true, "Shows the HardMode watermark");
            //bonusGoals = Config.Bind("Hardmode main settings", "bonusGoals", true, "Gives bonus goals that also increase your experience modifier. BREAKS SAVES THAT BONUS GOALS HAVE BEEN USED IN WHEN TURNED OFF.");
            infHaggle = Config.Bind("Hardmode main settings", "infiniteHaggle", true, "Adds a haggle talent at the end of the tree that can be gained infinitely");

            if (useConfig.Value)
            {
                LoadFromConfig(Config);
            }

            BasicMod.Factories.TalentFactory.onPreRegisterTalentsEvent += (_, e) =>
            {
                if (infHaggle.Value)
                {
                    LocalDict.AddKeyToDictionary("talent_inf_haggle_talent", "Infinite Haggling");
                    LocalDict.AddKeyToDictionary("talent_description_inf_haggle_talent", "Each level in this talent raises your total haggle reward by 5%");


                    TalentHaggleInfinite th = ScriptableObject.CreateInstance<TalentHaggleInfinite>();
                    th.name = "inf_haggle_talent";
                    th.parentTalent = TalentFactory.lastHaggleTalent;
                    th.cost = 4;
                    TalentFactory.lastHaggleTalent.nextTalent = th;

                    TalentFactory.AddTalent(th);

                }
            };


            if (showWatermark.Value)  AddSprite();
            PotionHealth.LoadFromBindings(Config);
           
        }

        public static void AddSprite()
        {
            var spriteHolder = new GameObject();
            spriteHolder.name = "HardModeSpriteHolder";
            spriteHolder.transform.Translate(logoPos);
            spriteHolder.layer = 5;

            var sprite = spriteHolder.AddComponent<SpriteRenderer>();
            sprite.sprite = SpriteLoader.LoadSpriteFromFile("hardmodelogosmall.png");
            sprite.sortingLayerID = 1812034761;


        }

        public static Chapter chapter;



        void LoadFromConfig(ConfigFile config)
        {
            PotionHealth.boneDamage = -Config.Bind("Hardmode main settings", "boneDamage", 0.8f, "The amount of damage you take while moving a full unit touching bones. Vanilla default is 0.4.").Value;

            HardModeModifier tutorialModifier = new NoStartTutorialModifier();
            NoStartTutorialModifier.instance.LoadFromBindings(config);

            HardModeModifier highlanderModifier = new HighlanderModifier();
            HighlanderModifier.instance.LoadFromBindings(config);

            HardModeModifier priceModifier = new PriceModifier();
            PriceModifier.instance.LoadFromBindings(config);

            HardModeModifier taxModifier = new TaxModifier();
            TaxModifier.instance.LoadFromBindings(config);

            HardModeModifier deteriorationModifer = new DeteriorationModifier();
            DeteriorationModifier.instance.LoadFromBindings(config);

            HardModeModifier waterhealsModifer = new PouringWaterHealsModifier();
            PouringWaterHealsModifier.instance.LoadFromBindings(config);

            HardModeModifier healthaffectstierModifer = new HealthAffectsPotionTier();
            HealthAffectsPotionTier.instance.LoadFromBindings(config);

            HardModeModifier gardenModifier = new GardenModifier();
            GardenModifier.instance.LoadFromBindings(config);

            HardModeModifier expModifier = new ExperienceModifier();
            ExperienceModifier.instance.LoadFromBindings(config);
        }

       
    }
}
