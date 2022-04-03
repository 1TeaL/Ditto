using DittoMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Orbs;
using static RoR2.BulletAttack;
using UnityEngine.Networking;

namespace DittoMod.SkillStates
{
    public class Transform : BaseSkillState
    {
 
        private float duration = 1f;
        private float fireTime = 0.2f;
        private bool hasFired;
        private DittoController dittoTracker;
        private HurtBox Target;

        public override void OnEnter()
        {
            base.OnEnter();

            dittoTracker = base.GetComponent<DittoController>();
            if (dittoTracker && base.isAuthority)
            {
                Target = dittoTracker.GetTrackingTarget();
            }

            if (!Target)
            {
                return;
            }
            hasFired = false;


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

            CharacterMaster master = player.master;
            CharacterBody oldBody = master.GetBody();

            master.bodyPrefab = BodyCatalog.FindBodyPrefab(BodyCatalog.GetBodyName(hurtBox.healthComponent.body.bodyIndex));


            CharacterBody newcharBody = master.GetBody();
            

            

            CharacterBody body;
            
            
            if (BodyCatalog.GetBodyName(oldBody.bodyIndex) == "CaptainBody")
            {
                master.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
            }

            if (master.bodyPrefab.name == "CaptainBody")
            {
                master.inventory.GiveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
            }

            if (BodyCatalog.GetBodyName(oldBody.bodyIndex) == "HereticBody")
            {
                master.inventory.RemoveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                master.inventory.RemoveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                master.inventory.RemoveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                master.inventory.RemoveItem(RoR2Content.Items.LunarUtilityReplacement, 1);
            }

            if (master.bodyPrefab.name != "HereticBody")
            {
                body = master.Respawn(master.GetBody().transform.position, master.GetBody().transform.rotation);
                newcharBody.baseMaxHealth = oldBody.baseMaxHealth;
                newcharBody.regen = oldBody.regen;
                newcharBody.baseJumpCount = oldBody.baseJumpCount;
                newcharBody.baseJumpPower = oldBody.baseJumpPower;
                newcharBody.armor = oldBody.armor;
                newcharBody.baseMoveSpeed = oldBody.baseMoveSpeed;

                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.fireelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.fireelitebuff);
                }
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.iceelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.iceelitebuff);
                }
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.hauntedelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.hauntedelitebuff);
                }
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.lightningelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.lightningelitebuff);
                }
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.mendingelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.mendingelitebuff);
                }
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.malachiteelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.malachiteelitebuff);
                }
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.speedelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.speedelitebuff);
                }
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.voidelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.voidelitebuff);
                }
                if (hurtBox.healthComponent.body.HasBuff(DittoMod.Modules.Assets.lunarelitebuff))
                {
                    body.AddBuff(DittoMod.Modules.Assets.lunarelitebuff);
                }
            }
            else
            {
                if (master.bodyPrefab.name == "HereticBody")
                {
                    master.inventory.GiveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                    master.inventory.GiveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                    master.inventory.GiveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                    master.inventory.GiveItem(RoR2Content.Items.LunarUtilityReplacement, 1);
                }

                body = master.GetBody();
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