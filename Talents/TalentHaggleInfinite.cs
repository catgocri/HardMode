using BasicMod.Utility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentsMenu;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft.Talents
{
    public class TalentHaggleInfinite : TalentHaggle
    {
        public float baseWinCoefficient = 1.6f;

        public TalentHaggleInfinite()
        {
            onTalentEarned.AddListener(IncreaseLevel);
            winCoefficient = baseWinCoefficient;
        }

        public void IncreaseLevel()
        {
            winCoefficient *= 1.05f;
            cost += 1;
        }

        public override void Unearn()
        {
            base.Unearn();
            winCoefficient = baseWinCoefficient;

        }

    }


    [HarmonyPatch(typeof(Talent), "IsEarned")]
    static class TalentButtonInfintePatch
    {
        public static bool Prefix( Talent __instance, ref bool __result)
        {
            if ((__instance as TalentHaggleInfinite)!= null)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(TalentButton), "OnButtonReleasedPointerInside")]
    static class TalentButtonCostResetPatch
    {
        public static void Postfix(TalentButton __instance)
        {
            if ((__instance.talent as TalentHaggleInfinite) != null)
            {
                Reflection.InvokePrivateMethod(__instance, "UpdateButtonContent");
            };
        }
    }
}
