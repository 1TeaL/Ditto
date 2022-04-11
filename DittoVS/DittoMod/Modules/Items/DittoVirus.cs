using BepInEx.Configuration;
using DittoMod.Items;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DittoMod.Modules.Items
{
    public class DittoVirus : ItemBase<DittoVirus>
    {

        public static GameObject ItemBodyModelPrefab;

        public float procChance { get; private set; } = 100f;

        public override string ItemName => "Ditto Virus";

        public override string ItemLangTokenName => "DITTO_VIRUS";

        public override string ItemPickupDesc => $"Chance to transform an enemy into a ditto after 5 seconds.";

        public override string ItemFullDescription => $"<style=cIsUtility>{procChance}%</style> chance to <style=cIsUtility>transform</style> an enemy into a ditto.</style>";

        public override string ItemLore => "The day will come when Ditto's rule the world. ";

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override ItemTier Tier => ItemTier.Lunar;

        public override GameObject ItemModel => Modules.Assets.DittoEquipmentPrefab;

        public override Sprite ItemIcon => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texDittoIcon");


        public override void Init(ConfigFile config)
        {
            CreateLang();
            CreateItem();
            Hooks();
            CreateItemDisplayRules();
        }
        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            //no model for becomeditto.
            return rules;
        }

        public override void Hooks()
        {

            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

        }
        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            List<string> speciallist = new List<string>();
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

            orig(self, di);


            CharacterBody oldBody = self.body;
            CharacterBody body;

            if (di == null || di.rejected || !di.attacker || di.attacker == self.gameObject) return;

            var cb = di.attacker.GetComponent<CharacterBody>();
            if (cb)
            {
                var icnt = GetCount(cb);
                if (icnt < 1) return;
                var proc = cb.master ? Util.CheckRoll(procChance, cb.master) : Util.CheckRoll(procChance);
                if (proc)
                {
                    if (!self.body.isChampion)
                    {
                        if (self.body.master.bodyPrefab.name != "DittoBody")
                        {

                            body = self.body.master.Respawn(self.body.master.GetBody().transform.position, self.body.master.GetBody().transform.rotation);
                            body.healthComponent.health = oldBody.healthComponent.health;
                            body.baseMaxHealth = oldBody.baseMaxHealth;
                            body.levelMaxHealth = oldBody.levelMaxHealth;
                            body.maxHealth = oldBody.maxHealth;
                            body.baseRegen = oldBody.regen;
                            body.baseJumpCount = oldBody.baseJumpCount;
                            body.maxJumpCount = oldBody.maxJumpCount;
                            body.maxJumpHeight = oldBody.maxJumpHeight;
                            body.jumpPower = oldBody.jumpPower;
                            body.baseJumpPower = oldBody.baseJumpPower;


                        }

                    }
                }
            }
        }
    }
}