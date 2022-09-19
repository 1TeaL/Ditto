using R2API;
using System;

namespace DittoMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Ditto
            string prefix = DittoPlugin.developerPrefix + "_DITTO_BODY_";

            string desc = "Ditto can transform into any character/monster<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Ditto copies every stat besides regen, armor and movespeed. The transformation will have 10% of its Max HP + Ditto's Max HP" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use Ditto's equipment to transform back to ditto. It can also drop naturally" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Ditto's secondary and utility skills are items that when activated give buffs that are carried over when transformed as well" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Aim to increase your HP as he has low base HP" + Environment.NewLine + Environment.NewLine;



            string outro = "..and so it left, ever evolving.";
            string outroFailure = "Ditto has Fainted.";

            LanguageAPI.Add(prefix + "NAME", "Ditto");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Transform Pokémon");
            LanguageAPI.Add(prefix + "LORE", "Pokemon #132. Normal Type. Altering every cell in its body, Ditto can transform into a perfect copy of many different Pokémon.");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Shiny");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Ability");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Ditto comes with an equipment that transform yourself into a ditto.<style=cIsUtility> Ditto has a double jump. He can sprint in any direction.</style>");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_NAME", "Struggle");
            LanguageAPI.Add(prefix + "PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Struggle around, dealing <style=cIsDamage>{100f * StaticValues.struggleDamageCoefficient}%</style>" + " multiple times.");
            #endregion

            #region Items
            LanguageAPI.Add(prefix + "ASSAULTVEST_NAME", "Assault Vest");
            LanguageAPI.Add(prefix + "ASSAULTVEST_DESCRIPTION", $"Equip an Assault Vest, granting <style=cIsUtility>{(StaticValues.assaultvestboost)} armor</style>" + ".");
            LanguageAPI.Add(prefix + "CHOICEBAND_NAME", "Choice Band");
            LanguageAPI.Add(prefix + "CHOICEBAND_DESCRIPTION", $"Equip a Choice Band, granting <style=cIsDamage>{100f * (StaticValues.choicebandboost)}% attackspeed</style>" + ".");
            LanguageAPI.Add(prefix + "CHOICESCARF_NAME", "Choice Scarf");
            LanguageAPI.Add(prefix + "CHOICESCARF_DESCRIPTION", $"Equip a Choice Scarf, granting <style=cIsDamage>{100f * (StaticValues.choicescarfboost)}% movespeed</style>" + ".");
            LanguageAPI.Add(prefix + "CHOICESPECS_NAME", "Choice Specs");
            LanguageAPI.Add(prefix + "CHOICESPECS_DESCRIPTION", $"Equip a Choice Specs, granting <style=cIsUtility>{100f * (1-StaticValues.choicespecsboost)}% cooldown reduction</style>" + ".");
            LanguageAPI.Add(prefix + "LEFTOVERS_NAME", "Leftovers");
            LanguageAPI.Add(prefix + "LEFTOVERS_DESCRIPTION", $"Equip a Leftovers, granting " + Helpers.Passive($"{100f * (StaticValues.leftoversregen)}% of your max health regen") + " per second.");
            LanguageAPI.Add(prefix + "LIFEORB_NAME", "Life Orb");
            LanguageAPI.Add(prefix + "LIFEORB_DESCRIPTION", $"Equip a Life Orb, granting <style=cIsDamage>{100f * (StaticValues.lifeorbboost)}% damage</style>" + " </style>.");
            LanguageAPI.Add(prefix + "LUCKYEGG_NAME", "Lucky Egg");
            LanguageAPI.Add(prefix + "LUCKYEGG_DESCRIPTION", $"Equip a Lucky Egg, granting <style=cIsUtility>1 luck</style>" + " </style>.");
            LanguageAPI.Add(prefix + "ROCKYHELMET_NAME", "Rocky Helmet");
            LanguageAPI.Add(prefix + "ROCKYHELMET_DESCRIPTION", $"Equip a Rocky Helmet, stunning and dealing <style=cIsDamage>{100f * StaticValues.rockyhelmetreflect}% damage</style>" + " to attackers around you when hit.");
            LanguageAPI.Add(prefix + "SCOPELENS_NAME", "Scope Lens");
            LanguageAPI.Add(prefix + "SCOPELENS_DESCRIPTION", $"Equip a Scope Lens, granting <style=cIsDamage>{StaticValues.scopelensboost}% critical hit chance</style>" + ".");
            LanguageAPI.Add(prefix + "SHELLBELL_NAME", "Shell Bell");
            LanguageAPI.Add(prefix + "SHELLBELL_DESCRIPTION", $"Equip a Shell Bell, granting <style=cIsUtility>{100f * (StaticValues.shellbelllifesteal)}% lifesteal</style>" + ".");
            #endregion

            #region Abilities
            LanguageAPI.Add(prefix + "FLAMEBODY_NAME", "Flame Body");
            LanguageAPI.Add(prefix + "FLAMEBODY_DESCRIPTION", $"Survivors with this ability <style=cIsDamage>burn nearby attackers</style> when hit" + ".");
            LanguageAPI.Add(prefix + "HUGEPOWER_NAME", "Huge Power");
            LanguageAPI.Add(prefix + "HUGEPOWER_DESCRIPTION", $"Double the <style=cIsDamage>damage</style> of survivors with this ability" + ".");
            LanguageAPI.Add(prefix + "LEVITATE_NAME", "Levitate");
            LanguageAPI.Add(prefix + "LEVITATE_DESCRIPTION", $"Survivors with this ability can <style=cIsUtility>float by holding the jump button</style>" + ".");
            LanguageAPI.Add(prefix + "MAGICGUARD_NAME", "Magic Guard");
            LanguageAPI.Add(prefix + "MAGICGUARD_DESCRIPTION", $"Survivors with this ability <style=cIsUtility>are immune to DoTs</style>" + ".");
            LanguageAPI.Add(prefix + "MOODY_NAME", "Moody");
            LanguageAPI.Add(prefix + "MOODY_DESCRIPTION", $"Survivors with this ability <style=cIsUtility>Gain 2 stacks of a random buff, and 1 stack of a random debuff every {StaticValues.moodyTimer}</style>. " + Environment.NewLine +
                $"Damage increments by {100f * StaticValues.moodyDamage}%. " + Environment.NewLine +
                $"Armor increments by {StaticValues.moodyArmor}. " + Environment.NewLine +
                $"Movespeed increments by {100f * StaticValues.moodyMovespeed}%. " + Environment.NewLine +
                $"Attackspeed increments by {100f * StaticValues.moodyAttackspeed}%" + ".");
            LanguageAPI.Add(prefix + "MOXIE_NAME", "Moxie");
            LanguageAPI.Add(prefix + "MOXIE_DESCRIPTION", $"Survivors with this ability gain <style=cIsDamage>{100f * StaticValues.moxieDamage}% damage on kill for {StaticValues.moxieTimer} seconds</style>, stacking" + ".");
            LanguageAPI.Add(prefix + "MULTISCALE_NAME", "Multiscale");
            LanguageAPI.Add(prefix + "MULTISCALE_DESCRIPTION", $"Survivors with this ability take <style=cIsUtility>{100f * (StaticValues.multiscaleReduction)}% reduced damage while at full health</style>" + ".");
            LanguageAPI.Add(prefix + "SNIPER_NAME", "Sniper");
            LanguageAPI.Add(prefix + "SNIPER_DESCRIPTION", $"Survivors with this ability deal <style=cIsDamage>{100f * (StaticValues.sniperBoost)}% damage on critical hits</style>" + ".");           
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_NAME", "Transform");
            LanguageAPI.Add(prefix + "SPECIAL_DESCRIPTION", $"Transform into the character you're looking at. " +
                $"Drop the elite's equipment. After {StaticValues.transformDuration} seconds, you can press [{Config.transformHotkey.Value}] to transform back.");
            #endregion

            #region Achievements
            LanguageAPI.Add("ACHIEVEMENT_" + prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Ditto: Mastery");
            LanguageAPI.Add("ACHIEVEMENT_" + prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Ditto, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Ditto: Mastery");
            #endregion
            #endregion


        }
    }
}
