using DittoMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using DittoMod.Modules;
using UnityEngine.Networking;
using RoR2.ExpansionManagement;

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

            PlayAnimation("Body", "BonusJump", "Attack.playbackRate", duration / 2);


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



        private void ChangeOrSetCharacter(NetworkUser player, HurtBox hurtBox)
        {
            List<string> blacklist = new List<string>();
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
            speciallist.Add("ScavBody");

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


            if (!blacklist.Contains(newbodyPrefab.name))
            {

                master.bodyPrefab = newbodyPrefab;

                //body = master.Respawn(master.GetBody().transform.position, master.GetBody().transform.rotation);

                master.TransformBody(BodyCatalog.GetBodyName(hurtBox.healthComponent.body.bodyIndex));

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

                if (speciallist.Contains(newbodyPrefab.name))
                {
                    dittomastercon.transformed = true;
                    //body.SetBuffCount(Modules.Buffs.transformBuff.buffIndex, Modules.StaticValues.transformDuration);
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

                body.baseMaxHealth = oldBody.baseMaxHealth + body.baseMaxHealth / 10;
                body.levelMaxHealth = oldBody.levelMaxHealth + body.levelMaxHealth / 10;
                body.maxHealth = oldBody.maxHealth + body.maxHealth / 10;
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
                }
                body.baseArmor = oldBody.baseArmor;
                body.armor = oldBody.armor;
                body.baseMoveSpeed = oldBody.baseMoveSpeed * Config.movespeedMultiplier.Value;
                body.moveSpeed = oldBody.moveSpeed * Config.movespeedMultiplier.Value;
                body.levelMoveSpeed = oldBody.levelMoveSpeed * Config.movespeedMultiplier.Value;

                body.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, Modules.StaticValues.invincibilityDuration);

                if (targetMaster.playerCharacterMasterController || !targetMaster.playerCharacterMasterController)
                {
                    if (Config.choiceOnTeammate.Value)
                    {

                    }
                    
                    if (!Config.choiceOnTeammate.Value)
                    {
                        body.SetBuffCount(Modules.Buffs.assaultvestBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.choicebandBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.choicescarfBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.choicespecsBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.leftoversBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.lifeorbBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.luckyeggBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.rockyhelmetBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.scopelensBuff.buffIndex, 0);
                        body.SetBuffCount(Modules.Buffs.shellbellBuff.buffIndex, 0);

                    }
                }

                //Debug.Log(hurtBox.healthComponent.body.activeBuffsList + "buffs");

                if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixBlue))
                {
                    body.AddBuff(RoR2Content.Buffs.AffixBlue);
                }
                if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixHaunted))
                {
                    body.AddBuff(RoR2Content.Buffs.AffixHaunted);
                }
                if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixLunar))
                {
                    body.AddBuff(RoR2Content.Buffs.AffixLunar);
                }
                if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixPoison))
                {
                    body.AddBuff(RoR2Content.Buffs.AffixPoison);
                }
                if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixRed))
                {
                    body.AddBuff(RoR2Content.Buffs.AffixRed);
                }
                if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixWhite))
                {
                    body.AddBuff(RoR2Content.Buffs.AffixWhite);
                }
                if (hurtBox.healthComponent.body.HasBuff(DLC1Content.Buffs.EliteEarth))
                {
                    body.AddBuff(DLC1Content.Buffs.EliteEarth);
                }
                if (hurtBox.healthComponent.body.HasBuff(DLC1Content.Buffs.EliteVoid))
                {
                    body.AddBuff(DLC1Content.Buffs.EliteVoid);
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