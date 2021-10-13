using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlayerManager;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(ExperienceSubManager), "AddExperience", new Type[] { typeof(float) })]
    public static class AddExperienceEvent
    {
        public static EventHandler<AddExperienceEventArgs> onAddExperienceEvent;

        public static void Prefix(ref float value)
        {
            var e = new AddExperienceEventArgs(value);
            onAddExperienceEvent?.Invoke(null, e);
            value = e.amount;
        }
    }

    public class AddExperienceEventArgs: EventArgs
    {
        public float amount;

        public AddExperienceEventArgs(float amount)
        {
            this.amount = amount;
        }
    }
}
