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
    public class TransformDitto : BaseSkillState
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

            var oldHealth = characterBody.healthComponent.health / characterBody.healthComponent.fullHealth;

            AkSoundEngine.PostEvent(1719197672, this.gameObject);
            CharacterMaster characterMaster = base.characterBody.master;
            if (characterMaster.bodyPrefab.name == "CaptainBody")
            {
                characterMaster.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
            }
            if (characterMaster.bodyPrefab.name == "HereticBody")
            {
                characterMaster.inventory.RemoveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                characterMaster.inventory.RemoveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                characterMaster.inventory.RemoveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                characterMaster.inventory.RemoveItem(RoR2Content.Items.LunarUtilityReplacement, 1);
            }

            if (characterMaster.bodyPrefab.name != "DittoBody")
            {
                characterMaster.TransformBody("DittoBody");

                CharacterBody body = characterMaster.GetBody();

                dittomastercon.transformed = false;
                dittomastercon.assaultvest = false;
                dittomastercon.choiceband = false;
                dittomastercon.choicescarf = false;
                dittomastercon.choicespecs = false;
                dittomastercon.leftovers = false;
                dittomastercon.lifeorb = false;
                dittomastercon.luckyegg = false;
                dittomastercon.rockyhelmet = false;
                dittomastercon.scopelens = false;
                dittomastercon.shellbell = false;
                dittomastercon.assaultvest2 = false;
                dittomastercon.choiceband2 = false;
                dittomastercon.choicescarf2 = false;
                dittomastercon.choicespecs2 = false;
                dittomastercon.leftovers2 = false;
                dittomastercon.lifeorb2 = false;
                dittomastercon.luckyegg2 = false;
                dittomastercon.rockyhelmet2 = false;
                dittomastercon.scopelens2 = false;
                dittomastercon.shellbell2 = false;
                if (Config.copyHealth.Value)
                    body.healthComponent.health = body.healthComponent.fullHealth * oldHealth;

            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();


            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

    }
}