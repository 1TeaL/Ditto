﻿using DittoMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Orbs;
using static RoR2.BulletAttack;

namespace DittoMod.SkillStates
{
    public class Airforce : BaseSkillState
    {
        public static float procCoefficient = 0.5f;
        public static float baseDuration = 0.5f;
        public static float force = 300f;
        public static float recoil = 1f;
        public static float range = 200f;

        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/tracerhuntresssnipe"); 
        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public float fajin;
        protected DamageType damageType;
        public static int maxRicochetCount = 8;
        public static bool resetBouncedObjects = true;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Airforce.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(this.duration);
            //this.muzzleString = "LFinger";

            hasFired = false;

            //base.PlayCrossfade("LeftArm, Override", "FingerFlick","Attack.playbackRate",this.duration, this.fireTime);
            //base.PlayAnimation("LeftArm, Override", "FingerFlick", "Attack.playbackRate", this.duration);


        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            base.characterBody.AddSpreadBloom(1f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
            AkSoundEngine.PostEvent(1063047365, this.gameObject);



            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * Airforce.recoil, -2f * Airforce.recoil, -0.5f * Airforce.recoil, 0.5f * Airforce.recoil);


                
                damageType = DamageType.Generic;
                

                bool hasHit = false;
                Vector3 hitPoint = Vector3.zero;
                float hitDistance = 0f;
                HealthComponent hitHealthComponent = null;

                var bulletAttack = new BulletAttack
                {
                    bulletCount = (uint)(2U),
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = Modules.StaticValues.airforceDamageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = damageType,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = Airforce.range,
                    force = Airforce.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = procCoefficient,
                    radius = 0.5f * fajin,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    //tracerEffectPrefab = Modules.Projectiles.bulletTracer,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,

                };
                if (maxRicochetCount > 0)
                {
                    bulletAttack.hitCallback = delegate (BulletAttack bulletAttackRef, ref BulletHit hitInfo)
                    {
                        var result = BulletAttack.defaultHitCallback(bulletAttackRef, ref hitInfo);
                        if (hitInfo.hitHurtBox)
                        hasHit = true;
                        hitPoint = hitInfo.point;
                        hitDistance = hitInfo.distance;
                        {
                            hitHealthComponent = hitInfo.hitHurtBox.healthComponent;
                        }
                        return result;
                    };
                }
                bulletAttack.filterCallback = delegate (BulletAttack bulletAttackRef, ref BulletAttack.BulletHit info)
                {
                    return (!info.entityObject || info.entityObject != bulletAttack.owner) && BulletAttack.defaultFilterCallback(bulletAttackRef, ref info);
                };
                bulletAttack.Fire();
                

            }
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate(); 

            if (base.fixedAge >= this.fireTime && !this.hasFired)
            {
                
                
                hasFired = true;
                this.Fire();
                
            }


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