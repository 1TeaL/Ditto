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
        private CharacterBody self;
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
            self = characterMaster.GetBody();
            
            dittomastercon = characterMaster.gameObject.GetComponent<DittoMasterController>();
            dittocon = self.gameObject.GetComponent<DittoController>();


            self.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.luckyeggBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 0, -1);
            self.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 0, -1);

            characterMaster.luck = 0;
            if (characterMaster.GetBody().skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
            {
                luckyegg = true;
                self.AddBuff(Modules.Buffs.luckyeggBuff);
                characterMaster.luck += 1f;
            }


            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ASSAULTVEST_NAME" && !assaultvest)
            {
                assaultvest = true;
                self.AddBuff(Modules.Buffs.assaultvestBuff);
            }
            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband)
            {
                choiceband = true;
                self.AddBuff(Modules.Buffs.choicebandBuff);
            }
            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf)
            {
                self.AddBuff(Modules.Buffs.choicescarfBuff);
            }
            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs)
            {
                choicespecs = true;
                self.AddBuff(Modules.Buffs.choicespecsBuff);
            }
            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers)
            {
                leftovers = true;
                self.AddBuff(Modules.Buffs.leftoversBuff);
            }
            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LIFEORB_NAME" && !lifeorb)
            {
                lifeorb = true;
                self.AddBuff(Modules.Buffs.lifeorbBuff);
            }
            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
            //{
            //    luckyegg = true;
            //    self.AddBuff(Modules.Buffs.luckyeggBuff);
            //}
            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet)
            {
                rockyhelmet = true;
                self.AddBuff(Modules.Buffs.rockyhelmetBuff);
            }
            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens)
            {
                scopelens = true;
                self.AddBuff(Modules.Buffs.scopelensBuff);
            }
            if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell)
            {
                shellbell = true;
                self.AddBuff(Modules.Buffs.shellbellBuff);
            }
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ASSAULTVEST_NAME" && !assaultvest1)
            //{
            //    assaultvest1 = true;
            //    dittocon.assaultvest1 = true;
            //    self.AddBuff(Modules.Buffs.assaultvestBuff);
            //}
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband1)
            //{
            //    choiceband1 = true;
            //    dittocon.choiceband1 = true;
            //    self.AddBuff(Modules.Buffs.choicebandBuff);
            //}
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf1)
            //{
            //    choicescarf1 = true;
            //    dittocon.choicescarf1 = true;
            //    self.AddBuff(Modules.Buffs.choicescarfBuff);
            //}
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs1)
            //{
            //    choicespecs1 = true;
            //    dittocon.choicespecs1 = true;
            //    self.AddBuff(Modules.Buffs.choicespecsBuff);
            //}
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers1)
            //{
            //    leftovers1 = true;
            //    dittocon.leftovers1 = true;
            //    self.AddBuff(Modules.Buffs.leftoversBuff);
            //}
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LIFEORB_NAME" && !lifeorb1)
            //{
            //    lifeorb1 = true;
            //    dittocon.lifeorb1 = true;
            //    self.AddBuff(Modules.Buffs.lifeorbBuff);
            //}
            ////if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg1)
            ////{
            ////    luckyegg1 = true;
            ////    self.AddBuff(Modules.Buffs.luckyeggBuff);
            ////}
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet1)
            //{
            //    rockyhelmet1 = true;
            //    dittocon.rockyhelmet1 = true;
            //    self.AddBuff(Modules.Buffs.rockyhelmetBuff);
            //}
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens1)
            //{
            //    scopelens1 = true;
            //    dittocon.scopelens1 = true;
            //    self.AddBuff(Modules.Buffs.scopelensBuff);
            //}
            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell1)
            //{
            //    shellbell1 = true;
            //    dittocon.shellbell1 = true;
            //    self.AddBuff(Modules.Buffs.shellbellBuff);
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

            self.luck = 0;
            if (luckyegg)
            {
                self.luck += 1f;
            }
            self.luck += self.inventory.GetItemCount(RoR2Content.Items.Clover);
            self.luck -= self.inventory.GetItemCount(RoR2Content.Items.LunarBadLuck);
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
                            //self.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.luckyeggBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 0);
                            //self.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 0);


                            //transformed = false;
                            //assaultvest = false;
                            //choiceband = false;
                            //choicescarf = false;
                            //choicespecs = false;
                            //leftovers = false;
                            //lifeorb = false;
                            //luckyegg = false;
                            //rockyhelmet = false;
                            //scopelens = false;
                            //shellbell = false;
                            //assaultvest1 = false;
                            //choiceband1 = false;
                            //choicescarf1 = false;
                            //choicespecs1 = false;
                            //leftovers1 = false;
                            //lifeorb1 = false;
                            //luckyegg1 = false;
                            //rockyhelmet1 = false;
                            //scopelens1 = false;
                            //shellbell1 = false;

                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
                            //{
                            //    luckyegg = true;
                            //    dittocon.luckyegg = true;
                            //    self.AddBuff(Modules.Buffs.luckyeggBuff);
                            //    characterMaster.luck += 1f;
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg1)
                            //{
                            //    luckyegg1 = true;
                            //    dittocon.luckyegg = true;
                            //    self.AddBuff(Modules.Buffs.luckyeggBuff);
                            //    characterMaster.luck += 1f;
                            //}


                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ASSAULTVEST_NAME" && !assaultvest)
                            //{
                            //    assaultvest = true;
                            //    dittocon.assaultvest = true;
                            //    self.AddBuff(Modules.Buffs.assaultvestBuff);
                            //}
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband)
                            //{
                            //    choiceband = true;
                            //    dittocon.choiceband = true;
                            //    self.AddBuff(Modules.Buffs.choicebandBuff);
                            //}
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf)
                            //{
                            //    choicescarf = true;
                            //    dittocon.choicescarf = true;
                            //    self.AddBuff(Modules.Buffs.choicescarfBuff);
                            //}
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs)
                            //{
                            //    choicespecs = true;
                            //    dittocon.choicespecs = true;
                            //    self.AddBuff(Modules.Buffs.choicespecsBuff);
                            //}
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers)
                            //{
                            //    leftovers = true;
                            //    dittocon.leftovers = true;
                            //    self.AddBuff(Modules.Buffs.leftoversBuff);
                            //}
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LIFEORB_NAME" && !lifeorb)
                            //{
                            //    lifeorb = true;
                            //    dittocon.lifeorb = true;
                            //    self.AddBuff(Modules.Buffs.lifeorbBuff);
                            //}
                            ////if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
                            ////{
                            ////    luckyegg = true;
                            ////    self.AddBuff(Modules.Buffs.luckyeggBuff);
                            ////}
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet)
                            //{
                            //    rockyhelmet = true;
                            //    dittocon.rockyhelmet = true;
                            //    self.AddBuff(Modules.Buffs.rockyhelmetBuff);
                            //}
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens)
                            //{
                            //    scopelens = true;
                            //    dittocon.scopelens = true;
                            //    self.AddBuff(Modules.Buffs.scopelensBuff);
                            //}
                            //if (self.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell)
                            //{
                            //    shellbell = true;
                            //    dittocon.shellbell = true;
                            //    self.AddBuff(Modules.Buffs.shellbellBuff);
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ASSAULTVEST_NAME" && !assaultvest1)
                            //{
                            //    assaultvest1 = true;
                            //    dittocon.assaultvest1 = true;
                            //    self.AddBuff(Modules.Buffs.assaultvestBuff);
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband1)
                            //{
                            //    choiceband1 = true;
                            //    dittocon.choiceband1 = true;
                            //    self.AddBuff(Modules.Buffs.choicebandBuff);
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf1)
                            //{
                            //    choicescarf1 = true;
                            //    dittocon.choicescarf1 = true;
                            //    self.AddBuff(Modules.Buffs.choicescarfBuff);
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs1)
                            //{
                            //    choicespecs1 = true;
                            //    dittocon.choicespecs1 = true;
                            //    self.AddBuff(Modules.Buffs.choicespecsBuff);
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers1)
                            //{
                            //    leftovers1 = true;
                            //    dittocon.leftovers1 = true;
                            //    self.AddBuff(Modules.Buffs.leftoversBuff);
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LIFEORB_NAME" && !lifeorb1)
                            //{
                            //    lifeorb1 = true;
                            //    dittocon.lifeorb1 = true;
                            //    self.AddBuff(Modules.Buffs.lifeorbBuff);
                            //}
                            ////if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg1)
                            ////{
                            ////    luckyegg1 = true;
                            ////    self.AddBuff(Modules.Buffs.luckyeggBuff);
                            ////}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet1)
                            //{
                            //    rockyhelmet1 = true;
                            //    dittocon.rockyhelmet1 = true;
                            //    self.AddBuff(Modules.Buffs.rockyhelmetBuff);
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens1)
                            //{
                            //    scopelens1 = true;
                            //    dittocon.scopelens1 = true;
                            //    self.AddBuff(Modules.Buffs.scopelensBuff);
                            //}
                            //if (self.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell1)
                            //{
                            //    shellbell1 = true;
                            //    dittocon.shellbell1 = true;
                            //    self.AddBuff(Modules.Buffs.shellbellBuff);
                            //}

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
                                self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 1);
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
                                self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 1);
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


                    }

                }
           

            }
            

        }


        public void FixedUpdate()
        {

            //transformback
            if (self.hasEffectiveAuthority)
            {
                if (Input.GetKeyDown(Config.transformHotkey.Value) && self.HasBuff(Modules.Buffs.transformdeBuff.buffIndex) && self.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name)
                {
                    Chat.AddMessage("Can't <style=cIsUtility>Transform </style>back yet, wait for the debuff to expire.");
                }
                if (Input.GetKeyDown(Config.transformHotkey.Value) && !self.HasBuff(Modules.Buffs.transformdeBuff.buffIndex) && self.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name)
                {
                    AkSoundEngine.PostEvent(1719197671, this.gameObject);
                    var oldHealth = self.healthComponent.health / self.healthComponent.fullHealth;
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

            //checkbuff

            //if (self.master.bodyPrefab.name == BodyCatalog.FindBodyPrefab("DittoBody").name && !givebuffs)
            //{
            //    givebuffs = true;
            //    if (assaultvest && assaultvest1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 1);
            //    }
            //    else if (assaultvest | assaultvest1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.assaultvestBuff.buffIndex, 1);
            //    }
            //    if (choiceband && choiceband1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 1);
            //    }
            //    else if (choiceband | choiceband1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.choicebandBuff.buffIndex, 1);
            //    }
            //    if (choicescarf && choicescarf1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 1);
            //    }
            //    else if (choicescarf | choicescarf1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.choicescarfBuff.buffIndex, 1);
            //    }
            //    if (choicespecs && choicespecs1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 1);
            //    }
            //    else if (choicespecs | choicespecs1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.choicespecsBuff.buffIndex, 1);
            //    }
            //    if (leftovers && leftovers1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 1);
            //    }
            //    else if (leftovers | leftovers1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.leftoversBuff.buffIndex, 1);
            //    }
            //    if (lifeorb && lifeorb1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 1);
            //    }
            //    else if (lifeorb | lifeorb1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.lifeorbBuff.buffIndex, 1);
            //    }
            //    //if (luckyegg && luckyegg1)
            //    //{
            //    //    self.master.luck = 0;
            //    //    self.ApplyBuff(Modules.Buffs.luckyeggBuff.buffIndex, 1);
            //    //    self.master.luck += 1f;
            //    //}
            //    //else if (luckyegg | luckyegg1)
            //    //{
            //    //    self.master.luck = 0;
            //    //    self.ApplyBuff(Modules.Buffs.luckyeggBuff.buffIndex, 1);
            //    //    self.master.luck += 1f;
            //    //}
            //    if (rockyhelmet && rockyhelmet1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 1);
            //    }
            //    else if (rockyhelmet | rockyhelmet1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.rockyhelmetBuff.buffIndex, 1);
            //    }
            //    if (scopelens && scopelens1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 1);
            //    }
            //    else if (scopelens | scopelens1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.scopelensBuff.buffIndex, 1);
            //    }
            //    if (shellbell && shellbell1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 1);
            //    }
            //    else if (shellbell | shellbell1)
            //    {
            //        self.ApplyBuff(Modules.Buffs.shellbellBuff.buffIndex, 1);
            //    }
            //}


            //transform softlock fix
            if (self.hasEffectiveAuthority)
            {
                if (self.HasBuff(Modules.Buffs.transformBuff.buffIndex))
                {

                    if (transformage1 > 1f)
                    {
                        int buffCountToApply = self.GetBuffCount(Modules.Buffs.transformBuff.buffIndex);
                        if (buffCountToApply > 1)
                        {
                            if (buffCountToApply >= 1)
                            {
                                //self.ApplyBuff(Modules.Buffs.transformBuff.buffIndex, (buffCountToApply - 1));
                                self.ApplyBuff(Modules.Buffs.transformBuff.buffIndex, buffCountToApply-1);

                                transformage1 = 0;


                            }
                        }
                        else
                        {

                            if (self.master.bodyPrefab.name == "CaptainBody")
                            {
                                self.master.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
                            }
                            //if (self.master.bodyPrefab.name == "HereticBody")
                            //{
                            //    self.master.inventory.RemoveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                            //    self.master.inventory.RemoveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                            //    self.master.inventory.RemoveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                            //    self.master.inventory.RemoveItem(RoR2Content.Items.LunarUtilityReplacement, 1);
                            //}

                            //self.master.bodyPrefab = BodyCatalog.FindBodyPrefab("DittoBody");
                            CharacterBody body;


                            //body = self.master.Respawn(self.master.GetBody().transform.position, self.master.GetBody().transform.rotation);

                            self.master.TransformBody("DittoBody");

                            body = self.master.GetBody();

                            transformed = false;

                        }
                    }

                    else transformage1 += Time.fixedDeltaTime;
                }

                if (self.HasBuff(Modules.Buffs.transformdeBuff.buffIndex))
                {

                    if (transformage1 > 1f)
                    {
                        int buffCountToApply1 = self.GetBuffCount(Modules.Buffs.transformdeBuff.buffIndex);
                        if (buffCountToApply1 > 1)
                        {
                            if (buffCountToApply1 >= 1)
                            {
                                //self.ApplyBuff(Modules.Buffs.transformBuff.buffIndex, (buffCountToApply - 1));
                                self.ApplyBuff(Modules.Buffs.transformdeBuff.buffIndex, buffCountToApply1 - 1);

                                transformage1 = 0;


                            }
                        }
                        else
                        {
                            self.RemoveBuff(Modules.Buffs.transformdeBuff.buffIndex);

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
