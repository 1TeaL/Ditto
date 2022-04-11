using BepInEx.Configuration;
using UnityEngine;

namespace DittoMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> choiceOnTeammate;
        public static ConfigEntry<bool> copyLoadout;

        public static void ReadConfig()
        {
            choiceOnTeammate = DittoPlugin.instance.Config.Bind("General", "Get Buffs From Teammates",true, "Whether you should get your ditto buffs when transforming into a teammate.");
            copyLoadout = DittoPlugin.instance.Config.Bind("General", "Copy loadout on transform",true, "Should you copy the loadout of characters you transform into.");
        }

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return DittoPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return DittoPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}