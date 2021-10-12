using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(RecipeMapManager), "MoveIndicatorTowardsBase")]
    public static class MoveIndicatorTowardsBaseEvent
    {
        public static EventHandler<MoveIndicatorTowardsBaseEventArgs> OnMoveIndicatorTowardsBase;

        static void Prefix(float value)
        {
            var e = new MoveIndicatorTowardsBaseEventArgs(value);
            OnMoveIndicatorTowardsBase?.Invoke(null, e);
        }
    }

    public class MoveIndicatorTowardsBaseEventArgs : EventArgs
    {
        public float liquidAmount;
        public MoveIndicatorTowardsBaseEventArgs(float value)
        {
            liquidAmount = value;
        }
    }

}
