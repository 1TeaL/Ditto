using BepInEx;
using BepInEx.Bootstrap;
using DittoMod.Equipment;
using DittoMod.Modules;
//using DittoMod.Modules.Networking;
using DittoMod.Modules.Survivors;
using DittoMod.SkillStates;
using R2API;
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
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

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
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "TEAL";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        public static DittoPlugin instance;
        public static CharacterBody DittoCharacterBody;
        GameObject voidcrabphase1 = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase1.prefab").WaitForCompletion();
        GameObject voidcrabphase2 = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase2.prefab").WaitForCompletion();
        GameObject voidcrabphase3 = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase3.prefab").WaitForCompletion();
        //GameObject shopkeeper = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Shopkeeper/ShopkeeperBody.prefab").WaitForCompletion();
        GameObject xiconstruct = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/MajorAndMinorConstruct/MegaConstructBody.prefab").WaitForCompletion();
        GameObject alphaconstruct = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/MajorAndMinorConstruct/MinorConstructBody.prefab").WaitForCompletion();
        GameObject voidinfestor = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/EliteVoid/VoidInfestorBody.prefab").WaitForCompletion();
        GameObject voidbarnacle = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidBarnacle/VoidBarnacleBody.prefab").WaitForCompletion();
        GameObject voidjailer = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidJailer/VoidJailerBody.prefab").WaitForCompletion();
        GameObject voidmegacrab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidMegaCrab/VoidMegaCrabBody.prefab").WaitForCompletion();
        GameObject droneman = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/DroneCommander/DroneCommanderBody.prefab").WaitForCompletion();
        GameObject gip = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GipBody.prefab").WaitForCompletion();
        GameObject geep = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GeepBody.prefab").WaitForCompletion();
        GameObject gup = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GupBody.prefab").WaitForCompletion();

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

            //give equipment slot
            GameObject shopkeeper = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/ShopkeeperBody");
            GameObject beetlequeen = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/BeetleQueen2Body");
            GameObject golem = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GolemBody");
            GameObject titan = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TitanBody");
            GameObject titangold = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TitanGoldBody");
            GameObject gravekeeper = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GravekeeperBody");
            GameObject vagrant = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/VagrantBody");
            GameObject magmaworm = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MagmaWormBody");
            GameObject overloadingworm = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ElectricWormBody");
            GameObject claydunestrider = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBossBody");
            GameObject roboballboss = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallBossBody");
            GameObject superroboballboss = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SuperRoboBallBossBody");
            GameObject xiconstruct2 = PrefabAPI.InstantiateClone(xiconstruct, "1xiconstruct");
            GameObject grandparent = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GrandParentBody");
            GameObject scavenger = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavBody");
            GameObject brother = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody");
            GameObject drone1 = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Drone1Body");
            GameObject drone2 = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Drone2Body");
            GameObject turret1 = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Turret1Body");
            GameObject missiledrone = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MissileDroneBody");
            GameObject flamedrone = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FlameDroneBody");
            GameObject backupdrone = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BackupDroneBody");
            GameObject emergencydrone = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EmergencyDroneBody");
            GameObject equipmentdrone = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EquipmentDroneBody");
            GameObject megadrone = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MegaDroneBody");
            GameObject engiturret = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody");
            GameObject engiwalkerturret= LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody");
            GameObject squidturret = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SquidTurretBody");
            GameObject urchinturret = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/UrchinTurretBody");
            GameObject beetle = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleBody");
            GameObject beetleguard = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleGuardBody");
            GameObject acidlarva = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AcidLarvaBody");
            GameObject lemurian = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LemurianBody");
            GameObject lemurianbruiser = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LemurianBruiserBody");
            GameObject flyingvermin = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FlyingVerminBody");
            GameObject vermin = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/VerminBody");
            GameObject wisp = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/WispBody");
            GameObject greaterwisp = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GreaterWispBody");
            GameObject imp = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ImpBody");
            GameObject jellyfish = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/JellyfishBody");
            GameObject bison = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BisonBody");
            GameObject claybruiser = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody");
            GameObject claygrenadier = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayGrenadierBody");
            GameObject vulture = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/VultureBody");
            GameObject roboballmini = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallMiniBody");
            GameObject roboballgreen = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallGreenBuddyBody");
            GameObject roboballred = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallRedBuddyBody");
            GameObject bellbody = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BellBody");
            GameObject alphaconstruct2 = PrefabAPI.InstantiateClone(alphaconstruct, "1alphaconstruct");
            GameObject minimushroom = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MiniMushroomBody");
            GameObject gip2 = PrefabAPI.InstantiateClone(gip, "1gip");
            GameObject geep2 = PrefabAPI.InstantiateClone(geep, "1geep");
            GameObject gup2 = PrefabAPI.InstantiateClone(gup, "1gup");
            GameObject hermitcrab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HermitCrabBody");
            GameObject voidinfestor2 = PrefabAPI.InstantiateClone(voidinfestor, "1voidinfestor");
            GameObject voidbarnacle2 = PrefabAPI.InstantiateClone(voidbarnacle, "1voidbarnacle");
            GameObject nullifier = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/NullifierBody");
            GameObject voidjailer2 = PrefabAPI.InstantiateClone(voidjailer, "PCVoidJailerBody");
            GameObject voidmegacrab2 = PrefabAPI.InstantiateClone(voidmegacrab, "1voidmegacrab");
            GameObject lunarexploder = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody");
            GameObject lunargolem = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarGolemBody");
            GameObject lunarwisp = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarWispBody");
            GameObject droneman2 = PrefabAPI.InstantiateClone(droneman, "1droneman");
            GameObject voidcrabphase12 = PrefabAPI.InstantiateClone(voidcrabphase1, "1voidcrabphase1"); 
            GameObject voidcrabphase22 = PrefabAPI.InstantiateClone(voidcrabphase2, "1voidcrabphase2");
            GameObject voidcrabphase32 = PrefabAPI.InstantiateClone(voidcrabphase3, "1voidcrabphase3"); 
            //GameObject shopkeeper2 = PrefabAPI.InstantiateClone(shopkeeper, "1shopkeeper");

            //GameObject voidcrabphase12 = LegacyResourcesAPI.Load<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase1.prefab");
            //GameObject voidcrabphase22 = LegacyResourcesAPI.Load<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase2.prefab");
            //GameObject voidcrabphase32 = LegacyResourcesAPI.Load<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase3.prefab")
            //GameObject VoidRaidCrabrOrig = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase2.prefab").WaitForCompletion();

            //GameObject VoidRaidCrabPlayer = VoidRaidCrabrOrig.InstantiateClone("MyVoidRaidCrabBodyPhase2");

            //EquipmentSlot equipmentSlot = VoidRaidCrabPlayer.GetComponent<EquipmentSlot>();

            //if (!equipmentSlot)
            //{
            //    equipmentSlot = VoidRaidCrabPlayer.AddComponent<EquipmentSlot>();
            //}
            //EquipmentSlot exists = beetlequeen.GetComponent<EquipmentSlot>();
            //bool flag3 = !exists;
            //if (flag3)
            //{
            //    exists = beetlequeen.AddComponent<EquipmentSlot>();
            //}
            PolishMonsterToSurvivor(beetle, 5f);
            PolishMonsterToSurvivor(beetleguard, 5f);
            PolishMonsterToSurvivor(acidlarva, 5f);
            PolishMonsterToSurvivor(lemurian, 5f);
            PolishMonsterToSurvivor(lemurianbruiser, 5f);
            PolishMonsterToSurvivor(flyingvermin, 5f);
            PolishMonsterToSurvivor(vermin, 3f);
            PolishMonsterToSurvivor(wisp, 5f);
            PolishMonsterToSurvivor(greaterwisp, 5f);
            PolishMonsterToSurvivor(imp, 5f);
            PolishMonsterToSurvivor(jellyfish, 5f);
            PolishMonsterToSurvivor(bison, 5f);
            PolishMonsterToSurvivor(claybruiser, 5f);
            PolishMonsterToSurvivor(claygrenadier, 5f);
            PolishMonsterToSurvivor(vulture, 5f);
            PolishMonsterToSurvivor(roboballmini, 5f);
            PolishMonsterToSurvivor(roboballgreen, 5f);
            PolishMonsterToSurvivor(roboballred, 5f);
            PolishMonsterToSurvivor(bellbody, 5f);
            PolishMonsterToSurvivor(alphaconstruct2, 5f);
            PolishMonsterToSurvivor(minimushroom, 5f);
            PolishMonsterToSurvivor(gip2, 10f);
            PolishMonsterToSurvivor(geep2, 10f);
            PolishMonsterToSurvivor(gup2, 10f);
            PolishMonsterToSurvivor(hermitcrab, 10f);
            PolishMonsterToSurvivor(voidinfestor2, 10f);
            PolishMonsterToSurvivor(voidbarnacle2, 10f);
            PolishMonsterToSurvivor(nullifier, 10f);
            PolishMonsterToSurvivor(voidjailer2, 10f);
            PolishMonsterToSurvivor(voidmegacrab2, 10f);
            PolishMonsterToSurvivor(lunarexploder, 10f);
            PolishMonsterToSurvivor(lunargolem, 10f);
            PolishMonsterToSurvivor(lunarwisp, 10f);
            PolishMonsterToSurvivor(beetlequeen, 10f);
            PolishMonsterToSurvivor(golem, 10f);
            PolishMonsterToSurvivor(titan, 20f);
            PolishMonsterToSurvivor(titangold, 20f);
            PolishMonsterToSurvivor(gravekeeper, 20f);
            PolishMonsterToSurvivor(vagrant, 20f);
            PolishMonsterToSurvivor(magmaworm, 20f);
            PolishMonsterToSurvivor(overloadingworm, 20f);
            PolishMonsterToSurvivor(claydunestrider, 20f);
            PolishMonsterToSurvivor(roboballboss, 10f);
            PolishMonsterToSurvivor(superroboballboss, 20f);
            PolishMonsterToSurvivor(xiconstruct2, 20f);
            PolishMonsterToSurvivor(grandparent, 10f);
            PolishMonsterToSurvivor(scavenger, 20f);
            PolishMonsterToSurvivor(brother, 20f);
            PolishMonsterToSurvivor(drone1, 5f);
            PolishMonsterToSurvivor(drone2, 5f);
            PolishMonsterToSurvivor(turret1, 5f);
            PolishMonsterToSurvivor(missiledrone, 5f);
            PolishMonsterToSurvivor(flamedrone, 5f);
            PolishMonsterToSurvivor(backupdrone, 5f);
            PolishMonsterToSurvivor(emergencydrone, 5f);
            PolishMonsterToSurvivor(equipmentdrone, 5f);
            PolishMonsterToSurvivor(megadrone, 20f);
            PolishMonsterToSurvivor(engiturret, 20f);
            PolishMonsterToSurvivor(engiwalkerturret, 20f);
            PolishMonsterToSurvivor(squidturret, 20f);
            PolishMonsterToSurvivor(urchinturret, 20f);
            PolishMonsterToSurvivor(droneman2, 20f);
            PolishMonsterToSurvivor(voidcrabphase12, 20f);
            PolishMonsterToSurvivor(voidcrabphase22, 20f);
            PolishMonsterToSurvivor(voidcrabphase32, 20f);
            PolishMonsterToSurvivor(shopkeeper, 20f);
        }

        private void PolishMonsterToSurvivor(GameObject monsterSurvivor, float maxInteractionDistance)
        {
            NetworkIdentity networkIdentity = monsterSurvivor.GetComponent<NetworkIdentity>();
            bool flag = !networkIdentity;
            if (flag)
            {
                base.Logger.LogMessage("Missing NetworkIdentity! Adding...");
                networkIdentity = monsterSurvivor.AddComponent<NetworkIdentity>();
            }
            bool flag2 = !networkIdentity.localPlayerAuthority;
            if (flag2)
            {
                base.Logger.LogMessage("Ensuring Networking");
                networkIdentity.localPlayerAuthority = true;
            }
            //DeathRewards component = monsterSurvivor.GetComponent<DeathRewards>();
            //bool flag3 = component;
            //if (flag3)
            //{
            //    Object.Destroy(component);
            //}
            //InteractionDriver exists = monsterSurvivor.GetComponent<InteractionDriver>();
            //bool flag4 = !exists;
            //if (flag4)
            //{
            //    exists = monsterSurvivor.AddComponent<InteractionDriver>();
            //}
            Interactor interactor = monsterSurvivor.GetComponent<Interactor>();
            bool flag5 = !interactor;
            if (flag5)
            {
                interactor = monsterSurvivor.AddComponent<Interactor>();
            }
            interactor.maxInteractionDistance = maxInteractionDistance;
            EquipmentSlot exists2 = monsterSurvivor.GetComponent<EquipmentSlot>();
            bool flag6 = !exists2;
            if (flag6)
            {
                exists2 = monsterSurvivor.AddComponent<EquipmentSlot>();
            }
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
                int buffnumber = report.attackerBody.GetBuffCount(Modules.Buffs.shellbellBuff);
                if (buffnumber > 0)
                {
                    if (buffnumber >= 1 && buffnumber < 2)
                    {
                        CharacterBody attackerBody = report.attackerBody;
                        attackerBody.healthComponent.Heal(report.damageDealt * Modules.StaticValues.shellbelllifesteal, default(ProcChainMask), true);
                    }
                    if (buffnumber >= 2)
                    {
                        CharacterBody attackerBody = report.attackerBody;
                        attackerBody.healthComponent.Heal(report.damageDealt * Modules.StaticValues.shellbelllifesteal2, default(ProcChainMask), true);
                    }
                }

            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self.body.HasBuff(Modules.Buffs.rockyhelmetBuff.buffIndex))
            {
                int buffnumber = self.body.GetBuffCount(Modules.Buffs.rockyhelmetBuff);
                if (buffnumber > 0)
                {
                    if (buffnumber >= 1 && buffnumber < 2)
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
                    if (buffnumber >= 2)
                    {
                        if (buffnumber >= 1 && buffnumber < 2)
                        {

                            var damageInfo2 = new DamageInfo();

                            damageInfo2.damage = self.body.damage * Modules.StaticValues.rockyhelmetreflect2;
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
                    }
                }

            }
            orig.Invoke(self, damageInfo);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            //buffs 
            orig.Invoke(self);

            
            if (self.HasBuff(Modules.Buffs.choicebandBuff))
            {
                int buffnumber = self.GetBuffCount(Modules.Buffs.choicebandBuff);
                if(buffnumber > 0)
                {
                    if(buffnumber >= 1 && buffnumber <2)
                    {
                        self.attackSpeed *= Modules.StaticValues.choicebandboost;
                    }
                    if(buffnumber >= 2)
                    {
                        self.attackSpeed *= Modules.StaticValues.choicebandboost2;
                    }
                }
            }
            if (self.HasBuff(Modules.Buffs.choicescarfBuff))
            {
                int buffnumber = self.GetBuffCount(Modules.Buffs.choicescarfBuff);
                if (buffnumber > 0)
                {
                    if (buffnumber >= 1 && buffnumber < 2)
                    {
                        self.moveSpeed *= Modules.StaticValues.choicescarfboost;
                    }
                    if (buffnumber >= 2)
                    {
                        self.moveSpeed *= Modules.StaticValues.choicescarfboost2;
                    }
                }
            }
            if (self.HasBuff(Modules.Buffs.choicespecsBuff))
            {
                int buffnumber = self.GetBuffCount(Modules.Buffs.choicespecsBuff);
                if (buffnumber > 0)
                {
                    if (buffnumber >= 1 && buffnumber < 2)
                    {
                        self.damage *= Modules.StaticValues.choicespecsboost;
                    }
                    if (buffnumber >= 2)
                    {
                        self.damage *= Modules.StaticValues.choicespecsboost2;
                    }
                }
            }
            if (self.HasBuff(Modules.Buffs.scopelensBuff))
            {
                int buffnumber = self.GetBuffCount(Modules.Buffs.scopelensBuff);
                if (buffnumber > 0)
                {
                    if (buffnumber >= 1 && buffnumber < 2)
                    {
                        self.crit += Modules.StaticValues.scopelensboost;
                    }
                    if (buffnumber >= 2)
                    {
                        self.crit += Modules.StaticValues.scopelensboost2;
                    }
                }
            }
            if (self.HasBuff(Modules.Buffs.leftoversBuff))
            {
                int buffnumber = self.GetBuffCount(Modules.Buffs.leftoversBuff);
                if (buffnumber > 0)
                {
                    if (buffnumber >= 1 && buffnumber < 2)
                    {

                        HealthComponent hp = self.healthComponent;
                        float regenValue = hp.fullCombinedHealth * Modules.StaticValues.leftoversregen;
                        self.regen += regenValue;
                        //Chat.AddMessage("hpregen activated");
                    }
                    if (buffnumber >= 2)
                    {

                        HealthComponent hp = self.healthComponent;
                        float regenValue = hp.fullCombinedHealth * Modules.StaticValues.leftoversregen2;
                        self.regen += regenValue;
                        //Chat.AddMessage("hpregen activated");
                    }
                }
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
                    self.inventory.GiveEquipmentString("EQUIPMENT_TM_TRANSFORM");
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
                //self.RemoveBuff(DittoMod.Modules.Assets.fireelitebuff);
                //self.RemoveBuff(DittoMod.Modules.Assets.iceelitebuff);
                //self.RemoveBuff(DittoMod.Modules.Assets.hauntedelitebuff);
                //self.RemoveBuff(DittoMod.Modules.Assets.lightningelitebuff);
                //self.RemoveBuff(DittoMod.Modules.Assets.mendingelitebuff);
                //self.RemoveBuff(DittoMod.Modules.Assets.malachiteelitebuff);
                ////self.RemoveBuff(DittoMod.Modules.Assets.speedelitebuff);
                //self.RemoveBuff(DittoMod.Modules.Assets.voidelitebuff);
                //self.RemoveBuff(DittoMod.Modules.Assets.lunarelitebuff);
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