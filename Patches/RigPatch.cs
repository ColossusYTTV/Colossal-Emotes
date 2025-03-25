using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    public class RigPatch
    {
        public static bool Prefix(VRRig __instance)
        {
            return !(__instance == GorillaTagger.Instance.offlineVRRig);
        }
    }
}