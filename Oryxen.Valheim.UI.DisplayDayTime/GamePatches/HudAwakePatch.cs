using HarmonyLib;

namespace Oryxen.Valheim.UI.DisplayDayTime.GamePatches
{
    [HarmonyPatch(typeof(Hud), "Awake")]
    public static class Hud_Awake_Patch
    {
        public static void Postfix(Hud __instance)
        {
            DisplayDayTimePlugin.CreatePanel(__instance);
        }
    }
}
