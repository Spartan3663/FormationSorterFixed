﻿using System;
using System.Collections.Generic;

using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace FormationSorter
{
    public static class Hotkeys
    {
        public static void OnApplicationTick()
        {
            if (!Mission.IsCurrentValid()) return;
            ProcessKey(Settings.OrderKey, () => Order.OnOrder());
            ProcessKey(Settings.TierSortKey, () => Order.OnOrder(tierSort: true));
            ProcessKey(Settings.SelectAllKey, () => Selection.SelectAllFormations());
            ProcessKey(Settings.SelectAllMeleeCavalryKey, () => Selection.SelectFormationsOfClasses(Selection.MeleeCavalryFormationClasses, "melee cavalry"));
            ProcessKey(Settings.SelectAllRangedCavalryKey, () => Selection.SelectFormationsOfClasses(Selection.RangedCavalryFormationClasses, "ranged cavalry"));
            ProcessKey(Settings.SelectAllGroundMeleeKey, () => Selection.SelectFormationsOfClasses(Selection.GroundMeleeFormationClasses, "ground melee"));
            ProcessKey(Settings.SelectAllGroundRangedKey, () => Selection.SelectFormationsOfClasses(Selection.GroundRangedFormationClasses, "ground ranged"));
            ProcessKey(Settings.SelectAllBasicMeleeKey, () => Selection.SelectFormationsOfClasses(Selection.BasicMeleeFormationClasses, "basic melee"));
            ProcessKey(Settings.SelectAllBasicRangedKey, () => Selection.SelectFormationsOfClasses(Selection.BasicRangedFormationClasses, "basic ranged"));
            ProcessKey(Settings.SelectAllGroundKey, () => Selection.SelectFormationsOfClasses(Selection.GroundFormationClasses, "ground"));
            ProcessKey(Settings.SelectAllCavalryKey, () => Selection.SelectFormationsOfClasses(Selection.CavalryFormationClasses, "cavalry"));
        }

        private static void ProcessKey(InputKey inputKey, Action action)
        {
            if (pressedLastTick is null)
                pressedLastTick = new Dictionary<InputKey, bool>();
            if (inputKey.IsDefinedAndDown())
            {
                if (!pressedLastTick.TryGetValue(inputKey, out bool b) || !b)
                {
                    pressedLastTick[inputKey] = true;
                    if (Mission.CanPlayerInteract() && inputKey == GetActionKey())
                        return;
                    action();
                }
            }
            else pressedLastTick[inputKey] = false;
        }

        private static Dictionary<InputKey, bool> pressedLastTick;

        private static InputKey GetActionKey() => TryGetGameKeyFromStringId("Action", out GameKey gameKey) ? gameKey.KeyboardKey.InputKey : InputKey.F;

        public static bool IsKeyBound(this InputKey inputKey) => IsDefined(inputKey) && (Settings.OrderKey == inputKey
            || Settings.ShieldSortingModifierKey == inputKey || Settings.SkirmisherSortingModifierKey == inputKey
            || Settings.EqualSortingModifierKey == inputKey || Settings.InverseSelectionModifierKey == inputKey
            || Settings.TierSortKey == inputKey || Settings.SelectAllKey == inputKey
            || Settings.SelectAllMeleeCavalryKey == inputKey || Settings.SelectAllRangedCavalryKey == inputKey
            || Settings.SelectAllGroundMeleeKey == inputKey || Settings.SelectAllGroundRangedKey == inputKey
            || Settings.SelectAllBasicMeleeKey == inputKey || Settings.SelectAllBasicRangedKey == inputKey
            || Settings.SelectAllGroundKey == inputKey || Settings.SelectAllCavalryKey == inputKey);

        public static bool IsGameKeyBound(string stringId) => !TryGetGameKeyFromStringId(stringId, out GameKey gameKey) || gameKey.KeyboardKey.InputKey.IsKeyBound();

        public static bool TryGetGameKeyFromStringId(string stringId, out GameKey gameKey)
        {
            List<GameKey> gameKeys = GenericGameKeyContext.Current?.GameKeys;
            if (!(gameKeys is null))
                foreach (GameKey _gameKey in gameKeys)
                    if (_gameKey.StringId == stringId && !(_gameKey.KeyboardKey is null))
                    {
                        gameKey = _gameKey;
                        return true;
                    }
            gameKey = null;
            return false;
        }

        public static bool IsDefined(this InputKey inputKey)
        {
            if (isKeyDefined.TryGetValue(inputKey, out bool defined))
                return defined;
            defined = Enum.IsDefined(typeof(InputKey), inputKey);
            isKeyDefined.Add(inputKey, defined);
            return defined;
        }

        private static readonly Dictionary<InputKey, bool> isKeyDefined = new Dictionary<InputKey, bool>();

        public static bool IsDefinedAndDown(this InputKey inputKey) => inputKey.IsDefined() && inputKey.IsDown();
    }
}