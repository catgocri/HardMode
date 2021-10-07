using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace catgocri.HardMode.PotionCraft
{
    [BepInPlugin("net.catgocri.PotionCraft.HardMode", "Potion Craft Hard Mode", "1.0")]
    public class TemplatePlugin : BaseUnityPlugin
    {   
      void Awake()
      {
        Debug.Log(@"[HardMode]: Loaded");
        var harmony = new Harmony("net.catgocri.PotionCraft.Template");
        harmony.PatchAll();
      }
    }
}