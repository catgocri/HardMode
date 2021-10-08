using BepInEx.Configuration;
using BepInEx;
using HarmonyLib;

using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
	public class NoStartTutorialModifier :HardModeModifier
	{

		public new string name = "Skip Tutorial";


        public override void LoadFromBindings(ConfigFile config)
        {
			SetActive(config.Bind(name + " Settings", "skipTutorial", true, "Skips the tutorial").Value);
        }
    }

	[HarmonyPatch(typeof(GameManager))]
	[HarmonyPatch("StartNewGame")]
	class NoStartTutorialPatch
	{
		static bool Prefix()
		{

			if(!NoStartTutorialModifier.instance.active)
            {
				//Returning true here skips our prefix
				return true;
            }

			//Do the StartNewGame stuff minus the tutorial
			Managers.Menu.CloseMenu();
			Managers.SaveLoad.LoadNewGameState();
			Managers.SaveLoad.SelectedProgressState = null;

			return false;
		}

	}

}