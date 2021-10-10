using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(IndicatorMapItem), "MoveIndicatorAlongThePath")]
    public static class MoveIndicatorAlongPathEvent
    {
        public static EventHandler<MoveIndicatorAlongPathEventArgs> OnMoveIndicatorAlongPath;

        static void Postfix(IndicatorMapItem __instance)
        {
            var e = new MoveIndicatorAlongPathEventArgs(__instance.targetPosition);
            OnMoveIndicatorAlongPath?.Invoke(null, e);
        }
    }

    public class MoveIndicatorAlongPathEventArgs : EventArgs
    {
        public Vector2 TargetPosition;
        public MoveIndicatorAlongPathEventArgs(Vector2 pos)
        {
            TargetPosition = pos;
        }
    }

}
