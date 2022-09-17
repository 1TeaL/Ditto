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
        public float transformAge;
        public float transformDebuffAge;
        public float leftoverTimer;
        public float moodyTimer;

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

        public bool flamebody;
        public bool hugepower;
        public bool levitate;
        public bool magicguard;
        public bool moody;
        public bool moxie;
        public bool multiscale;
        public bool sniper;

        public bool hookgiven;

        public NetworkInstanceId networkInstanceID;
        private bool initialized;
        private float jumpTimer;

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
            flamebody = false;
            hugepower = false; 
            levitate = false;
            magicguard = false;
            moody = false;
            moxie = false;
            multiscale = false;
            sniper = false;
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
            flamebody = false;
            hugepower = false;
            levitate = false;
            magicguard = false;
            moody = false;
            moxie = false;
            multiscale = false;
            sniper = false;
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
            flamebody = false;
            hugepower = false;
            levitate = false;
            magicguard = false;
            moody = false;
            moxie = false;
            multiscale = false;
            sniper = false;
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
                flamebody = false;
                hugepower = false;
                levitate = false;
                magicguard = false;
                moody = false;
                moxie = false;
                multiscale = false;
                sniper = false;

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

                            self.ApplyBuff(Buffs.assaultvestBuff.buffIndex, assaultvest ? 1 : 0);
                            self.ApplyBuff(Buffs.choicebandBuff.buffIndex, choiceband ? 1 : 0);
                            self.ApplyBuff(Buffs.choicescarfBuff.buffIndex, choicescarf ? 1 : 0);
                            self.ApplyBuff(Buffs.choicespecsBuff.buffIndex, choicespecs ? 1 : 0);
                            self.ApplyBuff(Buffs.leftoversBuff.buffIndex, leftovers ? 1 : 0);
                            self.ApplyBuff(Buffs.lifeorbBuff.buffIndex, lifeorb ? 1 : 0);
                            self.ApplyBuff(Buffs.luckyeggBuff.buffIndex, luckyegg ? 1 : 0);
                            self.ApplyBuff(Buffs.rockyhelmetBuff.buffIndex, rockyhelmet ? 1 : 0);
                            self.ApplyBuff(Buffs.scopelensBuff.buffIndex, scopelens ? 1 : 0);
                            self.ApplyBuff(Buffs.shellbellBuff.buffIndex, shellbell ? 1 : 0);

                            self.ApplyBuff(Buffs.flamebodyBuff.buffIndex, flamebody ? 1 : 0);
                            self.ApplyBuff(Buffs.hugepowerBuff.buffIndex, hugepower ? 1 : 0);
                            self.ApplyBuff(Buffs.levitateBuff.buffIndex, levitate ? 1 : 0);
                            self.ApplyBuff(Buffs.magicguardBuff.buffIndex, magicguard ? 1 : 0);
                            self.ApplyBuff(Buffs.moodyBuff.buffIndex, moody ? 1 : 0);
                            self.ApplyBuff(Buffs.moxieBuff.buffIndex, moxie ? 1 : 0);
                            self.ApplyBuff(Buffs.multiscaleBuff.buffIndex, multiscale ? 1 : 0);
                            self.ApplyBuff(Buffs.sniperBuff.buffIndex, sniper ? 1 : 0);


                        }
                        else if (self.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name)
                        {
                            if (Config.bossTimer.Value)
                            {
                                if (StaticValues.speciallist.Contains(self.master.bodyPrefab.name))
                                {
                                    if (transformed)
                                    {
                                        self.ApplyBuff(Buffs.transformBuff.buffIndex, 30);
                                    }
                                }

                            }


                            self.ApplyBuff(Buffs.assaultvestBuff.buffIndex, assaultvest ? 1 : 0);
                            self.ApplyBuff(Buffs.choicebandBuff.buffIndex, choiceband ? 1 : 0);
                            self.ApplyBuff(Buffs.choicescarfBuff.buffIndex, choicescarf ? 1 : 0);
                            self.ApplyBuff(Buffs.choicespecsBuff.buffIndex, choicespecs ? 1 : 0);
                            self.ApplyBuff(Buffs.leftoversBuff.buffIndex, leftovers ? 1 : 0);
                            self.ApplyBuff(Buffs.lifeorbBuff.buffIndex, lifeorb ? 1 : 0);
                            self.ApplyBuff(Buffs.luckyeggBuff.buffIndex, luckyegg ? 1 : 0);
                            self.ApplyBuff(Buffs.rockyhelmetBuff.buffIndex, rockyhelmet ? 1 : 0);
                            self.ApplyBuff(Buffs.scopelensBuff.buffIndex, scopelens ? 1 : 0);
                            self.ApplyBuff(Buffs.shellbellBuff.buffIndex, shellbell ? 1 : 0);

                            self.ApplyBuff(Buffs.flamebodyBuff.buffIndex, flamebody ? 1 : 0);
                            self.ApplyBuff(Buffs.hugepowerBuff.buffIndex, hugepower ? 1 : 0);
                            self.ApplyBuff(Buffs.levitateBuff.buffIndex, levitate ? 1 : 0);
                            self.ApplyBuff(Buffs.magicguardBuff.buffIndex, magicguard ? 1 : 0);
                            self.ApplyBuff(Buffs.moodyBuff.buffIndex, moody ? 1 : 0);
                            self.ApplyBuff(Buffs.moxieBuff.buffIndex, moxie ? 1 : 0);
                            self.ApplyBuff(Buffs.multiscaleBuff.buffIndex, multiscale ? 1 : 0);
                            self.ApplyBuff(Buffs.sniperBuff.buffIndex, sniper ? 1 : 0);


                        }


                    }

                }
           

            }
            

        }


        public void FixedUpdate()
        {
            CharacterBody body = characterMaster.GetBody();


            //Chat.AddMessage(BodyCatalog.FindBodyPrefab("DittoBody").name + "dittobody name");
            //Chat.AddMessage(characterMaster.bodyPrefab.name + "charactermaster name");

            if (characterMaster.netId == networkInstanceID)
            {
                if (body)
                {
                    //give skill buffs
                    if (!initialized && body.master.bodyPrefab.name == BodyCatalog.FindBodyPrefab("DittoBody").name)
                    {
                        initialized = true;

                        body.ApplyBuff(Buffs.assaultvestBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.choicebandBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.choicescarfBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.choicespecsBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.leftoversBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.lifeorbBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.luckyeggBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.rockyhelmetBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.scopelensBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.shellbellBuff.buffIndex, 0);

                        body.ApplyBuff(Buffs.flamebodyBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.hugepowerBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.levitateBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.magicguardBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.moodyBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.moxieBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.multiscaleBuff.buffIndex, 0);
                        body.ApplyBuff(Buffs.sniperBuff.buffIndex, 0);

                        characterMaster.luck = 0;
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
                        {
                            luckyegg = true;
                            body.AddBuff(Buffs.luckyeggBuff);
                            characterMaster.luck += 1f;
                        }


                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ASSAULTVEST_NAME" && !assaultvest)
                        {
                            assaultvest = true;
                            body.ApplyBuff(Buffs.assaultvestBuff.buffIndex, 1);
                        }
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband)
                        {
                            choiceband = true;
                            body.ApplyBuff(Buffs.choicebandBuff.buffIndex, 1);
                        }
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf)
                        {
                            body.ApplyBuff(Buffs.choicescarfBuff.buffIndex, 1);
                        }
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs)
                        {
                            choicespecs = true;
                            body.ApplyBuff(Buffs.choicespecsBuff.buffIndex, 1);
                        }
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers)
                        {
                            leftovers = true;
                            body.ApplyBuff(Buffs.leftoversBuff.buffIndex, 1);
                        }
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LIFEORB_NAME" && !lifeorb)
                        {
                            lifeorb = true;
                            body.ApplyBuff(Buffs.lifeorbBuff.buffIndex, 1);
                        }
                        //if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LUCKYEGG_NAME" && !luckyegg)
                        //{
                        //    luckyegg = true;
                        //    body.AddBuff(Buffs.luckyeggBuff);
                        //}
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet)
                        {
                            rockyhelmet = true;
                            body.ApplyBuff(Buffs.rockyhelmetBuff.buffIndex, 1);
                        }
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens)
                        {
                            scopelens = true;
                            body.ApplyBuff(Buffs.scopelensBuff.buffIndex, 1);
                        }
                        if (body.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell)
                        {
                            shellbell = true;
                            body.ApplyBuff(Buffs.shellbellBuff.buffIndex, 1);
                        }

                        if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_FLAMEBODY_NAME" && !flamebody)
                        {
                            flamebody = true;
                            body.AddBuff(Buffs.flamebodyBuff);
                        }
                        if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_HUGEPOWER_NAME" && !hugepower)
                        {
                            hugepower = true;
                            body.AddBuff(Buffs.hugepowerBuff);
                        }
                        if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEVITATE_NAME" && !levitate)
                        {
                            levitate = true;
                            body.AddBuff(Buffs.levitateBuff);
                        }
                        if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_MAGICGUARD_NAME" && !magicguard)
                        {
                            magicguard = true;
                            body.AddBuff(Buffs.magicguardBuff);
                        }
                        if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_MOODY_NAME" && !moody)
                        {
                            moody = true;
                            body.AddBuff(Buffs.moodyBuff);
                        }
                        if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_MOXIE_NAME" && !moxie)
                        {
                            moxie = true;
                            body.AddBuff(Buffs.moxieBuff);
                        }
                        if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_MULTISCALE_NAME" && !multiscale)
                        {
                            multiscale = true;
                            body.AddBuff(Buffs.multiscaleBuff);
                        }
                        if (body.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SNIPER_NAME" && !sniper)
                        {
                            sniper = true;
                            body.AddBuff(Buffs.sniperBuff);
                        }
                    }

                    //moody
                    if (body.HasBuff(Buffs.moodyBuff.buffIndex))
                    {
                        if (moodyTimer > StaticValues.moxieTimer)
                        {
                            moodyTimer = 0f;

                            int RandomBuff = UnityEngine.Random.Range(1, 4);
                            switch (RandomBuff)
                            {
                                case 1:
                                    body.ApplyBuff(Buffs.moodyArmorBuff.buffIndex, 2);
                                    break;
                                case 2:
                                    body.ApplyBuff(Buffs.moodyDamageBuff.buffIndex, 2);
                                    break;
                                case 3:
                                    body.ApplyBuff(Buffs.moodyMovespeedBuff.buffIndex, 2);
                                    break;
                                case 4:
                                    body.ApplyBuff(Buffs.moodyAttackspeedBuff.buffIndex, 2);
                                    break;
                            }

                            int RandomDebuff = UnityEngine.Random.Range(1, 4);
                            switch (RandomDebuff)
                            {
                                case 1:
                                    body.ApplyBuff(Buffs.moodyArmorDebuff.buffIndex, 1);
                                    break;
                                case 2:
                                    body.ApplyBuff(Buffs.moodyAttackspeedDebuff.buffIndex, 1);
                                    break;
                                case 3:
                                    body.ApplyBuff(Buffs.moodyDamageDebuff.buffIndex, 1);
                                    break;
                                case 4:
                                    body.ApplyBuff(Buffs.moodyMovespeedDebuff.buffIndex, 1);
                                    break;
                            }

                        }
                        else
                        {
                            moodyTimer += Time.fixedDeltaTime;
                        }
                    }

                    //leftovers
                    if (body.HasBuff(Buffs.leftoversBuff.buffIndex))
                    {
                        if (leftoverTimer > 1f)
                        {
                            leftoverTimer = 0f;
                            new LeftoversNetworked(characterMaster.netId).Send(NetworkDestination.Server);
                            //body.healthComponent.Heal(Modules.StaticValues.leftoversregen * body.healthComponent.fullHealth, new ProcChainMask(), true);
                        }
                        else
                        {
                            leftoverTimer += Time.fixedDeltaTime;
                        }
                    }

                    //levitate
                    if (body.HasBuff(Buffs.levitateBuff.buffIndex))
                    {
                        if (body.characterMotor)
                        {
                            if (!body.characterMotor.isGrounded)
                            {
                                if (body.inputBank.jump.down)
                                {
                                    jumpTimer += Time.fixedDeltaTime;
                                    if (jumpTimer > 0.5f)
                                    {
                                        if (body.characterMotor)
                                        {
                                            body.characterMotor.velocity.y = body.moveSpeed;
                                        }
                                    }
                                }

                            }
                            else if (body.characterMotor.isGrounded)
                            {
                                jumpTimer = 0f;
                            }
                        }
                    }

                    //transform back
                    if (body.HasBuff(Buffs.transformBuff.buffIndex))
                    {

                        if (transformAge > 1f)
                        {
                            int buffCountToApply = body.GetBuffCount(Buffs.transformBuff.buffIndex);
                            if (buffCountToApply > 1)
                            {
                                //body.ApplyBuff(Buffs.transformBuff.buffIndex, (buffCountToApply - 1));
                                body.ApplyBuff(Buffs.transformBuff.buffIndex, buffCountToApply - 1);

                                transformAge = 0;

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


                                //body = body.master.Respawn(body.master.GetBody().transform.position, body.master.GetBody().transform.rotation);

                                new TransformNetworked(characterMaster.netId).Send(NetworkDestination.Server);


                                transformed = false;

                            }
                        }

                        else transformAge += Time.fixedDeltaTime;
                    }

                    //transform debuff
                    if (body.HasBuff(Buffs.transformdeBuff.buffIndex))
                    {
                        if (Input.GetKeyDown(Config.transformHotkey.Value) && body.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name && body.hasEffectiveAuthority)
                        {
                            Chat.AddMessage("Can't <style=cIsUtility>Transform </style>back yet, wait for the debuff to expire.");
                        }

                        if (transformDebuffAge > 1f)
                        {

                            //Chat.AddMessage("buffcount" + body.GetBuffCount(Buffs.transformdeBuff.buffIndex));
                            int buffCountToApply1 = body.GetBuffCount(Buffs.transformdeBuff.buffIndex);
                            if (buffCountToApply1 > 1)
                            {
                                //Chat.AddMessage("applybuff for debuff");
                                body.ApplyBuff(Buffs.transformdeBuff.buffIndex, buffCountToApply1 - 1);

                                transformDebuffAge = 0;
                            }
                            else
                            {
                                body.ApplyBuff(Buffs.transformdeBuff.buffIndex, 0);

                            }
                        }
                        else transformDebuffAge += Time.fixedDeltaTime;


                    }
                    else if (!body.HasBuff(Buffs.transformdeBuff.buffIndex))
                    {
                        if (Input.GetKeyDown(Config.transformHotkey.Value) && body.master.bodyPrefab.name != BodyCatalog.FindBodyPrefab("DittoBody").name && body.hasEffectiveAuthority)
                        {
                            AkSoundEngine.PostEvent(1719197671, this.gameObject);
                            var oldHealthFraction = body.healthComponent.health / body.healthComponent.fullHealth;
                            if (characterMaster.bodyPrefab.name == "CaptainBody")
                            {
                                characterMaster.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);
                            }
                            else if (characterMaster.bodyPrefab.name == "HereticBody")
                            {
                                characterMaster.inventory.RemoveItem(RoR2Content.Items.LunarPrimaryReplacement, 1);
                                characterMaster.inventory.RemoveItem(RoR2Content.Items.LunarSecondaryReplacement, 1);
                                characterMaster.inventory.RemoveItem(RoR2Content.Items.LunarSpecialReplacement, 1);
                                characterMaster.inventory.RemoveItem(RoR2Content.Items.LunarUtilityReplacement, 1);
                            }
                            else
                            {
                                new TransformNetworked(characterMaster.netId).Send(NetworkDestination.Server);

                                CharacterBody newbody = characterMaster.GetBody();

                                if (Config.copyHealth.Value && oldHealthFraction > 0)
                                    newbody.healthComponent.health = newbody.healthComponent.fullHealth * oldHealthFraction;

                            }
                        }


                    }

                }
               

            }

            
        }

    }
}
