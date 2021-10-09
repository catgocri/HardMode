using BepInEx.Configuration;
using catgocrihxpmods.HardMode.PotionCraft.GameHooks;
using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public class NoStartTutorialModifier : HardModeModifier
    {

        public new string name = "Skip Tutorial";

        public override void LoadFromBindings(ConfigFile config)
        {
            SetActive(config.Bind(name + " Settings", "skipTutorial", true, "Skips the tutorial").Value);

            if (this.active)
            {
                NewGameEvent.OnNewGame += (_, e) =>
                {
                    // Mark the event handled so the game's own code doesnt run.
                    e.Handled = true;

                    //Do the StartNewGame stuff minus the tutorial
                    Managers.Menu.CloseMenu();
                    Managers.SaveLoad.LoadNewGameState();
                    Managers.SaveLoad.SelectedProgressState = null;
                };
            }
        }
    }
}