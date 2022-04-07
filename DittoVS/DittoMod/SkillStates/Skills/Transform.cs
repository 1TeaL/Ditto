using DittoMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Orbs;
using static RoR2.BulletAttack;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using R2API;

namespace DittoMod.SkillStates
{
    public class Transform : BaseSkillState
    {
 
        private float duration = 1f;
        private float fireTime = 0.2f;
        private bool hasFired;
        //private DittoMasterController dittocon;
        public DittoController dittocon;
        public HurtBox Target;

        public override void OnEnter()
        {
            base.OnEnter();

            AkSoundEngine.PostEvent(1719197672, this.gameObject);

            dittocon = base.GetComponent<DittoController>();
            if (dittocon && base.isAuthority)
            {
                Target = dittocon.GetTrackingTarget();
            }

            if (!Target)
            {
                return;
            }
            hasFired = false;

            PlayAnimation("Body", "BonusJump", "Attack.playbackRate", duration/2);


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
            blacklist.Add("MiniVoidRaidCrabBodyPhase1");
            blacklist.Add("MiniVoidRaidCrabBodyPhase2");
            blacklist.Add("MiniVoidRaidCrabBodyPhase3");
            blacklist.Add("DittoBody");
            blacklist.Add("MinorConstructBody");
            blacklist.Add("MinorConstructOnKillBody");
            blacklist.Add("ShopkeeperBody");
            blacklist.Add("VoidJailerBody");
            blacklist.Add("NullifierBody");

            CharacterMaster master = player.master;
            CharacterBody oldBody = master.GetBody();


            GameObject newbodyPrefab = BodyCatalog.FindBodyPrefab(BodyCatalog.GetBodyName(hurtBox.healthComponent.body.bodyIndex));
            

            CharacterBody body;


            if (!blacklist.Contains(newbodyPrefab.name))
            {

                master.bodyPrefab = newbodyPrefab;
                //newbodyPrefab.
                
                body = master.Respawn(master.GetBody().transform.position, master.GetBody().transform.rotation);
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
                    

                if (rigid)
                {
                    rigid.characterBody.moveSpeed = oldBody.moveSpeed;
                }

                body.baseMaxHealth = oldBody.baseMaxHealth + body.baseMaxHealth/10;
                body.levelMaxHealth = oldBody.levelMaxHealth + body.levelMaxHealth / 10;
                body.maxHealth = oldBody.maxHealth + body.maxHealth / 10 ;
                body.baseRegen = oldBody.regen;
                body.baseJumpCount = oldBody.baseJumpCount;
                body.maxJumpCount = oldBody.maxJumpCount;
                body.maxJumpHeight = oldBody.maxJumpHeight;
                body.jumpPower = oldBody.jumpPower;
                body.baseJumpPower = oldBody.baseJumpPower;
                if (body.characterMotor)
                {
                    body.characterMotor.mass = oldBody.characterMotor.mass;
                }
                body.baseArmor = oldBody.armor;
                body.baseMoveSpeed = oldBody.baseMoveSpeed;

                body.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, Modules.StaticValues.invincibilityDuration);

                if (dittocon.choiceband)
                {
                    body.AddBuff(Modules.Buffs.choicebandBuff);
                }
                if (dittocon.choicescarf)
                {
                    body.AddBuff(Modules.Buffs.choicescarfBuff);
                }
                if (dittocon.choicespecs)
                {
                    body.AddBuff(Modules.Buffs.choicespecsBuff);
                }
                if (dittocon.leftovers)
                {
                    body.AddBuff(Modules.Buffs.leftoversBuff);
                }
                if (dittocon.rockyhelmet)
                {
                    body.AddBuff(Modules.Buffs.rockyhelmetBuff);
                }
                if (dittocon.scopelens)
                {
                    body.AddBuff(Modules.Buffs.scopelensBuff);
                }
                if (dittocon.shellbell)
                {
                    body.AddBuff(Modules.Buffs.shellbellBuff);
                }
                if (dittocon.choiceband2)
                {
                    body.AddBuff(Modules.Buffs.choicebandBuff);
                }
                if (dittocon.choicescarf2)
                {
                    body.AddBuff(Modules.Buffs.choicescarfBuff);
                }
                if (dittocon.choicespecs2)
                {
                    body.AddBuff(Modules.Buffs.choicespecsBuff);
                }
                if (dittocon.leftovers2)
                {
                    body.AddBuff(Modules.Buffs.leftoversBuff);
                }
                if (dittocon.rockyhelmet2)
                {
                    body.AddBuff(Modules.Buffs.rockyhelmetBuff);
                }
                if (dittocon.scopelens2)
                {
                    body.AddBuff(Modules.Buffs.scopelensBuff);
                }
                if (dittocon.shellbell2)
                {
                    body.AddBuff(Modules.Buffs.shellbellBuff);
                }

                Debug.Log(hurtBox.healthComponent.body.activeBuffsList+"buffs");

                if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixBlue))
                {
                    body.AddBuff(RoR2Content.Buffs.AffixBlue);
                }
                if (hurtBox.healthComponent.body.HasBuff(RoR2Content.Buffs.AffixEcho))
                {
                    body.AddBuff(RoR2Content.Buffs.AffixEcho);
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
                //if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.fireelitebuff))
                //{
                //    body.AddBuff(DittoMod.Modules.Assets.fireelitebuff);
                //}
                //if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.iceelitebuff))
                //{
                //    body.AddBuff(DittoMod.Modules.Assets.iceelitebuff);
                //}
                //if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.hauntedelitebuff))
                //{
                //    body.AddBuff(DittoMod.Modules.Assets.hauntedelitebuff);
                //}
                //if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.lightningelitebuff))
                //{
                //    body.AddBuff(DittoMod.Modules.Assets.lightningelitebuff);
                //}
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.mendingelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.mendingelitebuff);
                }
                //if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.malachiteelitebuff))
                //{
                //    body.AddBuff(DittoMod.Modules.Assets.malachiteelitebuff);
                //}
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.voidelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.voidelitebuff);
                }
                //if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.lunarelitebuff))
                //{
                //    body.AddBuff(DittoMod.Modules.Assets.lunarelitebuff);
                //}
                //if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.speedelitebuff))
                //{
                //    body.AddBuff(DittoMod.Modules.Assets.speedelitebuff);
                //}            


            }
            else
            {
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