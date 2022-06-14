using System;
using System.Collections.Generic;

namespace DittoMod.Modules
{
    internal static class StaticValues
    {

        internal const float struggleDamageCoefficient = 0.3f;
        internal const float invincibilityDuration = 1f;

        internal const float assaultvestboost = 100f;
        internal const float choicebandboost = 1.5f;
        internal const float choicescarfboost = 1.5f;
        internal const float choicespecsboost = 0.75f;
        internal const float leftoversregen = 0.03125f;
        internal const float lifeorbboost = 1.5f;
        internal const float luckyeggboost = 1f;
        internal const float rockyhelmetreflect = 1f;
        internal const float scopelensboost = 30f;
        internal const float shellbelllifesteal = 0.05f;
        internal const float assaultvestboost2 = 200f;
        internal const float choicebandboost2 = 2f;
        internal const float choicescarfboost2 = 2f;
        internal const float choicespecsboost2 = 0.5f;
        internal const float leftoversregen2 = 0.0625f;
        internal const float lifeorbboost2 = 2f;
        internal const float luckyeggboost2 = 2f;
        internal const float rockyhelmetreflect2 = 2f;
        internal const float scopelensboost2 = 60f;
        internal const float shellbelllifesteal2 = 0.1f;

        internal const float TransformEquipmentCooldown = 60f;

        internal const int transformDuration = 30;

        internal const int enemytransformTimer = 5;

        public static List<string> blacklist = new List<string>();
        public static List<string> speciallist = new List<string>();
        public static List<string> bosslist = new List<string>();

        public static void LoadList()
        {
            blacklist.Add("DroneCommanderBody");
            blacklist.Add("ExplosivePotDestructibleBody");
            blacklist.Add("SulfurPodBody");
            blacklist.Add("DittoBody");
            blacklist.Add("AffixEarthHealerBody");
            blacklist.Add("MinorConstructAttachableBody");
            blacklist.Add("ClayGrenadierBody");
            blacklist.Add("SMMaulingRockLarge");
            blacklist.Add("SMMaulingRockMedium");
            blacklist.Add("SMMaulingRockSmall");
            blacklist.Add("VultureEggBody");
            blacklist.Add("VoidInfestorBody");
            blacklist.Add("GokuBody");
            blacklist.Add("VegetaBody");
            blacklist.Add("TrunksBody");
            blacklist.Add("BeetleWard");

            speciallist.Add("NullifierBody");
            speciallist.Add("VoidJailerBody");
            speciallist.Add("MinorConstructBody");
            speciallist.Add("MinorConstructOnKillBody");
            speciallist.Add("MiniVoidRaidCrabBodyPhase1");
            speciallist.Add("MiniVoidRaidCrabBodyPhase2");
            speciallist.Add("MiniVoidRaidCrabBodyPhase3");
            speciallist.Add("ElectricWormBody");
            speciallist.Add("MagmaWormBody");
            speciallist.Add("BeetleQueen2Body");
            speciallist.Add("TitanBody");
            speciallist.Add("TitanGoldBody");
            speciallist.Add("VagrantBody");
            speciallist.Add("GravekeeperBody");
            speciallist.Add("ClayBossBody");
            speciallist.Add("RoboBallBossBody");
            speciallist.Add("SuperRoboBallBossBody");
            speciallist.Add("MegaConstructBody");
            speciallist.Add("VoidInfestorBody");
            speciallist.Add("VoidBarnacleBody");
            speciallist.Add("MegaConstructBody");
            speciallist.Add("VoidMegaCrabBody");
            speciallist.Add("GrandParentBody");
            speciallist.Add("ImpBossBody");
            speciallist.Add("BrotherBody");
            speciallist.Add("BrotherHurtBody");
            speciallist.Add("ScavBody");

            bosslist.Add("ElectricWormBody");
            bosslist.Add("MagmaWormBody");
            bosslist.Add("BeetleQueen2Body");
            bosslist.Add("TitanBody");
            bosslist.Add("TitanGoldBody");
            bosslist.Add("VagrantBody");
            bosslist.Add("GravekeeperBody");
            bosslist.Add("ClayBossBody");
            bosslist.Add("RoboBallBossBody");
            bosslist.Add("SuperRoboBallBossBody");
            bosslist.Add("MegaConstructBody");
            bosslist.Add("VoidMegaCrabBody");
            bosslist.Add("GrandParentBody");
            bosslist.Add("ImpBossBody");
            bosslist.Add("BrotherBody");
            bosslist.Add("BrotherHurtBody");
            bosslist.Add("ScavBody");

        }
    }
}
