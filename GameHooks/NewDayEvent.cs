
using System;
using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(DayManager), "StartNewDay")]
    public static class NewDayEvent
    {
        public static EventHandler<NewDayEventArgs> OnNewDay;

        static void Postfix()
        {
            var e = new NewDayEventArgs(Managers.Day.CurrentDayAbsoluteNum);
            OnNewDay?.Invoke(null, e);
        }
    }

    public class NewDayEventArgs : EventArgs
    {
        public NewDayEventArgs(int day)
        {
            Day = day;
        }

        public int Day { get; }
    }
}