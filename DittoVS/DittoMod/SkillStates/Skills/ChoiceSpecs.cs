﻿using EntityStates;
using RoR2;
using UnityEngine;
using EntityStates;
using RoR2;
using UnityEngine;
using DittoMod.Modules.Survivors;

namespace DittoMod.SkillStates
{
    public class ChoiceSpecs : BaseSkillState
    {
        public float duration = 0.1f;
        public DittoController dittocon;

        public override void OnEnter()
        {
            base.OnEnter();

            dittocon = base.GetComponent<DittoController>();
            dittocon.choicespecs = true;
            characterBody.AddBuff(Modules.Buffs.choicespecsBuff);
            
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