using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    [BepInPlugin("net.catgocrihxpmods.PotionCraft.HardMode", "Potion Craft Hard Mode", "1.0")]
    public class TemplatePlugin : BaseUnityPlugin
    {   
      void Awake()
      {
        Debug.Log(@"[HardMode]: Loaded");
        var harmony = new Harmony("net.catgocrihxpmods.PotionCraft.Template");
        harmony.PatchAll();
      }
    }
}
