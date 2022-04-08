using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DittoMod.Modules.Survivors
{
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(TeamComponent))]
    [RequireComponent(typeof(InputBankTest))]
    public class DittoMasterController : MonoBehaviour
    {
        private CharacterMaster characterMaster;
        private CharacterBody body;
        public bool bosskilled;
        public bool transformed;    
        private int buffCountToApply;
        public float transformage;

        private void Awake()
        {
            bosskilled = false;
            transformed = false;
            On.RoR2.HealthComponent.TakeDamage += SwitchBack;
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
        }

        private void Start()
        {
            characterMaster = gameObject.GetComponent<CharacterMaster>();
            //Debug.Log(transformed + "istransformed");

        }
        private void SwitchBack(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            if (!self) { return; }
            if (self.health <= 0) { return; }
            if (self.fullCombinedHealth <= 10 && transformed)
            {
                transformed = false;


                if (self.body.master.bodyPrefab.name != "DittoBody")
                {

                    if (self.body.name == "CaptainBody")
                    {
                        self.body.master.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
                    }
                    if (self.body.master.bodyPrefab.name == "HereticBody")
                    {
                        self.body.master.inventory.RemoveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                        self.body.master.inventory.RemoveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                        self.body.master.inventory.RemoveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                        self.body.master.inventory.RemoveItem(RoR2Content.Items.LunarUtilityReplacement, 1);
                    }

                    self.body.master.bodyPrefab = BodyCatalog.FindBodyPrefab("DittoBody");
                    CharacterBody body;
                    body = self.body.master.Respawn(self.body.master.GetBody().transform.position, self.body.master.GetBody().transform.rotation);

                    body.RemoveBuff(RoR2Content.Buffs.OnFire);
                    body.RemoveBuff(RoR2Content.Buffs.AffixBlue);
                    body.RemoveBuff(RoR2Content.Buffs.AffixEcho);
                    body.RemoveBuff(RoR2Content.Buffs.AffixHaunted);
                    body.RemoveBuff(RoR2Content.Buffs.AffixLunar);
                    body.RemoveBuff(RoR2Content.Buffs.AffixPoison);
                    body.RemoveBuff(RoR2Content.Buffs.AffixRed);
                    body.RemoveBuff(RoR2Content.Buffs.AffixWhite);
                    body.RemoveBuff(DittoMod.Modules.Assets.mendingelitebuff);
                    body.RemoveBuff(DittoMod.Modules.Assets.voidelitebuff);

                }
            }
        }


        private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);


            if(self.hasEffectiveAuthority)
            {
                if (self.HasBuff(Modules.Buffs.transformBuff.buffIndex))
                {

                    if (transformage > 1f)
                    {
                        int buffnumber = self.GetBuffCount(Modules.Buffs.transformBuff.buffIndex);
                        if (buffnumber > 1)
                        {
                            if (buffnumber >= 2)
                            {
                                Debug.Log(bosskilled + "bosskilledfixedupdate");
                                self.SetBuffCount(Modules.Buffs.transformBuff.buffIndex, (buffnumber - 1));
                                if (!bosskilled)
                                {
                                    transformage = 0;
                                }

                            }
                        }
                        else
                        {

                            if (self.master.bodyPrefab.name == "CaptainBody")
                            {
                                self.master.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
                            }
                            if (self.master.bodyPrefab.name == "HereticBody")
                            {
                                self.master.inventory.RemoveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                                self.master.inventory.RemoveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                                self.master.inventory.RemoveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                                self.master.inventory.RemoveItem(RoR2Content.Items.LunarUtilityReplacement, 1);
                            }

                            self.master.bodyPrefab = BodyCatalog.FindBodyPrefab("DittoBody");
                            CharacterBody body;
                            body = self.master.Respawn(self.master.GetBody().transform.position, self.master.GetBody().transform.rotation);

                            body.RemoveBuff(RoR2Content.Buffs.OnFire);
                            body.RemoveBuff(RoR2Content.Buffs.AffixBlue);
                            body.RemoveBuff(RoR2Content.Buffs.AffixEcho);
                            body.RemoveBuff(RoR2Content.Buffs.AffixHaunted);
                            body.RemoveBuff(RoR2Content.Buffs.AffixLunar);
                            body.RemoveBuff(RoR2Content.Buffs.AffixPoison);
                            body.RemoveBuff(RoR2Content.Buffs.AffixRed);
                            body.RemoveBuff(RoR2Content.Buffs.AffixWhite);
                            body.RemoveBuff(DittoMod.Modules.Assets.mendingelitebuff);
                            body.RemoveBuff(DittoMod.Modules.Assets.voidelitebuff);

                        }
                    }

                    else transformage += Time.fixedDeltaTime;
                }
                else
                {
                    bosskilled = false;
                }

            }


        }



    }
}
