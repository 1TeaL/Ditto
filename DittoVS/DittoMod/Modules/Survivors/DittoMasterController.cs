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
        public bool transformed;
        private int buffCountToApply;
        public float transformage;

        private void Awake()
        {
            transformed = false;
            //On.RoR2.Stage.Start += Stage_Start;
            //On.RoR2.CharacterMaster.Respawn += CharacterMaster_Respawn;
            On.RoR2.CharacterBody.Start += CharacterBody_Start;
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
        }


        private void Start()
        {
            characterMaster = gameObject.GetComponent<CharacterMaster>();
            //Debug.Log(transformed + "istransformed");


        }

        private void CharacterBody_Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            orig(self);
            if (self.master.gameObject.GetComponent<DittoMasterController>())
            {
                if (self.master.bodyPrefab != BodyCatalog.FindBodyPrefab("DittoBody"))
                {
                    if (transformed)
                    {
                        self.SetBuffCount(Modules.Buffs.transformBuff.buffIndex, 1);
                    }
                }

            }

        }

        //private CharacterBody CharacterMaster_Respawn(On.RoR2.CharacterMaster.orig_Respawn orig, CharacterMaster self, Vector3 footPosition, Quaternion rotation)
        //{
        //    if (self.gameObject.GetComponent<DittoMasterController>())
        //    {
        //        if (self.bodyPrefab != BodyCatalog.FindBodyPrefab("DittoBody"))
        //        {
        //            self.GetBody().SetBuffCount(Modules.Buffs.transformBuff.buffIndex, 1);
        //        }

        //    }      

        //    return orig(self, footPosition, rotation);
        //}


        private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);

            if (self.hasEffectiveAuthority)
            {
                if (self.HasBuff(Modules.Buffs.transformBuff.buffIndex))
                {

                    if (transformage > 1f)
                    {
                        buffCountToApply = self.GetBuffCount(Modules.Buffs.transformBuff.buffIndex);
                        if (buffCountToApply > 1)
                        {
                            if (buffCountToApply >= 2)
                            {
                                self.SetBuffCount(Modules.Buffs.transformBuff.buffIndex, (buffCountToApply - 1));
                                
                                transformage = 0;
                                

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
                            transformed = false;

                        }
                    }

                    else transformage += Time.fixedDeltaTime;
                }                 


            }         


        }

    }
}
