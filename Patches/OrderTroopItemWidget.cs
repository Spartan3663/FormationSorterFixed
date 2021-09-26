﻿using HarmonyLib;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets;

namespace FormationSorter
{
    [HarmonyPatch(typeof(OrderTroopItemWidget))]
    public static class PatchOrderTroopItemWidget
    {
        [HarmonyPatch("UpdateBackgroundState")]
        [HarmonyPostfix]
        public static void UpdateBackgroundState(OrderTroopItemWidget __instance)
        {
            if (__instance.IsSelectable && __instance.CurrentMemberCount <= 0)
            {
                __instance.SetState(__instance.IsSelected ? "Selected" : "Disabled");
            }
        }
    }
}