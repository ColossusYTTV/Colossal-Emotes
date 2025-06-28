using HarmonyLib;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    internal class DisableRig
    {
        public static bool Prefix(VRRig __instance)
        {
            return !(__instance == VRRig.LocalRig);
        }
    }

    [HarmonyPatch(typeof(VRRigJobManager), "DeregisterVRRig")]
    public static class DisableRigBypass
    {
        public static bool Prefix(VRRigJobManager __instance, VRRig rig)
        {
            return !(__instance == VRRig.LocalRig);
        }
    }
}