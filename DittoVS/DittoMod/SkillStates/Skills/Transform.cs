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
                    if(Target != default)
                    {
                        Debug.Log("Target");
                        if (Target.healthComponent.body)
                        {
                            Debug.Log("Target.gameobject");
                            ChangeOrSetCharacter(characterBody.master.playerCharacterMasterController.networkUser, Target.healthComponent.body.gameObject);
                        }
                    }
                    return;
                }
            }


            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void ChangeOrSetCharacter(NetworkUser player, GameObject bodyPrefab)
        {
            CharacterMaster master = player.master;
            CharacterBody oldBody = master.GetBody();

            master.bodyPrefab = bodyPrefab;
            CharacterBody body;
            
            
            if (BodyCatalog.GetBodyName(oldBody.bodyIndex) == "CaptainBody")
            {
                master.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
            }

            if (bodyPrefab.name == "CaptainBody")
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

            if (bodyPrefab.name != "HereticBody")
            {
                body = master.Respawn(master.GetBody().transform.position, master.GetBody().transform.rotation);
            }
            else
            {
                if (bodyPrefab.name == "HereticBody")
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