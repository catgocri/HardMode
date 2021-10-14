using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using TMPro;
using System;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class ExperienceModifier : HardModeModifier
    {
        public new string name = "Experience";
        public static float modifier = 1;
        public static EventHandler<ExperienceMultChangeEventArgs> onExperienceMultChangeEvent;
        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "doExperienceModifier", true, "If completing hard mode goals should add an experience multiplier.").Value);

            AddExpModifierText();

            onExperienceMultChangeEvent += (_, e) =>
            {
                TMPMult.text = "Modifier: " + modifier;
            };

            if (this.active)
            {
                AddExperienceEvent.onAddExperienceEvent += (sender, e) =>
                {
                    //Debug.Log(e.amount);
                    e.amount *= modifier;
                };
            }



        }

        public static TextMeshPro TMPMult;

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
            TMPMult.color = Color.black;
            TMPMult.text = "Multiplier";

            GameObject panel = GameObject.Find("Room Lab/RecipeMap In Room/UI");
            if (panel is not null)
            {
                textHolder.transform.SetParent(panel.transform);
            }
        }

        public static void IncreaseModifier(float amount)
        {
            modifier += amount;
            var e = new ExperienceMultChangeEventArgs();
            onExperienceMultChangeEvent.Invoke(null,e);
        }
    }


    public class ExperienceMultChangeEventArgs : EventArgs
    {
        public ExperienceMultChangeEventArgs()
        {

        }

    }
}
