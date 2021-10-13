using BasicMod;
using BasicMod.Factories;
using BepInEx;
using BepInEx.Configuration;
using Books.GoalsBook;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    [BepInPlugin("net.catgocrihxpmods.PotionCraft.HardMode", "Potion Craft Hard Mode", "1.0")]
    public class HardModePlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> useConfig;
        public static List<Goal> tier3PotionGoals = new List<Goal>();

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

            AchievePotionGoalsEvent.onAchievePotionGoalsEvent += (_, e) => {
                GoalsLoader.GetGoalByName("potion10").ProgressIncrement();
                GoalsLoader.GetGoalByName("potion100").ProgressIncrement();
                GoalsLoader.GetGoalByName("potion1000").ProgressIncrement();

                int count = (int)Potion.GetArrayOfEffectTypes(e.potion.effects)[0][0];
                PotionEffect effect = (PotionEffect)Potion.GetArrayOfEffectTypes(e.potion.effects)[0][1];

                if ( count == 3)
                {
                    Goal goal = (from g in tier3PotionGoals
                               where g.descriptionParameters[0] == "#effect_" + effect.name
                               select g).FirstOrDefault<Goal>();


                    if (goal != null)
                    {
                        goal.ProgressIncrement();
                    };

                }

            }; 


            BasicMod.GameHooks.GoalManagerStartEvent.OnGoalManagerStart += (_, e) =>
            {
                Goal[] allGoalsCraftPotionWithEffect = GoalsLoader.allGoalsCraftPotionWithEffect;

                foreach (Goal goal in allGoalsCraftPotionWithEffect)
                {
                    Goal g = GoalFactory.CreateGoal(goal.name + "t3");
                    string effectName = goal.descriptionParameters[0].Remove(0, 8);

                    LocalDict.AddKeyToDictionary($"goal_{goal.name}t3", $"Make a tier 3 {effectName} potion.");

                    g.descriptionParameters = new List<string>();
                    g.descriptionParameters.Add(goal.descriptionParameters[0]);
                    g.experience = PotionEffect.GetByName(effectName).price/2;


                    tier3PotionGoals.Add(g);
                    
                    GoalFactory.AddGoalToChapter(g, chapter);
                }
            };

            PotionHealth.Start();
            SetUpGoals();
           
        }

        public static Chapter chapter;

        void SetUpGoals()
        {
            Goal g1 = GoalFactory.CreateGoal("finishhm");
            g1.experience = 1000;

            Goal g2 = GoalFactory.CreateGoal("potion10");
            g2.targetValue = 10;
            g2.experience = 50;

            Goal g3 = GoalFactory.CreateGoal("potion100");
            g3.targetValue = 100;
            g3.experience = 50;

            Goal g4 = GoalFactory.CreateGoal("potion1000");
            g4.targetValue = 1000;
            g4.experience = 50;

            chapter = GoalFactory.CreateChapter("HMchapter1");
            chapter.chapterGoal = g1;

            LocalDict.AddKeyToDictionary("goal_finishhm", "Complete all goals in HardMode.");
            LocalDict.AddKeyToDictionary("goal_potion10", "Make 10 potions.");
            LocalDict.AddKeyToDictionary("goal_potion100", "Make 100 potions.");
            LocalDict.AddKeyToDictionary("goal_potion1000", "Make 1000 potions.");

            LocalDict.AddKeyToDictionary("finish_current_chapter_to_complete_hmgoalbookchapter", "Each goal you complete in this chapter will increase your global EXP modifier.");

            GoalFactory.AddGoalToChapter(g2, chapter);
            GoalFactory.AddGoalToChapter(g3, chapter);
            GoalFactory.AddGoalToChapter(g4, chapter);


            ChaptersGroup goalbook = GoalFactory.CreateChaptersGroup("HMGoalbookchapter");
            GoalFactory.AddChapterToChapterGroup(chapter, goalbook);

            g2.onGoalCompleted.AddListener(delegate { ExperienceModifier.IncreaseModifier(1); });
            g3.onGoalCompleted.AddListener(delegate { ExperienceModifier.IncreaseModifier(1); });
            g4.onGoalCompleted.AddListener(delegate { ExperienceModifier.IncreaseModifier(1); });


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

            HardModeModifier expModifier = new ExperienceModifier();
            ExperienceModifier.instance = expModifier;
            ExperienceModifier.instance.LoadFromBindings(config);
        }

       
    }
}
