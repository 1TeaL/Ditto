using BepInEx;
using BepInEx.Bootstrap;
using DittoMod.Equipment;
using DittoMod.Modules;
//using DittoMod.Modules.Networking;
using DittoMod.Modules.Survivors;
using DittoMod.SkillStates;
using R2API.Networking;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace DittoMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "ItemAPI"
    })]

    public class DittoPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said

        public static bool scepterInstalled = false;

        public const string MODUID = "com.TeaL.DittoMod";
        public const string MODNAME = "DittoMod";
        public const string MODVERSION = "0.1";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "TEAL";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        public static DittoPlugin instance;
        public static CharacterBody DittoCharacterBody;

        private void Awake()
        {
            instance = this;
            DittoCharacterBody = null;
            DittoPlugin.instance = this;

            // load assets and read config
            Modules.Assets.Initialize();
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new Ditto().Initialize();

            //Equipment Initialization;
            var EquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EquipmentBase)));
            foreach (var equipmentType in EquipmentTypes)
            {
                EquipmentBase equipment = (EquipmentBase)System.Activator.CreateInstance(equipmentType);
                equipment.Init();
            }

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;

            Hook();
        }

        private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            // have to set item displays later now because they require direct object references..
            Modules.Survivors.Ditto.instance.SetItemDisplays();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            On.RoR2.CharacterMaster.Start += CharacterMaster_Start;
            On.RoR2.CharacterBody.Start += CharacterBody_Start;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_OnDamageDealt;
        }
        //lifesteal
        private void GlobalEventManager_OnDamageDealt(DamageReport report)
        {

            bool flag = !report.attacker || !report.attackerBody;
            if (!flag && report.attackerBody.HasBuff(Modules.Buffs.shellbellBuff))
            {
                CharacterBody attackerBody = report.attackerBody;
                attackerBody.healthComponent.Heal(report.damageDealt * Modules.StaticValues.shellbelllifesteal, default(ProcChainMask), true);

            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self.body.HasBuff(Modules.Buffs.rockyhelmetBuff.buffIndex))
            {
                var damageInfo2 = new DamageInfo();

                damageInfo2.damage = self.body.damage * Modules.StaticValues.rockyhelmetreflect;
                damageInfo2.position = damageInfo.attacker.transform.position;
                damageInfo2.force = Vector3.zero;
                damageInfo2.damageColorIndex = DamageColorIndex.Default;
                damageInfo2.crit = Util.CheckRoll(self.body.crit, self.body.master);
                damageInfo2.attacker = self.gameObject;
                damageInfo2.inflictor = null;
                damageInfo2.damageType = DamageType.BypassArmor | DamageType.Generic;
                damageInfo2.procCoefficient = 0.5f;
                damageInfo2.procChainMask = default(ProcChainMask);

                if (damageInfo.attacker.gameObject.GetComponent<CharacterBody>().baseNameToken
                    != DittoPlugin.developerPrefix + "_DITTO_BODY_NAME" && damageInfo.attacker != null)
                {
                    damageInfo.attacker.GetComponent<CharacterBody>().healthComponent.TakeDamage(damageInfo2);
                }

                Vector3 enemyPos = damageInfo.attacker.transform.position;
                EffectManager.SpawnEffect(effectPrefab, new EffectData
                {
                    origin = enemyPos,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(enemyPos - self.body.transform.position)

                }, false);

            }
            orig.Invoke(self, damageInfo);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            //buffs 
            orig.Invoke(self);

            
            if (self.HasBuff(Modules.Buffs.choicebandBuff))
            {
                self.attackSpeed *= Modules.StaticValues.choicebandboost;
            }
            if (self.HasBuff(Modules.Buffs.choicescarfBuff))
            {
                self.moveSpeed *= Modules.StaticValues.choicescarfboost;
            }
            if (self.HasBuff(Modules.Buffs.choicespecsBuff))
            {
                self.damage *= Modules.StaticValues.choicespecsboost;
            }
            if (self.HasBuff(Modules.Buffs.scopelensBuff))
            {
                self.crit += Modules.StaticValues.scopelensboost;
            }
            if (self.HasBuff(Modules.Buffs.leftoversBuff))
            {
                HealthComponent hp = self.healthComponent;
                float regenValue = hp.fullCombinedHealth * Modules.StaticValues.leftoversregen;
                self.regen += regenValue;
                //Chat.AddMessage("hpregen activated");
            }
            
        }

        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            orig(self);
            if (self.baseNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_NAME")
            {
                AkSoundEngine.PostEvent(3468082827, this.gameObject);
            }

        }
        private void CharacterMaster_Start(On.RoR2.CharacterMaster.orig_Start orig, CharacterMaster self)
        {
            orig.Invoke(self);

            if (self.bodyPrefab)
            {
                Debug.Log(self.bodyPrefab);
                if (self.bodyPrefab.name.Contains("Ditto"))
                {
                    //Give equipment
                    self.inventory.GiveEquipmentString("EQUIPMENT_BECOME_DITTO");
                    //setup UI
                }
            }
        }
        private void CharacterBody_Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            orig.Invoke(self);

            if (self.baseNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_NAME")
            {
                AkSoundEngine.PostEvent(3468082827, this.gameObject);

                self.RemoveBuff(RoR2Content.Buffs.OnFire);
                self.RemoveBuff(RoR2Content.Buffs.AffixBlue);
                self.RemoveBuff(RoR2Content.Buffs.AffixEcho);
                self.RemoveBuff(RoR2Content.Buffs.AffixHaunted);
                self.RemoveBuff(RoR2Content.Buffs.AffixLunar);
                self.RemoveBuff(RoR2Content.Buffs.AffixPoison);
                self.RemoveBuff(RoR2Content.Buffs.AffixRed);
                self.RemoveBuff(RoR2Content.Buffs.AffixWhite);
                self.RemoveBuff(DittoMod.Modules.Assets.fireelitebuff);
                self.RemoveBuff(DittoMod.Modules.Assets.iceelitebuff);
                self.RemoveBuff(DittoMod.Modules.Assets.hauntedelitebuff);
                self.RemoveBuff(DittoMod.Modules.Assets.lightningelitebuff);
                self.RemoveBuff(DittoMod.Modules.Assets.mendingelitebuff);
                self.RemoveBuff(DittoMod.Modules.Assets.malachiteelitebuff);
                self.RemoveBuff(DittoMod.Modules.Assets.speedelitebuff);
                self.RemoveBuff(DittoMod.Modules.Assets.voidelitebuff);
                self.RemoveBuff(DittoMod.Modules.Assets.lunarelitebuff);
            }
        }

        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("DittoDisplay"))
            {
                AkSoundEngine.PostEvent(3468082827, this.gameObject);
            }

        }
        
        //private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        //{
        //    orig(self);
                        

        //}

    }
}