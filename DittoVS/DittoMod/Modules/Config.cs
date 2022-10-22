using BepInEx.Configuration;
using RiskOfOptions.Options;
using RiskOfOptions;
using UnityEngine;
using RiskOfOptions.OptionConfigs;

namespace DittoMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> choiceOnTeammate;
        public static ConfigEntry<bool> copyLoadout;
        public static ConfigEntry<bool> copyHealth;
        public static ConfigEntry<bool> bossTimerOn;
        public static ConfigEntry<int> bossTimer;
        public static ConfigEntry<bool> grabAspect;
        public static ConfigEntry<float> damageMultiplier;
        public static ConfigEntry<float> movespeedMultiplier;
        public static ConfigEntry<float> attackspeedMultiplier;
        public static ConfigEntry<KeyboardShortcut> transformHotkey { get; set; }
        public static void ReadConfig()
        {
            choiceOnTeammate = DittoPlugin.instance.Config.Bind("General", "Get Buffs From Teammates",true, "Whether you should get your ditto buffs when transforming into a teammate.");
            copyLoadout = DittoPlugin.instance.Config.Bind("General", "Copy loadout on transform",true, "Should you copy the loadout of characters you transform into.");
            copyHealth = DittoPlugin.instance.Config.Bind("General", "Copy fractional health",true, "Should you copy the fractional health of your previous state when transforming.");
            bossTimerOn = DittoPlugin.instance.Config.Bind("Transformations", "Adds timers to Boss transformations", true, "Should you add a timer to bosses when transforming.");
            bossTimer = DittoPlugin.instance.Config.Bind<int>("Transformations", "Boss transform duration", 30, "Adjusts duration of staying as a boss before transforming back.");
            grabAspect = DittoPlugin.instance.Config.Bind("Transformations", "Drop Elite Aspects on transform", true, "Whether you should drop elite aspects when transforming into an elite.");
            damageMultiplier = DittoPlugin.instance.Config.Bind<float>("Multipliers", "Transform damage multiplier", 1f, "Adjusts damage multiplier for your transform.");
            movespeedMultiplier = DittoPlugin.instance.Config.Bind<float>("Multipliers", "Transform movespeed multiplier", 1f, "Adjusts movespeed multiplier for your transform.");
            attackspeedMultiplier = DittoPlugin.instance.Config.Bind<float>("Multipliers", "Transform attackspeed multiplier", 1f, "Adjusts attackspeed multiplier for your transform.");

            transformHotkey = DittoPlugin.instance.Config.Bind<KeyboardShortcut>("Input", "Transform Key", new KeyboardShortcut(UnityEngine.KeyCode.F), "Keybinding for Transform");
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
        public static void SetupRiskOfOptions()
        {
            //Risk of Options intialization
            ModSettingsManager.AddOption(new KeyBindOption(
                transformHotkey));
            ModSettingsManager.AddOption(new CheckBoxOption(
                choiceOnTeammate));
            ModSettingsManager.AddOption(new CheckBoxOption(
                copyLoadout));
            ModSettingsManager.AddOption(new CheckBoxOption(
                copyHealth));
            ModSettingsManager.AddOption(new CheckBoxOption(
                grabAspect));
            ModSettingsManager.AddOption(new CheckBoxOption(
                bossTimerOn));
            ModSettingsManager.AddOption(new IntSliderOption(
                bossTimer, new IntSliderConfig() { min = 30, max = 300}));
            ModSettingsManager.AddOption(new StepSliderOption(
                damageMultiplier, new StepSliderConfig() { min = 1, max = 100, increment = 1f }));
            ModSettingsManager.AddOption(new StepSliderOption(
                movespeedMultiplier, new StepSliderConfig() { min = 1, max = 100, increment = 1f }));
            ModSettingsManager.AddOption(new StepSliderOption(
                attackspeedMultiplier, new StepSliderConfig() { min = 1, max = 100, increment = 1f }));

        }

    }

}
