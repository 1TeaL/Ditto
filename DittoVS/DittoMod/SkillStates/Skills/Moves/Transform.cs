using DittoMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using DittoMod.Modules;
using UnityEngine.Networking;
using RoR2.ExpansionManagement;
using R2API.Networking;
using DittoMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace DittoMod.SkillStates
{
    public class Transform : BaseSkillState
    {

        private float duration = 1f;
        private float fireTime = 0.2f;
        private bool hasFired;
        public DittoController dittocon;
        public DittoMasterController dittomastercon;
        public HurtBox Target;

        public override void OnEnter()
        {
            base.OnEnter();


            dittocon = base.GetComponent<DittoController>();
            dittomastercon = characterBody.master.gameObject.GetComponent<DittoMasterController>();
            if (dittocon && base.isAuthority)
            {
                Target = dittocon.GetTrackingTarget();
            }

            if (!Target)
            {
                return;
            }
            hasFired = false;

            //PlayAnimation("Body", "BonusJump", "Attack.playbackRate", duration / 2);


        }

        public override void OnExit()
        {
            base.OnExit();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime && !this.hasFired)
            {
                hasFired = true;
                if (Target)
                {

                    Debug.Log("Target");
                    Debug.Log(BodyCatalog.FindBodyPrefab(BodyCatalog.GetBodyName(Target.healthComponent.body.bodyIndex)));
                    AkSoundEngine.PostEvent(1719197672, this.gameObject);
                    ChangeOrSetCharacter(characterBody.master.playerCharacterMasterController.networkUser, Target);


                    return;
                }
            }


            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public void dropEquipment(EquipmentDef def)
        {
            //PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(def.equipmentIndex), base.transform.position + Vector3.up * 1.5f, Vector3.up * 20f + base.transform.forward * 2f);

            if (base.isAuthority)
            {
                new EquipmentDropNetworked(PickupCatalog.FindPickupIndex(def.equipmentIndex),
                    base.transform.position + Vector3.up * 1.5f,
                    Vector3.up * 20f + base.transform.forward * 2f).Send(NetworkDestination.Clients);
            }
        }

        private void ChangeOrSetCharacter(NetworkUser player, HurtBox hurtBox)
        {

            CharacterMaster master = player.master;

            CharacterBody oldBody = master.GetBody();

            var oldHealth = oldBody.healthComponent.health/oldBody.healthComponent.fullHealth;

            var name = BodyCatalog.GetBodyName(hurtBox.healthComponent.body.bodyIndex);
            GameObject newbodyPrefab = BodyCatalog.FindBodyPrefab(name);

            var targetMaster = hurtBox.healthComponent.body.master;

            var survivorDef = SurvivorCatalog.FindSurvivorDefFromBody(newbodyPrefab);
            if (survivorDef && !survivorDef.CheckUserHasRequiredEntitlement(master.playerCharacterMasterController.networkUser))
                return;
            
            var requirementComponent = newbodyPrefab.GetComponent<ExpansionRequirementComponent>();
            if (requirementComponent && !requirementComponent.PlayerCanUseBody(master.playerCharacterMasterController))
                return;
            
            CharacterBody body;


            if (!Modules.StaticValues.blacklist.Contains(newbodyPrefab.name))
            {

                master.bodyPrefab = newbodyPrefab;

                EntityStateMachine EntityStateMachine = newbodyPrefab.GetComponent<EntityStateMachine>();
                EntityStateMachine.initialStateType = new SerializableEntityStateType(typeof(SkillStates.BaseStates.SpawnState));

                master.TransformBody(BodyCatalog.GetBodyName(hurtBox.healthComponent.body.bodyIndex));

                master.bodyPrefab = BodyCatalog.FindBodyPrefab(newbodyPrefab.name);

                body = master.GetBody();

                if (Config.copyLoadout.Value)
                    body.SetLoadoutServer(targetMaster.loadout);



                if (Config.copyHealth.Value)
                    body.healthComponent.health = oldHealth * body.healthComponent.fullHealth;

                RigidbodyMotor rigid = body.gameObject.GetComponent<RigidbodyMotor>();

                EquipmentSlot exists2 = body.gameObject.GetComponent<EquipmentSlot>();
                bool flag6 = !exists2;
                if (flag6)
                {
                    exists2 = body.gameObject.AddComponent<EquipmentSlot>();
                }
                if (body.name == "CaptainBody")
                {
                    master.inventory.GiveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
                }

                if (body.name == "HereticBody")
                {
                    master.inventory.GiveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                    master.inventory.GiveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                    master.inventory.GiveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                    master.inventory.GiveItem(RoR2Content.Items.LunarUtilityReplacement, 1);

                }

                if (Modules.StaticValues.speciallist.Contains(newbodyPrefab.name))
                {
                    dittomastercon.transformed = true;
                    //body.ApplyBuff(Modules.Buffs.transformBuff.buffIndex, Modules.StaticValues.transformDuration);
                }


                if (rigid)
                {
                    rigid.characterBody.moveSpeed = oldBody.moveSpeed * Config.movespeedMultiplier.Value;
                }
                float damage = body.damage * Config.damageMultiplier.Value;
                float basedamage = body.baseDamage * Config.damageMultiplier.Value;
                float leveldamage = body.levelDamage * Config.damageMultiplier.Value;
                float attackspeed = body.attackSpeed* Config.attackspeedMultiplier.Value;
                float baseattackspeed = body.baseAttackSpeed * Config.attackspeedMultiplier.Value;
                float levelattackspeed = body.levelAttackSpeed * Config.attackspeedMultiplier.Value;

                body.baseMaxHealth = oldBody.baseMaxHealth + body.baseMaxHealth / 5;
                body.levelMaxHealth = oldBody.levelMaxHealth + body.levelMaxHealth / 5;
                body.maxHealth = oldBody.maxHealth + body.maxHealth / 5;
                body.baseRegen = oldBody.regen;
                body.baseJumpCount = oldBody.baseJumpCount;
                body.maxJumpCount = oldBody.maxJumpCount;
                body.maxJumpHeight = oldBody.maxJumpHeight;
                body.jumpPower = oldBody.jumpPower;
                body.baseJumpPower = oldBody.baseJumpPower;
                body.damage = damage;
                body.baseDamage = basedamage;
                body.levelDamage = leveldamage;
                body.attackSpeed = attackspeed;
                body.baseAttackSpeed = baseattackspeed;
                body.levelAttackSpeed = levelattackspeed;
                if (body.characterMotor)
                {
                    body.characterMotor.mass = oldBody.characterMotor.mass;
                    body.characterMotor.jumpCount = oldBody.characterMotor.jumpCount;
                }
                body.baseArmor = oldBody.baseArmor;
                body.armor = oldBody.armor;
                body.baseMoveSpeed = oldBody.baseMoveSpeed * Config.movespeedMultiplier.Value;
                body.moveSpeed = oldBody.moveSpeed * Config.movespeedMultiplier.Value;
                body.levelMoveSpeed = oldBody.levelMoveSpeed * Config.movespeedMultiplier.Value;

                body.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, Modules.StaticValues.invincibilityDuration);
                //body.ApplyBuff(Modules.Buffs.transformdeBuff.buffIndex, 1, Modules.StaticValues.transformDuration);
                body.AddTimedBuffAuthority(Buffs.transformdeBuff.buffIndex, StaticValues.transformDuration);

                if (targetMaster.playerCharacterMasterController || !targetMaster.playerCharacterMasterController)
                {
                    if (Config.choiceOnTeammate.Value)
                    {

                    }
                    
                    if (!Config.choiceOnTeammate.Value)
                    {
                        body.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.luckyeggBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 0);
                        body.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 0);

                    }
                }

                //Debug.Log(hurtBox.healthComponent.body.activeBuffsList + "buffs");

                if(Config.grabAspect.Value == true)
                {
                    if (hurtBox.healthComponent.body.isElite)
                    {
                        if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixBlue))
                        {
                            dropEquipment(RoR2Content.Elites.Lightning.eliteEquipmentDef);
                        }
                        if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixHaunted))
                        {
                            dropEquipment(RoR2Content.Elites.Haunted.eliteEquipmentDef);
                        }
                        if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixLunar))
                        {
                            dropEquipment(RoR2Content.Elites.Lunar.eliteEquipmentDef);
                        }
                        if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixPoison))
                        {
                            dropEquipment(RoR2Content.Elites.Poison.eliteEquipmentDef);
                        }
                        if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixRed))
                        {
                            dropEquipment(RoR2Content.Elites.Fire.eliteEquipmentDef);
                        }
                        if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixWhite))
                        {
                            dropEquipment(RoR2Content.Elites.Ice.eliteEquipmentDef);
                        }
                        if (hurtBox.healthComponent.body.HasBuff(DLC1Content.Buffs.EliteEarth))
                        {
                            dropEquipment(DLC1Content.Elites.Earth.eliteEquipmentDef);
                        }
                        if (hurtBox.healthComponent.body.HasBuff(DLC1Content.Buffs.EliteVoid))
                        {
                            dropEquipment(DLC1Content.Elites.Void.eliteEquipmentDef);
                        }
                    }

                }


            }
            else
            {
                dittomastercon.transformed = false;
                Chat.AddMessage("Ditto's <style=cIsUtility>Transform failed!</style>");
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.Target));
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            this.Target = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}