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
        public bool assaultvest2;
        public bool choiceband2;
        public bool choicescarf2;
        public bool choicespecs2;
        public bool leftovers2;
        public bool lifeorb2;
        public bool luckyegg2;
        public bool rockyhelmet2;
        public bool scopelens2;
        public bool shellbell2;

        private void Awake()
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
            assaultvest2 = false;
            choiceband2 = false;
            choicescarf2 = false;
            choicespecs2 = false;
            leftovers2 = false;
            lifeorb2 = false;
            luckyegg2 = false;
            rockyhelmet2 = false;
            scopelens2 = false;
            shellbell2 = false;
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
            List<string> speciallist = new List<string>();
            speciallist.Add("NullifierBody");
            speciallist.Add("VoidJailerBody");
            speciallist.Add("MinorConstructBody");
            speciallist.Add("MinorConstructOnKillBody");
            speciallist.Add("MiniVoidRaidCrabBodyPhase1");
            speciallist.Add("MiniVoidRaidCrabBodyPhase2");
            speciallist.Add("MiniVoidRaidCrabBodyPhase3");
            speciallist.Add("ElectricWormBody");
            speciallist.Add("MagmaWormBody");
            speciallist.Add("BeetleQueen2Body");
            speciallist.Add("TitanBody");
            speciallist.Add("TitanGoldBody");
            speciallist.Add("VagrantBody");
            speciallist.Add("GravekeeperBody");
            speciallist.Add("ClayBossBody");
            speciallist.Add("RoboBallBossBody");
            speciallist.Add("SuperRoboBallBossBody");
            speciallist.Add("MegaConstructBody");
            speciallist.Add("VoidInfestorBody");
            speciallist.Add("VoidBarnacleBody");
            speciallist.Add("MegaConstructBody");
            speciallist.Add("VoidMegaCrabBody");
            speciallist.Add("GrandParentBody");
            speciallist.Add("ImpBossBody");
            speciallist.Add("BrotherBody");
            speciallist.Add("BrotherHurtBody");

            if (self.master.gameObject.GetComponent<DittoMasterController>())
            {
                if (self.master.bodyPrefab != BodyCatalog.FindBodyPrefab("DittoBody"))
                {
                    if (speciallist.Contains(self.master.bodyPrefab.name))
                    {
                        if (transformed)
                        {
                            self.SetBuffCount(Modules.Buffs.transformBuff.buffIndex, 1);
                        }
                    }
                    else
                    {
                        if (Config.choiceOnTeammate.Value)
                        {
                            if (assaultvest)
                            {
                                self.AddBuff(Modules.Buffs.assaultvestBuff);
                            }
                            if (choiceband)
                            {
                                self.AddBuff(Modules.Buffs.choicebandBuff);
                            }
                            if (choicescarf)
                            {
                                self.AddBuff(Modules.Buffs.choicescarfBuff);
                            }
                            if (choicespecs)
                            {
                                self.AddBuff(Modules.Buffs.choicespecsBuff);
                            }
                            if (leftovers)
                            {
                                self.AddBuff(Modules.Buffs.leftoversBuff);
                            }
                            if (lifeorb)
                            {
                                self.AddBuff(Modules.Buffs.lifeorbBuff);
                            }
                            if (luckyegg)
                            {
                                self.AddBuff(Modules.Buffs.luckyeggBuff);
                            }
                            if (rockyhelmet)
                            {
                                self.AddBuff(Modules.Buffs.rockyhelmetBuff);
                            }
                            if (scopelens)
                            {
                                self.AddBuff(Modules.Buffs.scopelensBuff);
                            }
                            if (shellbell)
                            {
                                self.AddBuff(Modules.Buffs.shellbellBuff);
                            }
                            if (assaultvest2)
                            {
                                self.AddBuff(Modules.Buffs.assaultvestBuff);
                            }
                            if (choiceband2)
                            {
                                self.AddBuff(Modules.Buffs.choicebandBuff);
                            }
                            if (choicescarf2)
                            {
                                self.AddBuff(Modules.Buffs.choicescarfBuff);
                            }
                            if (choicespecs2)
                            {
                                self.AddBuff(Modules.Buffs.choicespecsBuff);
                            }
                            if (leftovers2)
                            {
                                self.AddBuff(Modules.Buffs.leftoversBuff);
                            }
                            if (lifeorb2)
                            {
                                self.AddBuff(Modules.Buffs.lifeorbBuff);
                            }
                            if (luckyegg2)
                            {
                                self.AddBuff(Modules.Buffs.luckyeggBuff);
                            }
                            if (rockyhelmet2)
                            {
                                self.AddBuff(Modules.Buffs.rockyhelmetBuff);
                            }
                            if (scopelens2)
                            {
                                self.AddBuff(Modules.Buffs.scopelensBuff);
                            }
                            if (shellbell2)
                            {
                                self.AddBuff(Modules.Buffs.shellbellBuff);
                            }

                        }
                    }
                }

            }

        }


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

                            //self.master.bodyPrefab = BodyCatalog.FindBodyPrefab("DittoBody");
                            CharacterBody body;


                            //body = self.master.Respawn(self.master.GetBody().transform.position, self.master.GetBody().transform.rotation);

                            self.master.TransformBody("DittoBody");

                            body = self.master.GetBody();

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
