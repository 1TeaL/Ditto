using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using DittoMod.Equipment;

namespace DittoMod.Modules.Equipment
{
    class TMTransform : EquipmentBase<TMTransform>
    {
        public override string EquipmentName => "TM00 - Transform?";

        public override string EquipmentLangTokenName => "TM_TRANSFORM";

        public override string EquipmentPickupDesc => "Turn yourself into a Ditto.";

        public override string EquipmentFullDescription => "Turn yourself into a Ditto. yea.";

        public override string EquipmentLore => "The power of science enables the creation of more Ditto.";

        public override GameObject EquipmentModel => Modules.Assets.DittoEquipmentPrefab;
            
        public override Sprite EquipmentIcon => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("TMtransformSprite");

        public override bool CanDrop => true;

        public override float Cooldown => Modules.StaticValues.TransformEquipmentCooldown;

        public static GameObject ItemBodyModelPrefab;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            //no model for becomeditto.
            return rules;
        }

        public override void Hooks()
        {
            
        }

        public override void Init()
        {
            //CreateConfig(config);
            CreateLang();
            //CreateEffect();
            //CreateInteractable();
            //CreateSound();
            CreateEquipment();
            Hooks();
        }

        private void CreateEffect() {

        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            CharacterBody charBody = slot.characterBody;

            if (!slot.characterBody || !slot.characterBody.teamComponent) return false;
            if (!slot.characterBody.master) { return false; }

            var component = slot.characterBody.master.gameObject.GetComponent<DittoHandler>();
            if (!component)
            {
                slot.characterBody.master.gameObject.AddComponent<DittoHandler>();
                component = slot.characterBody.master.gameObject.GetComponent<DittoHandler>();
            }

            if (slot.stock <= 0 || !component) { return false; }

            component.BecomeDitto();
            return true;
        }

        public class DittoHandler : MonoBehaviour
        {
            private CharacterMaster characterMaster;
            private CharacterBody body;

            public void Awake()
            {
                characterMaster = this.gameObject.GetComponent<CharacterMaster>();
            }

            public void Start()
            {
                AkSoundEngine.PostEvent(1719197672, this.gameObject);
            }

            //Turns into Ditto
            public void BecomeDitto()
            {
                
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
                    characterMaster.bodyPrefab = BodyCatalog.FindBodyPrefab("DittoBody");
                    body = characterMaster.Respawn(characterMaster.GetBody().transform.position, characterMaster.GetBody().transform.rotation);

                    body.RemoveBuff(RoR2Content.Buffs.OnFire);
                    body.RemoveBuff(RoR2Content.Buffs.AffixBlue);
                    body.RemoveBuff(RoR2Content.Buffs.AffixEcho);
                    body.RemoveBuff(RoR2Content.Buffs.AffixHaunted);
                    body.RemoveBuff(RoR2Content.Buffs.AffixLunar);
                    body.RemoveBuff(RoR2Content.Buffs.AffixPoison);
                    body.RemoveBuff(RoR2Content.Buffs.AffixRed);
                    body.RemoveBuff(RoR2Content.Buffs.AffixWhite);
                    body.RemoveBuff(DittoMod.Modules.Assets.fireelitebuff);
                    body.RemoveBuff(DittoMod.Modules.Assets.iceelitebuff);
                    body.RemoveBuff(DittoMod.Modules.Assets.hauntedelitebuff);
                    body.RemoveBuff(DittoMod.Modules.Assets.lightningelitebuff);
                    body.RemoveBuff(DittoMod.Modules.Assets.mendingelitebuff);
                    body.RemoveBuff(DittoMod.Modules.Assets.malachiteelitebuff);
                    //body.RemoveBuff(DittoMod.Modules.Assets.speedelitebuff);
                    body.RemoveBuff(DittoMod.Modules.Assets.voidelitebuff);
                    body.RemoveBuff(DittoMod.Modules.Assets.lunarelitebuff);
                }
                
            }
        }
    }
}
