using BepInEx;
using BepInEx.Bootstrap;
using DittoMod.Modules;
//using DittoMod.Modules.Networking;
using DittoMod.Modules.Survivors;
using DittoMod.SkillStates;
using R2API.Networking;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
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
        "NetworkingAPI",
        "SkinAPI",
        "LoadoutAPI",
        "DamageAPI"
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

            //networking
            //NetworkingAPI.RegisterMessageType<ForceCounterState>();

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
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_OnDamageDealt;
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            //On.RoR2.HealthComponent.Awake += HealthComponent_Awake;
        }

        //private void HealthComponent_Awake(On.RoR2.HealthComponent.orig_Awake orig, HealthComponent healthComponent)
        //{
        //    //if (healthComponent)
        //    //{
        //    //    if (!healthComponent.gameObject.GetComponent<DangerSenseComponent>())
        //    //    {
        //    //        healthComponent.gameObject.AddComponent<DangerSenseComponent>();
        //    //    }
                    
        //    //}


        //    //orig.Invoke(healthComponent);
        //}

        //private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        //{
        //    if (self.body.HasBuff(Modules.Buffs.counterBuff.buffIndex))
        //    {
        //        damageInfo.damage = 0f;
        //        self.body.RemoveBuff(Modules.Buffs.counterBuff.buffIndex);
        //        Debug.Log(self.body.HasBuff(Modules.Buffs.counterBuff.buffIndex));

        //        var dekucon = self.body.gameObject.GetComponent<DekuController>();
        //        dekucon.countershouldflip = true;

        //        var damageInfo2 = new DamageInfo();

        //        damageInfo2.damage = self.body.damage * Modules.StaticValues.counterDamageCoefficient;
        //        damageInfo2.position = damageInfo.attacker.transform.position;
        //        damageInfo2.force = Vector3.zero;
        //        damageInfo2.damageColorIndex = DamageColorIndex.Default;
        //        damageInfo2.crit = Util.CheckRoll(self.body.crit, self.body.master);
        //        damageInfo2.attacker = self.gameObject;
        //        damageInfo2.inflictor = null;
        //        damageInfo2.damageType = DamageType.BypassArmor | DamageType.Stun1s;
        //        damageInfo2.procCoefficient = 2f;
        //        damageInfo2.procChainMask = default(ProcChainMask);

        //        if (damageInfo.attacker.gameObject.GetComponent<CharacterBody>().baseNameToken
        //            != DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && damageInfo.attacker != null)
        //        {
        //            damageInfo.attacker.GetComponent<CharacterBody>().healthComponent.TakeDamage(damageInfo2);
        //        }

        //        Vector3 enemyPos = damageInfo.attacker.transform.position;
        //        EffectManager.SpawnEffect(Modules.Projectiles.airforceTracer, new EffectData
        //        {
        //            origin = self.body.transform.position,
        //            scale = 1f,
        //            rotation = Quaternion.LookRotation(enemyPos - self.body.transform.position)

        //        }, true);


        //        DangerSenseCounter dangersenseCounter = new DangerSenseCounter();
        //        dangersenseCounter.enemyPosition = enemyPos;
        //        self.body.gameObject.GetComponent<EntityStateMachine>().SetState(dangersenseCounter);



        //        if (self.body.characterMotor && self.body.characterDirection)
        //        {
        //            self.body.characterMotor.rootMotion += (self.body.transform.position - damageInfo.attacker.transform.position).normalized * self.body.moveSpeed;
        //        }

        //        EntityStateMachine[] stateMachines = self.body.gameObject.GetComponents<EntityStateMachine>();
        //        foreach (EntityStateMachine stateMachine in stateMachines)
        //        {
        //            if (stateMachine.customName == "Body")
        //            {

        //                self.body.gameObject.GetComponent<EntityStateMachine>().SetNextState(new DangerSenseCounter
        //                {
        //                    enemyPosition = enemyPos
        //                });


        //            }

        //        }

        //    }
        //    orig.Invoke(self, damageInfo);
        //}

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            //regen 
            orig.Invoke(self);
        }
        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            orig(self);
            if (self.baseNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_NAME")
            {
                //AkSoundEngine.PostEvent(779278001, this.gameObject);
            }

        }
        //lifesteal
        private void GlobalEventManager_OnDamageDealt(DamageReport report)
        {
            
        }


        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("DittoDisplay"))
            {
                //AkSoundEngine.PostEvent(2656882895, this.gameObject);
            }

        }
        
        private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);
                        

        }

    }
}