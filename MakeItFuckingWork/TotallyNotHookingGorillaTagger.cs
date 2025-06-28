
using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Awake")]
    internal class OnGameInit
    {
        public static void Prefix()
        {
            BepInPatch.CreateBepInPatch();
        }
    }

    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class BepInPatch : BaseUnityPlugin
    {
        private static GameObject gameob;

        BepInPatch()
        {
            new Harmony("ColossusYTTV.ColossalEmotes").PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void CreateBepInPatch()
        {
            if (gameob == null)
                gameob = new GameObject();
            gameob.name = "ColossalEmotes";
            gameob.AddComponent<Plugin>();
            UnityEngine.Object.DontDestroyOnLoad(gameob);
        }
    }
}
