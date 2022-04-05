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

		private void Awake()
		{
			indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
			//On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
		}

		private void Start()
		{
			inputBank = gameObject.GetComponent<InputBankTest>();
			characterBody = gameObject.GetComponent<CharacterBody>();

			choiceband = false;
			choicescarf = false;
			choicespecs = false;
			leftovers = false;
			rockyhelmet = false;
			scopelens = false;	
			shellbell = false;

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

			if(characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICE_BAND_NAME")
            {
				characterBody.AddBuff(Modules.Buffs.choicebandBuff);
            }
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICE_SCARF_NAME")
			{
				characterBody.AddBuff(Modules.Buffs.choicescarfBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_CHOICE_SPECS_NAME")
			{
				characterBody.AddBuff(Modules.Buffs.choicespecsBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_ROCKY_HELMET_NAME")
			{
				characterBody.AddBuff(Modules.Buffs.rockyhelmetBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_LEFTOVERS_NAME")
			{
				characterBody.AddBuff(Modules.Buffs.leftoversBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SCOPE_LENS_NAME")
			{
				characterBody.AddBuff(Modules.Buffs.scopelensBuff);
			}
			if (characterBody.skillLocator.secondary.skillNameToken == DittoPlugin.developerPrefix + "_DITTO_BODY_SHELL_BELL_NAME")
			{
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
