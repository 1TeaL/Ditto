using R2API.Networking;
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
        public DittoController dittocon;
        public DittoMasterController dittomastercon;
        private CharacterMaster characterMaster;
        private CharacterBody body;
        public bool transformed;
        //private int buffCountToApply;
        public float transformage1;
        public bool assaultvest;
        public bool choiceband;
        public bool choicescarf;
        public bool choicespecs;
        public bool leftovers;
        public bool lifeorb;
        public bool luckyegg;
        public bool rockyhelmet;
        public bool scopelens;
        public bool shellbell;
        private bool givebuffs;

        public bool hookgiven;

        public void Awake()
        {
            transformed = false;
            assaultvest = false;
            choiceband = false;
            choicescarf = false;
            choicespecs = false;
            leftovers = false;
            lifeorb = false;
            luckyegg = false;
            rockyhelmet = false;
            scopelens = false;
            shellbell = false;
            //On.RoR2.Stage.Start += Stage_Start;
            //On.RoR2.CharacterMaster.Respawn += CharacterMaster_Respawn;
            if (!hookgiven)
            {
                On.RoR2.CharacterBody.Start += CharacterBody_Start;
                On.RoR2.Run.Start += Run_Start;
                //On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
                On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
                On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
                hookgiven = true;
            }
        }

        private void Run_Start(On.RoR2.Run.orig_Start orig, Run self)
        {
            orig.Invoke(self);
            transformed = false;
            assaultvest = false;
            choiceband = false;
            choicescarf = false;
            choicespecs = false;
            leftovers = false;
            lifeorb = false;
            luckyegg = false;
            rockyhelmet = false;
            scopelens = false;
            shellbell = false;

        }

        public void OnDestroy()
        {
            On.RoR2.CharacterBody.Start -= CharacterBody_Start;
            On.RoR2.Run.Start -= Run_Start;
            On.RoR2.CharacterMaster.OnInventoryChanged -= CharacterMaster_OnInventoryChanged;
            On.RoR2.CharacterModel.Awake -= CharacterModel_Awake;
        }


        public void Start()
        {
            transformed = false;
            assaultvest = false;
            choiceband = false;
            choicescarf = false;
            choicespecs = false;
            leftovers = false;
            lifeorb = false;
            luckyegg = false;
            rockyhelmet = false;
            scopelens = false;
            shellbell = false;
            characterMaster = gameObject.GetComponent<CharacterMaster>();
            //Debug.Log(transformed + "istransformed");
            body = characterMaster.GetBody();
            
            dittomastercon = characterMaster.gameObject.GetComponent<DittoMasterController>();
            dittocon = body.gameObject.GetComponent<DittoController>();


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

            characterMaster.luck = 0;
            if (characterMaster.GetBody().skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
            {
                luckyegg = true;
                body.AddBuff(Modules.Buffs.luckyeggBuff);
                characterMaster.luck += 1f;
            }


            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ASSAULTVEST_NAME" && !assaultvest)
            {
                assaultvest = true;
                body.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 1);
            }
            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband)
            {
                choiceband = true;
                body.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 1);
            }
            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf)
            {
                body.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 1);
            }
            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs)
            {
                choicespecs = true;
                body.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 1);
            }
            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers)
            {
                leftovers = true;
                body.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 1);
            }
            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LIFEORB_NAME" && !lifeorb)
            {
                lifeorb = true;
                body.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 1);
            }
            //if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
            //{
            //    luckyegg = true;
            //    body.AddBuff(Modules.Buffs.luckyeggBuff);
            //}
            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet)
            {
                rockyhelmet = true;
                body.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 1);
            }
            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens)
            {
                scopelens = true;
                body.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 1);
            }
            if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell)
            {
                shellbell = true;
                body.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 1);
            }
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ASSAULTVEST_NAME" && !assaultvest1)
            //{
            //    assaultvest1 = true;
            //    dittocon.assaultvest1 = true;
            //    body.AddBuff(Modules.Buffs.assaultvestBuff);
            //}
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband1)
            //{
            //    choiceband1 = true;
            //    dittocon.choiceband1 = true;
            //    body.AddBuff(Modules.Buffs.choicebandBuff);
            //}
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf1)
            //{
            //    choicescarf1 = true;
            //    dittocon.choicescarf1 = true;
            //    body.AddBuff(Modules.Buffs.choicescarfBuff);
            //}
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs1)
            //{
            //    choicespecs1 = true;
            //    dittocon.choicespecs1 = true;
            //    body.AddBuff(Modules.Buffs.choicespecsBuff);
            //}
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers1)
            //{
            //    leftovers1 = true;
            //    dittocon.leftovers1 = true;
            //    body.AddBuff(Modules.Buffs.leftoversBuff);
            //}
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LIFEORB_NAME" && !lifeorb1)
            //{
            //    lifeorb1 = true;
            //    dittocon.lifeorb1 = true;
            //    body.AddBuff(Modules.Buffs.lifeorbBuff);
            //}
            ////if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg1)
            ////{
            ////    luckyegg1 = true;
            ////    body.AddBuff(Modules.Buffs.luckyeggBuff);
            ////}
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet1)
            //{
            //    rockyhelmet1 = true;
            //    dittocon.rockyhelmet1 = true;
            //    body.AddBuff(Modules.Buffs.rockyhelmetBuff);
            //}
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens1)
            //{
            //    scopelens1 = true;
            //    dittocon.scopelens1 = true;
            //    body.AddBuff(Modules.Buffs.scopelensBuff);
            //}
            //if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell1)
            //{
            //    shellbell1 = true;
            //    dittocon.shellbell1 = true;
            //    body.AddBuff(Modules.Buffs.shellbellBuff);
            //}


        }



        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("DittoDisplay"))
            {

                transformed = false;
                assaultvest = false;
                choiceband = false;
                choicescarf = false;
                choicespecs = false;
                leftovers = false;
                lifeorb = false;
                luckyegg = false;
                rockyhelmet = false;
                scopelens = false;
                shellbell = false;

            }
            //Destroy(dittomastercon);


        }


        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig.Invoke(self);

            if (self)
            {
                if (characterMaster.netId == self.netId)
                {
                    self.luck = 0;
                    if (luckyegg)
                    {
                        self.luck += 1f;
                    }
                    self.luck += self.inventory.GetItemCount(RoR2Content.Items.Clover);
                    self.luck -= self.inventory.GetItemCount(RoR2Content.Items.LunarBadLuck);

                }
            }
            
        }

        public void CharacterBody_Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            orig.Invoke(self);



            if (self)
            {
                if (self.master)
                {
                    if(characterMaster.netId == self.master.netId)
                    {
                        if (self.master.bodyPrefab.name == BodyCatalog.FindBodyPrefab("DittoBody").name)
                        {
                            givebuffs = false;
                            
                            if (assaultvest)
                            {
                                self.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 1);
                            }
                            else
                            {
                                self.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 0);
                            }
                            if (choiceband)
                            {
                                self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 1);
                            }
                            else 
                            {
                                self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 0);
                            }
                            if (choicescarf)
                            {
                                self.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 1);
                            }
                            else
                            { 
                                self.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 0);
                            }
                            if (choicespecs)
                            {
                                self.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 1);
                            }
                            else 
                            {
                                self.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 0);
                            }
                            if (leftovers)
                            {
                                self.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 1);
                            }
                            else 
                            {
                                self.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 0);
                            }
                            if (lifeorb)
                            {
                                self.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 1);
                            }
                            else
                            {
                                self.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 0);
                            }
                            //if (luckyegg && luckyegg1)
                            //{
                            //    self.master.luck = 0;
                            //    self.ApplyBuff(Modules.Buffs.luckyeggBuff.buffIndex, 1);
                            //    self.master.luck += 1f;
                            //}
                            //else if (luckyegg | luckyegg1)
                            //{
                            //    self.master.luck = 0;
                            //    self.ApplyBuff(Modules.Buffs.luckyeggBuff.buffIndex, 1);
                            //    self.master.luck += 1f;
                            //}
                            if (rockyhelmet)
                            {
                                self.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 1);
                            }
                            else 
                            {
                                self.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 0);
                            }
                            if (scopelens)
                            {
                                self.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 1);
                            }
                            else 
                            {
                                self.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 0);
                            }
                            if (shellbell)
                            {
                                self.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 1);
                            }
                            else 
                            {
                                self.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 0);
                            }


                        }
                        else if (self.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name)
                        {
                            if (Config.bossTimer.Value)
                            {
                                if (Modules.StaticValues.speciallist.Contains(self.master.bodyPrefab.name))
                                {
                                    if (transformed)
                                    {
                                        self.ApplyBuff(Modules.Buffs.transformBuff.buffIndex, 30);
                                    }
                                }

                            }

                            transformed = false;
                            assaultvest = false;
                            choiceband = false;
                            choicescarf = false;
                            choicespecs = false;
                            leftovers = false;
                            lifeorb = false;
                            luckyegg = false;
                            rockyhelmet = false;
                            scopelens = false;
                            shellbell = false;

                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ASSAULTVEST_NAME" && !assaultvest)
                            {
                                assaultvest = true;
                                self.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 1);
                            }
                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband)
                            {
                                choiceband = true;
                                self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 1);
                            }
                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf)
                            {
                                self.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 1);
                            }
                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs)
                            {
                                choicespecs = true;
                                self.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 1);
                            }
                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers)
                            {
                                leftovers = true;
                                self.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 1);
                            }
                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LIFEORB_NAME" && !lifeorb)
                            {
                                lifeorb = true;
                                self.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 1);
                            }
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
                            //{
                            //    luckyegg = true;
                            //    self.AddBuff(Modules.Buffs.luckyeggBuff);
                            //}
                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet)
                            {
                                rockyhelmet = true;
                                self.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 1);
                            }
                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens)
                            {
                                scopelens = true;
                                self.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 1);
                            }
                            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell)
                            {
                                shellbell = true;
                                self.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 1);
                            }



                        }


                    }

                }
           

            }
            

        }


        public void FixedUpdate()
        {

            //transformback
            if (body.hasEffectiveAuthority)
            {
                if (body.HasBuff(Modules.Buffs.transformdeBuff.buffIndex))
                {
                    if (Input.GetKeyDown(Config.transformHotkey.Value) && body.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name)
                    {
                        Chat.AddMessage("Can't <style=cIsUtility>Transform </style>back yet, wait for the debuff to expire.");
                    }
                }
                else if (!body.HasBuff(Modules.Buffs.transformdeBuff.buffIndex))
                {
                    if (Input.GetKeyDown(Config.transformHotkey.Value) && body.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name)
                    {
                        AkSoundEngine.PostEvent(1719197671, this.gameObject);
                        var oldHealth = body.healthComponent.health / body.healthComponent.fullHealth;
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

                            body = characterMaster.GetBody();

                            if (Config.copyHealth.Value && oldHealth > 0)
                                body.healthComponent.health = body.healthComponent.fullHealth * oldHealth;

                        }
                    }
                       

                }
                

            }


            //transform softlock fix
            if (body.hasEffectiveAuthority)
            {
                Chat.AddMessage("authority");
                if (body.HasBuff(Modules.Buffs.transformBuff.buffIndex))
                {

                    if (transformage1 > 1f)
                    {
                        int buffCountToApply = body.GetBuffCount(Modules.Buffs.transformBuff.buffIndex);
                        if (buffCountToApply > 1)
                        {
                            if (buffCountToApply >= 1)
                            {
                                //body.ApplyBuff(Modules.Buffs.transformBuff.buffIndex, (buffCountToApply - 1));
                                body.ApplyBuff(Modules.Buffs.transformBuff.buffIndex, buffCountToApply-1);

                                transformage1 = 0;


                            }
                        }
                        else
                        {

                            if (body.master.bodyPrefab.name == "CaptainBody")
                            {
                                body.master.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
                            }
                            //if (body.master.bodyPrefab.name == "HereticBody")
                            //{
                            //    body.master.inventory.RemoveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                            //    body.master.inventory.RemoveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                            //    body.master.inventory.RemoveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                            //    body.master.inventory.RemoveItem(RoR2Content.Items.LunarUtilityReplacement, 1);
                            //}

                            //body.master.bodyPrefab = BodyCatalog.FindBodyPrefab("DittoBody");
                            CharacterBody newbody;


                            //body = body.master.Respawn(body.master.GetBody().transform.position, body.master.GetBody().transform.rotation);

                            body.master.TransformBody("DittoBody");

                            newbody = body.master.GetBody();

                            transformed = false;

                        }
                    }

                    else transformage1 += Time.fixedDeltaTime;
                }

                if (body.HasBuff(Modules.Buffs.transformdeBuff.buffIndex))
                {

                    Chat.AddMessage("hasdebuff");
                    if (transformage1 > 1f)
                    {

                        Chat.AddMessage("buffcount" + body.GetBuffCount(Modules.Buffs.transformdeBuff.buffIndex));
                        int buffCountToApply1 = body.GetBuffCount(Modules.Buffs.transformdeBuff.buffIndex);
                        if (buffCountToApply1 > 1)
                        {
                            if (buffCountToApply1 >= 1)
                            {

                                Chat.AddMessage("applybuff for debuff");
                                body.ApplyBuff(Modules.Buffs.transformdeBuff.buffIndex, buffCountToApply1 - 1);

                                transformage1 = 0;


                            }
                        }
                        else
                        {
                            body.RemoveBuff(Modules.Buffs.transformdeBuff.buffIndex);

                        }
                    }

                    else transformage1 += Time.fixedDeltaTime;
                }

            }

            
            

            
            if (!characterMaster.gameObject)
            {
                Destroy(dittomastercon);
            }
            


        }

    }
}
