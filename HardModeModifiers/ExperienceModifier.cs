using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using TMPro;
using System;
using Books.GoalsBook;
using System.Collections.Generic;
using System.Linq;
using BasicMod.Factories;
using BasicMod;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class ExperienceModifier : HardModeModifier
    {
        public new string name = "Experience";
        public static float modifier = 1.00f;
        public static EventHandler<ExperienceMultChangeEventArgs> onExperienceMultChangeEvent;

        public static TextMeshPro TMPMult;
        public static List<Goal> tier3PotionGoals = new List<Goal>();
        public static List<Goal> ourGoals = new List<Goal>(); //Help remember goals we add with this mod
        public static Chapter chapter;

        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "doExperienceModifier", true, "Adds hard mode goals, which give a global experience modifier when completed.").Value);
            AddExpModifierText(); //Always add the text. Show/hide if modifier is active or not

            if (this.active)
            {
                TMPMult.gameObject.SetActive(true);
                AddExperienceEvent.onAddExperienceEvent += ExperienceChange;

                onExperienceMultChangeEvent += UpdateText;

                BasicMod.GameHooks.GoalManagerStartEvent.OnGoalManagerStart += AddPotionGoals;

                AchievePotionGoalsEvent.onAchievePotionGoalsEvent += AchievePotionGoals;

                GoalsManagerLoadChaptersEvent.onGoalsManagerLoadChaptersEvent += LoadPotionGoals;


            }
            else
            {
                Clear();
            }

        }
      
        public void Clear()
        {
            TMPMult.gameObject.SetActive(false);
            AddExperienceEvent.onAddExperienceEvent -= ExperienceChange;
            onExperienceMultChangeEvent -= UpdateText;
            BasicMod.GameHooks.GoalManagerStartEvent.OnGoalManagerStart -= AddPotionGoals;
            AchievePotionGoalsEvent.onAchievePotionGoalsEvent -= AchievePotionGoals;
            GoalsManagerLoadChaptersEvent.onGoalsManagerLoadChaptersEvent -= LoadPotionGoals;

        }


        public static void AddExpModifierText()
        {
            var textHolder = new GameObject();
            textHolder.name = "ExpModifierTextHolder";
            textHolder.transform.Translate(HardModePlugin.logoPos + new Vector3(0, -1.7f));
            textHolder.layer = 5;

            TMPMult = textHolder.AddComponent<TextMeshPro>();
            TMPMult.alignment = TextAlignmentOptions.Center;
            TMPMult.enableAutoSizing = true;
            TMPMult.sortingLayerID = -1650695527;
            TMPMult.sortingOrder = 100;
            TMPMult.fontSize = 3;
            TMPMult.fontSizeMin = 3;
            TMPMult.fontSizeMax = 3;
            TMPMult.color = new Color32(57, 30, 20, 255);
            TMPMult.text = "Multiplier";

            GameObject panel = GameObject.Find("Room Lab/RecipeMap In Room/UI");
            if (panel is not null)
            {
                textHolder.transform.SetParent(panel.transform);
            }
        }

        public static void IncreaseModifier(float amount, bool silent = false)
        {
            modifier += amount;
            var e = new ExperienceMultChangeEventArgs();
            onExperienceMultChangeEvent.Invoke(null,e);
            
            if(!silent) Notification.ShowText("EXP Modifier", "+" + Math.Round(amount, 2) + " modifier!", Notification.TextType.LevelUpText);
        }

        public void AddPotionGoals(object sender, EventArgs e)
        {

            //Hard coded goals start

            Goal g1 = GoalFactory.CreateGoal("finishhm");
            g1.experience = 1000;

            Goal g2 = GoalFactory.CreateGoal("potion10");
            g2.targetValue = 10;
            g2.experience = 50;
            float fg2experience = (float)g2.experience;

            Goal g3 = GoalFactory.CreateGoal("potion100");
            g3.targetValue = 100;
            g3.experience = 50;
            float fg3experience = (float)g3.experience;

            Goal g4 = GoalFactory.CreateGoal("potion1000");
            g4.targetValue = 1000;
            g4.experience = 50;
            float fg4experience = (float)g4.experience;

            ourGoals.Add(g1);
            ourGoals.Add(g2);
            ourGoals.Add(g3);
            ourGoals.Add(g4);


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

            g2.onGoalCompleted.AddListener(delegate { ExperienceModifier.IncreaseModifier(fg2experience / 500f); });
            g3.onGoalCompleted.AddListener(delegate { ExperienceModifier.IncreaseModifier(fg3experience / 500f); });
            g4.onGoalCompleted.AddListener(delegate { ExperienceModifier.IncreaseModifier(fg4experience / 500f); });

            //Hard coded goals end


           
            //Potion goals start
            Goal[] allGoalsCraftPotionWithEffect = GoalsLoader.allGoalsCraftPotionWithEffect;

            foreach (Goal goal in allGoalsCraftPotionWithEffect)
            {
                Goal g = GoalFactory.CreateGoal(goal.name + "t3");
                string effectName = goal.descriptionParameters[0].Remove(0, 8);

                LocalDict.AddKeyToDictionary($"goal_{goal.name}t3", $"Make a tier 3 {effectName} potion.");

                g.descriptionParameters = new List<string>();
                g.descriptionParameters.Add(goal.descriptionParameters[0]);
                g.experience = PotionEffect.GetByName(effectName).price / 2;
                float fgexperience = (float)g.experience;
                g.onGoalCompleted.AddListener(delegate { ExperienceModifier.IncreaseModifier(fgexperience / 500f); });

                ourGoals.Add(g);
                tier3PotionGoals.Add(g);


                GoalFactory.AddGoalToChapter(g, chapter);
            }
        }

        public void AchievePotionGoals(object sender, AchievePotionGoalsEventArgs e) {

            GoalsLoader.GetGoalByName("potion10").ProgressIncrement();
            GoalsLoader.GetGoalByName("potion100").ProgressIncrement();
            GoalsLoader.GetGoalByName("potion1000").ProgressIncrement();

            int count = (int)Potion.GetArrayOfEffectTypes(e.potion.effects)[0][0];
            PotionEffect effect = (PotionEffect)Potion.GetArrayOfEffectTypes(e.potion.effects)[0][1];

            if (count == 3)
            {
                Goal goal = (from g in tier3PotionGoals
                             where g.descriptionParameters[0] == "#effect_" + effect.name
                             select g).FirstOrDefault<Goal>();


                if (goal != null)
                {
                    goal.ProgressIncrement();
                };

            }
        }

        public void LoadPotionGoals(object sender, EventArgs e)
        {
            //Easy way to save and load
            IncreaseModifier(-modifier + 1, true); //Set its back to 1

            foreach (Goal goal in ourGoals)
            {
                if (goal.IsCompleted())
                {
                    float fgoalexperience = (float)goal.experience;
                    ExperienceModifier.IncreaseModifier(fgoalexperience / 500f, true);
                }
            }
        }

        public void ExperienceChange(object sender, AddExperienceEventArgs e)
        {
            e.amount *= modifier;
        }
        public void UpdateText(object sender, EventArgs e)
        {
            if (TMPMult != null)
            {
                TMPMult.text = "EXP Modifier: " + Math.Round(modifier, 2) + "x";
            }
        }
    }

   

    public class ExperienceMultChangeEventArgs : EventArgs
    {
        public ExperienceMultChangeEventArgs()
        {

        }

    }
}
