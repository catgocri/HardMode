
using System;
using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(GameManager), "StartNewGame")]
    public static class NewGameEvent
    {
        public static EventHandler<NewGameEventArgs> OnNewGame;

        static bool Prefix()
        {
            var e = new NewGameEventArgs();
            OnNewGame?.Invoke(null, e);

            // If handled, do not let the original run;
            return !e.Handled;
        }
    }

    public class NewGameEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;
    }
}
