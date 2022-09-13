using DittoMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DittoMod.Modules.Survivors
{
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(TeamComponent))]
    [RequireComponent(typeof(InputBankTest))]
    public class DittoMasterController : MonoBehaviour
    {
        public DittoMasterController dittomastercon;
        private CharacterMaster characterMaster;
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

        public NetworkInstanceId networkInstanceID;
        private bool initialized;

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
            
            networkInstanceID = characterMaster.netId;
            //Debug.Log(transformed + "istransformed");
            
            dittomastercon = characterMaster.gameObject.GetComponent<DittoMasterController>();

            initialized = false;
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
                    if(networkInstanceID == self.master.netId)
                    {

                        if (self.master.bodyPrefab.name == BodyCatalog.FindBodyPrefab("DittoBody").name)
                        {
                            givebuffs = false;


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


                            self.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, assaultvest ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, choiceband ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, choicescarf ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, choicespecs ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, leftovers ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, lifeorb ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.luckyeggBuff.buffIndex, luckyegg ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, rockyhelmet ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, scopelens ? 1 : 0);
                            self.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, shellbell ? 1 : 0);



                        }


                    }

                }
           

            }
            

        }


        public void FixedUpdate()
        {
            CharacterBody body = characterMaster.GetBody();
            if (!initialized)
            {
                initialized = true;

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
                if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
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



            if (characterMaster.netId == networkInstanceID)
            {
                //transform back
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
                                body.ApplyBuff(Modules.Buffs.transformBuff.buffIndex, buffCountToApply - 1);

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

                //transform debuff
                if (body.HasBuff(Modules.Buffs.transformdeBuff.buffIndex))
                {
                    if (Input.GetKeyDown(Config.transformHotkey.Value) && body.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name && body.hasEffectiveAuthority)
                    {
                        Chat.AddMessage("Can't <style=cIsUtility>Transform </style>back yet, wait for the debuff to expire.");
                    }

                    if (transformage1 > 1f)
                    {

                        //Chat.AddMessage("buffcount" + body.GetBuffCount(Modules.Buffs.transformdeBuff.buffIndex));
                        int buffCountToApply1 = body.GetBuffCount(Modules.Buffs.transformdeBuff.buffIndex);
                        if (buffCountToApply1 > 1)
                        {
                            if (buffCountToApply1 >= 1)
                            {

                                //Chat.AddMessage("applybuff for debuff");
                                body.ApplyBuff(Modules.Buffs.transformdeBuff.buffIndex, buffCountToApply1 - 1);

                                transformage1 = 0;


                            }
                        }
                        else
                        {
                            body.ApplyBuff(Modules.Buffs.transformdeBuff.buffIndex, 0);

                        }
                    }
                    else transformage1 += Time.fixedDeltaTime;


                }
                else if (!body.HasBuff(Modules.Buffs.transformdeBuff.buffIndex))
                {
                    if (Input.GetKeyDown(Config.transformHotkey.Value) && body.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name && body.hasEffectiveAuthority)
                    {
                        AkSoundEngine.PostEvent(1719197671, this.gameObject);
                        var oldHealthFraction = body.healthComponent.health / body.healthComponent.fullHealth;
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
                            new TransformNetworked(characterMaster.netId).Send(NetworkDestination.Server);

                            CharacterBody newbody = characterMaster.GetBody();

                            if (Config.copyHealth.Value && oldHealthFraction > 0)
                                newbody.healthComponent.health = newbody.healthComponent.fullHealth * oldHealthFraction;

                        }
                    }


                }

            }

            if (!characterMaster.gameObject)
            {
                Destroy(dittomastercon);
            }
            
        }

    }
}
