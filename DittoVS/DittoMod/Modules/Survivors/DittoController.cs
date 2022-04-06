using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DittoMod.Modules.Survivors
{
	public class DittoController : MonoBehaviour
	{
		public float maxTrackingDistance = 40f;
		public float maxTrackingAngle = 30f;
		public float trackerUpdateFrequency = 10f;
		private HurtBox trackingTarget;
		private CharacterBody characterBody;
		private InputBankTest inputBank;
		private float trackerUpdateStopwatch;
		private Indicator indicator;
		public bool transformed;
		private readonly BullseyeSearch search = new BullseyeSearch();
		private CharacterMaster characterMaster;
		private CharacterBody origCharacterBody;
		private string origName;

		public bool choiceband;
		public bool choicescarf;
		public bool choicespecs;
		public bool leftovers;
		public bool rockyhelmet;
		public bool scopelens;
		public bool shellbell;
		public bool choiceband2;
		public bool choicescarf2;
		public bool choicespecs2;
		public bool leftovers2;
		public bool rockyhelmet2;
		public bool scopelens2;
		public bool shellbell2;

		private void Awake()
		{
			indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
			//On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
			characterBody = gameObject.GetComponent<CharacterBody>();
			inputBank = gameObject.GetComponent<InputBankTest>();
			choiceband = false;
			choicescarf = false;
			choicespecs = false;
			leftovers = false;
			rockyhelmet = false;
			scopelens = false;
			shellbell = false;
			choiceband2 = false;
			choicescarf2 = false;
			choicespecs2 = false;
			leftovers2 = false;
			rockyhelmet2 = false;
			scopelens2 = false;
			shellbell2 = false;


			if (characterBody.HasBuff(RoR2Content.Buffs.OnFire))
			{
				characterBody.RemoveBuff(RoR2Content.Buffs.OnFire);
			}
			if (characterBody.HasBuff(RoR2Content.Buffs.AffixBlue))
			{
				characterBody.RemoveBuff(RoR2Content.Buffs.AffixBlue);
			}
			if (characterBody.HasBuff(RoR2Content.Buffs.AffixEcho))
			{
				characterBody.RemoveBuff(RoR2Content.Buffs.AffixEcho);
			}
			if (characterBody.HasBuff(RoR2Content.Buffs.AffixHaunted))
			{
				characterBody.RemoveBuff(RoR2Content.Buffs.AffixHaunted);
			}
			if (characterBody.HasBuff(RoR2Content.Buffs.AffixLunar))
			{
				characterBody.RemoveBuff(RoR2Content.Buffs.AffixLunar);
			}
			if (characterBody.HasBuff(RoR2Content.Buffs.AffixPoison))
			{
				characterBody.RemoveBuff(RoR2Content.Buffs.AffixPoison);
			}
			if (characterBody.HasBuff(RoR2Content.Buffs.AffixRed))
			{
				characterBody.RemoveBuff(RoR2Content.Buffs.AffixRed);
			}
			if (characterBody.HasBuff(RoR2Content.Buffs.AffixWhite))
			{
				characterBody.RemoveBuff(RoR2Content.Buffs.AffixWhite);
			}
			if (characterBody.HasBuff(DittoMod.Modules.Assets.fireelitebuff))
			{
				characterBody.RemoveBuff(DittoMod.Modules.Assets.fireelitebuff);
			}
			if (characterBody.HasBuff(DittoMod.Modules.Assets.iceelitebuff))
			{
				characterBody.RemoveBuff(DittoMod.Modules.Assets.iceelitebuff);
			}
			if (characterBody.HasBuff(DittoMod.Modules.Assets.hauntedelitebuff))
			{
				characterBody.RemoveBuff(DittoMod.Modules.Assets.hauntedelitebuff);
			}
			if (characterBody.HasBuff(DittoMod.Modules.Assets.lightningelitebuff))
			{
				characterBody.RemoveBuff(DittoMod.Modules.Assets.lightningelitebuff);
			}
			if (characterBody.HasBuff(DittoMod.Modules.Assets.mendingelitebuff))
			{
				characterBody.RemoveBuff(DittoMod.Modules.Assets.mendingelitebuff);
			}
			if (characterBody.HasBuff(DittoMod.Modules.Assets.malachiteelitebuff))
			{
				characterBody.RemoveBuff(DittoMod.Modules.Assets.malachiteelitebuff);
			}
			if (characterBody.HasBuff(DittoMod.Modules.Assets.voidelitebuff))
			{
				characterBody.RemoveBuff(DittoMod.Modules.Assets.voidelitebuff);
			}
			if (characterBody.HasBuff(DittoMod.Modules.Assets.lunarelitebuff))
			{
				characterBody.RemoveBuff(DittoMod.Modules.Assets.lunarelitebuff);
			}
		}

		private void Start()
		{


			//characterMaster = characterBody.master;

			//characterMaster.gameObject.AddComponent<DittoMasterController>();
		}

		public HurtBox GetTrackingTarget()
		{
			return this.trackingTarget;
		}

		private void OnEnable()
		{
			this.indicator.active = true;
		}

		private void OnDisable()
		{
			this.indicator.active = false;
		}
		private void FixedUpdate()
		{
			this.trackerUpdateStopwatch += Time.fixedDeltaTime;
			if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
			{
				this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
				HurtBox hurtBox = this.trackingTarget;
				Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
				this.SearchForTarget(aimRay);
				this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);
			}

			if(characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband)
            {
				choiceband = true;
				characterBody.AddBuff(Modules.Buffs.choicebandBuff);
            }
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf)
			{
				choicescarf = true;
				characterBody.AddBuff(Modules.Buffs.choicescarfBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs)
			{
				choicespecs = true;
				characterBody.AddBuff(Modules.Buffs.choicespecsBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet)
			{
				rockyhelmet = true;
				characterBody.AddBuff(Modules.Buffs.rockyhelmetBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers)
			{
				leftovers = true;
				characterBody.AddBuff(Modules.Buffs.leftoversBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens)
			{
				scopelens= true;
				characterBody.AddBuff(Modules.Buffs.scopelensBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell)
			{
				shellbell = true;
				characterBody.AddBuff(Modules.Buffs.shellbellBuff);
			}

			if (characterBody.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICEBAND_NAME" && !choiceband2)
			{
				choiceband2 = true;
				characterBody.AddBuff(Modules.Buffs.choicebandBuff);
			}
			if (characterBody.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESCARF_NAME" && !choicescarf2)
			{
				choicescarf2 = true;
				characterBody.AddBuff(Modules.Buffs.choicescarfBuff);
			}
			if (characterBody.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICESPECS_NAME" && !choicespecs2)
			{
				choicespecs2 = true;
				characterBody.AddBuff(Modules.Buffs.choicespecsBuff);
			}
			if (characterBody.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKYHELMET_NAME" && !rockyhelmet2)
			{
				rockyhelmet2 = true;
				characterBody.AddBuff(Modules.Buffs.rockyhelmetBuff);
			}
			if (characterBody.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME" && !leftovers2)
			{
				leftovers2 = true;
				characterBody.AddBuff(Modules.Buffs.leftoversBuff);
			}
			if (characterBody.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPELENS_NAME" && !scopelens2)
			{
				scopelens2 = true;
				characterBody.AddBuff(Modules.Buffs.scopelensBuff);
			}
			if (characterBody.skillLocator.utility.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELLBELL_NAME" && !shellbell2)
			{
				shellbell2 = true;
				characterBody.AddBuff(Modules.Buffs.shellbellBuff);
			}

		}

		private void SearchForTarget(Ray aimRay)
		{
			this.search.teamMaskFilter = TeamMask.all;
			this.search.filterByLoS = true;
			this.search.searchOrigin = aimRay.origin;
			this.search.searchDirection = aimRay.direction;
			this.search.sortMode = BullseyeSearch.SortMode.Distance;
			this.search.maxDistanceFilter = this.maxTrackingDistance;
			this.search.maxAngleFilter = this.maxTrackingAngle;
			this.search.RefreshCandidates();
			this.search.FilterOutGameObject(base.gameObject);
			this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();
		}
	}
}
